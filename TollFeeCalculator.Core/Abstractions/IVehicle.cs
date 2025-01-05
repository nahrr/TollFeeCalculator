using TollFeeCalculatorApp.Core.Models.Enums;

namespace TollFeeCalculatorApp.Core.Abstractions;

public interface IVehicle
{
    VehicleType  GetVehicleType();
}
