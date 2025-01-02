using TollFeeCalculatorApp.Core.Abstractions;

namespace TollFeeCalculatorApp.Core.Models
{
    public class Motorbike : IVehicle
    {
        public string GetVehicleType()
        {
            return "Motorbike";
        }
    }
}
