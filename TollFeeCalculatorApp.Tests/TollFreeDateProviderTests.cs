using TollFeeCalculatorApp.Rules;

namespace TollFeeCalculatorApp.Tests;

public class TollFreeDateProviderTests
{
    private readonly TollFreeDateProvider _provider = new();

    [Theory]
    [InlineData(2025, 1, 18)] // Saturday
    [InlineData(2025, 1, 19)] // Sunday
    public void Should_Return_True_For_Weekends(int year, int month, int day)
    {
        var date = new DateTime(year, month, day);

        var result = _provider.IsTollFreeDate(date);

        Assert.True(result);
    }

    [Theory]
    [InlineData(2013, 1, 1)] // New Year's Day
    [InlineData(2013, 3, 28)] // Specific toll-free holiday
    [InlineData(2013, 12, 25)] // Christmas Day
    public void Should_Return_True_For_TollFreeHolidays(int year, int month, int day)
    {
        var date = new DateTime(year, month, day);

        var result = _provider.IsTollFreeDate(date);

        Assert.True(result);
    }

    [Theory]
    [InlineData(2025, 1, 20)] // Regular weekday
    [InlineData(2013, 5, 15)] // Non-holiday weekday in 2013
    public void Should_Return_False_For_RegularDays(int year, int month, int day)
    {
        var date = new DateTime(year, month, day);

        var result = _provider.IsTollFreeDate(date);

        Assert.False(result);
    }
}