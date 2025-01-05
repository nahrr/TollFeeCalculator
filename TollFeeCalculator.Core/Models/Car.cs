using TollFeeCalculatorApp.Core.Abstractions;
using TollFeeCalculatorApp.Core.Models.Enums;

namespace TollFeeCalculatorApp.Core.Models;

public sealed class Car : IVehicle
{
    public VehicleType GetVehicleType() =>
        VehicleType.Car;
}
