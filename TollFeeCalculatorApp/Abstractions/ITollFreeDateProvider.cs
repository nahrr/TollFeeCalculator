namespace TollFeeCalculatorApp.Abstractions;

public interface ITollFreeDateProvider
{
    bool IsTollFreeDate(DateTime date);
}
