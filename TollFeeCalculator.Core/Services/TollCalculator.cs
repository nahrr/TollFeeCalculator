using TollFeeCalculatorApp.Core.Abstractions;

namespace TollFeeCalculatorApp.Core.Services;

public sealed class TollCalculator(ITollFeeRules tollFeeRules)
{
    private readonly ITollFeeRules
        _tollFeeRules = tollFeeRules ?? throw new ArgumentNullException(nameof(tollFeeRules));

    private const int MaxDailyFee = 60;
    private const int TollFree = 0;

    /// <summary>
    /// Calculate the total toll fee for one day.
    /// </summary>
    /// <param name="vehicle">The vehicle.</param>
    /// <param name="dates">Date and time of all passes on one day.</param>
    /// <returns>The total toll fee for that day.</returns>
    public int GetTollFee(IVehicle vehicle, IEnumerable<DateTime> dates)
    {
        if (_tollFeeRules.IsTollFreeVehicle(vehicle))
        {
            return TollFree;
        }

        var totalFee = 0;
        var sortedDates = dates.OrderBy(d => d).ToArray(); 
        var intervalStart = sortedDates.FirstOrDefault();
        var maxFeeInWindow = 0;

        foreach (var date in sortedDates)
        {
            if (_tollFeeRules.IsTollFreeDate(date))
            {
                continue;
            }

            var currentFee = _tollFeeRules.GetFeeForTime(date);
            var minutesDifference = (date - intervalStart).TotalMinutes;

            if (minutesDifference > 60)
            {
                totalFee += maxFeeInWindow;
                maxFeeInWindow = 0;
                intervalStart = date;
            }

            maxFeeInWindow = Math.Max(maxFeeInWindow, currentFee);
        }

        totalFee += maxFeeInWindow;

        return Math.Min(totalFee, MaxDailyFee);
    }
}