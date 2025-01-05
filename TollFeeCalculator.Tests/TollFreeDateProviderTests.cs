using Moq;
using TollFeeCalculator.Infrastructure.Abstractions;
using TollFeeCalculatorApp.Core.Rules;

namespace TollFeeCalculatorApp.Tests;

public class TollFreeDateProviderTests
{
    private readonly Mock<IHolidayApi> _mockHolidayApi;
    private readonly TollFreeDateProvider _provider;

    public TollFreeDateProviderTests()
    {
        _mockHolidayApi = new Mock<IHolidayApi>();
        _provider = new TollFreeDateProvider(_mockHolidayApi.Object);
    }

    [Theory]
    [InlineData(2025, 1, 18)] // Saturday
    [InlineData(2025, 1, 19)] // Sunday
    public async Task Should_Return_True_For_Weekends(int year, int month, int day)
    {
        var date = new DateTime(year, month, day);

        var result = await _provider.IsTollFreeDate(date);

        Assert.True(result);
    }

    [Fact]
    public async Task Should_Return_True_When_Date_Is_A_Holiday()
    {
        var holidayDate = new DateTime(2025, 12, 25);
        _mockHolidayApi.Setup(api => api.GetHolidaysAsync(2025))
            .ReturnsAsync([holidayDate]);

        var result = await _provider.IsTollFreeDate(holidayDate);

        Assert.True(result, "The date should be recognized as a toll-free holiday.");
    }

    [Fact]
    public async Task Should_Return_False_When_Date_Is_Not_A_Holiday()
    {
        var nonHolidayDate = new DateTime(2025, 12, 1);
        DateTime[] holidays = [new DateTime(2025, 12, 25)];

        _mockHolidayApi.Setup(api => api.GetHolidaysAsync(2025))
            .ReturnsAsync(holidays);

        var result = await _provider.IsTollFreeDate(nonHolidayDate);

        Assert.False(result, "The date should not be recognized as a toll-free holiday.");
    }
}