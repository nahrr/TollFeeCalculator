using TollFeeCalculatorApp.Core.Abstractions;

namespace TollFeeCalculatorApp.Core.Models;

public sealed class Car : IVehicle
{
    public VehicleType GetVehicleType() =>
        VehicleType.Car;
}