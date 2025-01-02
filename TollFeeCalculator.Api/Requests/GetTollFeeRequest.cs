namespace TollFeeCalculatorApp.Api.Requests;

public sealed record GetTollFeeRequest(string VehicleType, DateTime[] TollPassTimes);