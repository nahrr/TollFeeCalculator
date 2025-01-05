namespace TollFeeCalculatorApp.Core.Abstractions;

public interface ITollFreeDateProvider
{
    Task<bool> IsTollFreeDate(DateTime date);
}

