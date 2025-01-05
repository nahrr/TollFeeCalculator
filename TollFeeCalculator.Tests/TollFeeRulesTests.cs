using Moq;
using TollFeeCalculatorApp.Core.Abstractions;
using TollFeeCalculatorApp.Core.Models;
using TollFeeCalculatorApp.Core.Rules;

namespace TollFeeCalculatorApp.Tests;

public class TollFeeRulesTests
{
    private readonly Mock<ITollFreeDateProvider> _mockDateProvider;
    private readonly TollFeeRules _tollFeeRules;

    public TollFeeRulesTests()
    {
        _mockDateProvider = new Mock<ITollFreeDateProvider>();
        _tollFeeRules = new TollFeeRules(_mockDateProvider.Object);
    }

    [Theory]
    [InlineData(VehicleType.Motorbike, true)]
    [InlineData(VehicleType.Tractor, true)]
    [InlineData(VehicleType.Emergency, true)]
    [InlineData(VehicleType.Diplomat, true)]
    [InlineData(VehicleType.Foreign, true)]
    [InlineData(VehicleType.Military, true)]
    [InlineData(VehicleType.Car, false)]
    public void IsTollFreeVehicle_Should_Return_Correct_Result(VehicleType vehicleType, bool expected)
    {
        var vehicle = new MockVehicle(vehicleType);

        var result = _tollFeeRules.IsTollFreeVehicle(vehicle);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task IsTollFreeDate_Should_Use_DateProvider()
    {
        var testDate = new DateTime(2025, 1, 20);
        _mockDateProvider.Setup(p => p.IsTollFreeDate(testDate)).ReturnsAsync(true);

        var result = await _tollFeeRules.IsTollFreeDate(testDate);

        Assert.True(result);
        _mockDateProvider.Verify(p => p.IsTollFreeDate(testDate), Times.Once);
    }

    [Theory]
    [InlineData(6, 0, 8)] // 6:00 - 6:29 -> 8 kr
    [InlineData(6, 30, 13)] // 6:30 - 6:59 -> 13 kr
    [InlineData(7, 0, 18)] // 7:00 - 7:59 -> 18 kr
    [InlineData(8, 30, 8)] // 8:30 - 14:59 -> 8 kr
    [InlineData(18, 30, 0)] // 18:30 - 05:59 -> 0 kr
    public void GetFeeForTime_Should_Return_Correct_Fee(int hour, int minute, int expectedFee)
    {
        var date = new DateTime(2025, 1, 20, hour, minute, 0);

        var result = _tollFeeRules.GetFeeForTime(date);

        Assert.Equal(expectedFee, result);
    }

    [Fact]
    public void GetFeeForTime_Should_Return_Zero_For_Times_Outside_All_Ranges()
    {
        var date = new DateTime(2025, 1, 20, 19, 0, 0); // 19:00 -> Outside any range

        var result = _tollFeeRules.GetFeeForTime(date);

        Assert.Equal(0, result);
    }
}

public class MockVehicle(VehicleType type) : IVehicle
{
    public VehicleType GetVehicleType()
    {
        return type;
    }
}