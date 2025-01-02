using TollFeeCalculatorApp.Core.Abstractions;

namespace TollFeeCalculatorApp.Core.Models;

public class Car : IVehicle
{
    public string GetVehicleType()
    {
        return "Car";
    }
}