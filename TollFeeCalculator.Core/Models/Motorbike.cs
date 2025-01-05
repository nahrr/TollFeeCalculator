using TollFeeCalculatorApp.Core.Abstractions;
using TollFeeCalculatorApp.Core.Models.Enums;

namespace TollFeeCalculatorApp.Core.Models;

public sealed class Motorbike : IVehicle
{
    public VehicleType GetVehicleType() => VehicleType.Motorbike;
}
