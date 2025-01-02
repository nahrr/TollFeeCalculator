using TollFeeCalculatorApp.Abstractions;
using TollFeeCalculatorApp.Mappers;
using TollFeeCalculatorApp.Middlewares;
using TollFeeCalculatorApp.Models;
using TollFeeCalculatorApp.Rules;
using TollFeeCalculatorApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IVehicle, Car>();
builder.Services.AddTransient<IVehicle, Motorbike>();
builder.Services.AddScoped<TollCalculator>();
builder.Services.AddSingleton<ITollFreeDateProvider, TollFreeDateProvider>();
builder.Services.AddSingleton<ITollFeeRules, TollFeeRules>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


var app = builder.Build();

app.UseExceptionHandler();

app.MapGet("/toll-fee", (HttpContext context, TollCalculator tollCalculator) =>
{
    var query = context.Request.Query;

    var vehicleType = query["VehicleType"].FirstOrDefault();
    if (string.IsNullOrWhiteSpace(vehicleType))
    {
        return Results.BadRequest("VehicleType is required.");
    }
    
    var vehicle = VehicleMapper.Map(vehicleType);

    var passes = query["Passes"]
        .Where(p => DateTime.TryParse(p, out _)).Select(x => DateTime.Parse(x ?? string.Empty)).ToArray();

    if (passes.Length == 0)
    {
        return Results.BadRequest("At least one valid Pass is required.");
    }

    var totalFee = tollCalculator.GetTollFee(vehicle, passes);

    return Results.Ok(new CalculateTollFeeResponse(totalFee));
});


app.Run();

public sealed record CalculateTollFeeResponse(int TotalFee);

