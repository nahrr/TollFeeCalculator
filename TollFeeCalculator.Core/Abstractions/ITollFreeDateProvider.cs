namespace TollFeeCalculatorApp.Core.Abstractions;

public interface ITollFreeDateProvider
{
    bool IsTollFreeDate(DateTime date);
}
