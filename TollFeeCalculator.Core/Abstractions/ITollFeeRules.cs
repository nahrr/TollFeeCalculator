namespace TollFeeCalculatorApp.Core.Abstractions;

public interface ITollFeeRules
{
    bool IsTollFreeVehicle(IVehicle? vehicle);
    bool IsTollFreeDate(DateTime date);
    int GetFeeForTime(DateTime date);
}