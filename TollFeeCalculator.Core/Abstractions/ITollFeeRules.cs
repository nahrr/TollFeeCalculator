namespace TollFeeCalculatorApp.Core.Abstractions;

public interface ITollFeeRules
{
    bool IsTollFreeVehicle(IVehicle? vehicle);
    Task<bool> IsTollFreeDate(DateTime date);
    int GetFeeForTime(DateTime date);
}
