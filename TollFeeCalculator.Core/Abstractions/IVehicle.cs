using TollFeeCalculatorApp.Core.Models;

namespace TollFeeCalculatorApp.Core.Abstractions;

public interface IVehicle
{
    VehicleType  GetVehicleType();
}