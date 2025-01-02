using TollFeeCalculatorApp.Abstractions;

namespace TollFeeCalculatorApp.Models
{
    public class Motorbike : IVehicle
    {
        public string GetVehicleType()
        {
            return "Motorbike";
        }
    }
}
