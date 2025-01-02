using TollFeeCalculatorApp.Abstractions;

namespace TollFeeCalculatorApp.Models;

public class Car : IVehicle
{
    public string GetVehicleType()
    {
        return "Car";
    }
}