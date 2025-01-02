using TollFeeCalculatorApp.Core.Abstractions;

namespace TollFeeCalculatorApp.Core.Models;

public sealed class Motorbike : IVehicle
{
    public VehicleType GetVehicleType() => VehicleType.Motorbike;
}