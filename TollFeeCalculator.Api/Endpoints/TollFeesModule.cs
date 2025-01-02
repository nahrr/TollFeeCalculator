using TollFeeCalculatorApp.Api.Mappers;
using TollFeeCalculatorApp.Api.Requests;
using TollFeeCalculatorApp.Api.Responses;
using TollFeeCalculatorApp.Core.Services;

namespace TollFeeCalculatorApp.Api.Endpoints;

public static class TollFeesModule
{
    public static void AddTollFeeEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/toll-fee", ([AsParameters] GetTollFeeRequest request, TollCalculator tollCalculator) =>
            {
                if (string.IsNullOrWhiteSpace(request.VehicleType))
                {
                    return Results.BadRequest("Vehicle type is required.");
                }

                if (request.TollPassTimes.Length == 0)
                {
                    return Results.BadRequest("At least one valid toll pass time is required.");
                }

                var vehicle = VehicleMapper.Map(request.VehicleType);
                var totalFee = tollCalculator.GetTollFee(vehicle, request.TollPassTimes.ToArray());

                return Results.Ok(new GetTollFeeResponse(totalFee));
            })
            .Produces<GetTollFeeResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("GetTollFee")
            .WithTags("TollFee");
    }
}