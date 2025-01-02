using Moq;
using TollFeeCalculatorApp.Abstractions;
using TollFeeCalculatorApp.Models;
using TollFeeCalculatorApp.Services;

namespace TollFeeCalculatorApp.Tests;

using Xunit;

public class TollCalculatorTests
{
    private readonly Mock<ITollFeeRules> _mockTollFeeRules;
    private readonly TollCalculator _tollCalculator;

    public TollCalculatorTests()
    {
        _mockTollFeeRules = new Mock<ITollFeeRules>();
        _tollCalculator = new TollCalculator(_mockTollFeeRules.Object);
    }

    [Fact]
    public void Should_Return_Zero_For_TollFreeVehicle()
    {
        _mockTollFeeRules.Setup(r => r.IsTollFreeVehicle(It.IsAny<IVehicle>())).Returns(true);

        var motorbike = new Motorbike();
        DateTime[] dates = [new DateTime(2025, 01, 2, 6, 0, 0)];

        var result = _tollCalculator.GetTollFee(motorbike, dates);

        Assert.Equal(0, result);
    }

    [Fact]
    public void Should_Return_Zero_For_TollFreeDate()
    {
        _mockTollFeeRules.Setup(r => r.IsTollFreeDate(It.IsAny<DateTime>())).Returns(true);
        var car = new Car();
        DateTime[] dates = [new DateTime(2025, 7, 1, 6, 0, 0)];

        var result = _tollCalculator.GetTollFee(car, dates);

        Assert.Equal(0, result);
    }

    [Fact]
    public void Should_Calculate_Fee_For_SinglePass()
    {
        _mockTollFeeRules.Setup(r => r.IsTollFreeDate(It.IsAny<DateTime>())).Returns(false);
        _mockTollFeeRules.Setup(r => r.IsTollFreeVehicle(It.IsAny<IVehicle>())).Returns(false);
        _mockTollFeeRules.Setup(r => r.GetFeeForTime(It.IsAny<DateTime>())).Returns(8);

        var car = new Car();
        DateTime[] dates = [new DateTime(2025, 01, 20, 6, 0, 0)];

        var result = _tollCalculator.GetTollFee(car, dates);

        Assert.Equal(8, result);
    }

    [Fact]
    public void Should_Cap_Fee_At_MaxDailyLimit()
    {
        _mockTollFeeRules.Setup(r => r.IsTollFreeDate(It.IsAny<DateTime>())).Returns(false);
        _mockTollFeeRules.Setup(r => r.IsTollFreeVehicle(It.IsAny<IVehicle>())).Returns(false);
        _mockTollFeeRules.Setup(r => r.GetFeeForTime(It.IsAny<DateTime>())).Returns(15);

        var car = new Car();
        var passes = GeneratePasses(new DateTime(2025, 1, 20), 6, 18, 15);

        var result = _tollCalculator.GetTollFee(car, passes);

        Assert.Equal(60, result);
    }

    [Theory]
    [InlineData(2013, 1, 1)]
    [InlineData(2013, 3, 28)]
    [InlineData(2013, 3, 29)]
    [InlineData(2013, 4, 1)]
    [InlineData(2013, 4, 30)]
    [InlineData(2013, 5, 1)]
    [InlineData(2013, 5, 8)]
    [InlineData(2013, 6, 5)]
    [InlineData(2013, 6, 6)]
    [InlineData(2013, 6, 21)]
    [InlineData(2013, 7, 1)]
    [InlineData(2013, 7, 31)]
    [InlineData(2013, 12, 24)]
    [InlineData(2013, 12, 25)]
    [InlineData(2013, 12, 26)]
    [InlineData(2013, 12, 31)]
    public void Should_Return_Zero_For_TollFreeDates_2013(int year, int month, int day)
    {
        _mockTollFeeRules.Setup(r => r.IsTollFreeDate(It.IsAny<DateTime>())).Returns(true);


        var date = new DateTime(year, month, day);
        var car = new Car();
        var result = _tollCalculator.GetTollFee(car, [date]);

        Assert.Equal(0, result);
    }

    [Theory]
    [InlineData(2013, 1, 2, 6, 30, 13)] // Not a holiday, 06:30 -> 13 kr
    [InlineData(2013, 11, 11, 8, 0, 13)] // Regular date, 08:00 -> 13 kr
    public void Should_Calculate_Fee_For_NonTollFreeDates(
        int year,
        int month,
        int day,
        int hour,
        int minute,
        int expectedFee)
    {
        _mockTollFeeRules.Setup(r => r.IsTollFreeDate(It.IsAny<DateTime>())).Returns(false);
        _mockTollFeeRules.Setup(r => r.IsTollFreeVehicle(It.IsAny<IVehicle>())).Returns(false);
        _mockTollFeeRules.Setup(r => r.GetFeeForTime(It.IsAny<DateTime>())).Returns(expectedFee);


        var date = new DateTime(year, month, day, hour, minute, 0);
        var car = new Car();

        var result = _tollCalculator.GetTollFee(car, [date]);

        Assert.Equal(expectedFee, result);
    }

    [Fact]
    public void Should_Handle_Empty_Pass_List()
    {
        var car = new Car();
        DateTime[] dates = [];

        var result = _tollCalculator.GetTollFee(car, dates);

        Assert.Equal(0, result);
    }

    [Fact]
    public void Should_Use_Highest_Fee_In_60_Minute_Window()
    {
        _mockTollFeeRules.Setup(r => r.IsTollFreeDate(It.IsAny<DateTime>())).Returns(false);
        _mockTollFeeRules.Setup(r => r.IsTollFreeVehicle(It.IsAny<IVehicle>())).Returns(false);
        _mockTollFeeRules
            .Setup(r => r.GetFeeForTime(It.IsAny<DateTime>()))
            .Returns<DateTime>(d => d.Minute == 0 ? 8 : 13); // Return higher fee for a specific time

        var car = new Car();
        DateTime[] dates =
        [
            new DateTime(2025, 1, 20, 6, 0, 0), // 8 kr
            new DateTime(2025, 1, 20, 6, 45, 0) // 13 kr, within the same 60-minute window
        ];

        var result = _tollCalculator.GetTollFee(car, dates);

        Assert.Equal(13, result);
    }

    private static DateTime[] GeneratePasses(DateTime baseDate, int startHour, int endHour, int intervalMinutes)
    {
        var passes = new List<DateTime>();
        for (var hour = startHour; hour <= endHour; hour++)
        {
            for (var minute = 0; minute < 60; minute += intervalMinutes)
            {
                passes.Add(new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, hour, minute, 0));
            }
        }

        return passes.ToArray();
    }
}