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
    public int GetTollFee(IVehicle vehicle, DateTime[] dates)
    {
        if (_tollFeeRules.IsTollFreeVehicle(vehicle))
        {
            return TollFree;
        }

        var totalFee = 0;
        var intervalStart = dates.FirstOrDefault();
        var maxFeeInWindow = 0;

        foreach (var date in dates.OrderBy(d => d))
        {
            if (_tollFeeRules.IsTollFreeDate(date))
            {
                continue;
            }

            var currentFee = _tollFeeRules.GetFeeForTime(date);
            var minutesDifference = (date - intervalStart).TotalMinutes;

            if (minutesDifference <= 60)
            {
                // Accumulate the maximum fee within the 60-minute window
                maxFeeInWindow = Math.Max(maxFeeInWindow, currentFee);
            }
            else
            {
                // Once out of the 60-minute window, add the max fee and reset for the next window
                totalFee += maxFeeInWindow;
                maxFeeInWindow = currentFee; // Start a new fee window
                intervalStart = date;
            }
        }

        // Adding the last computed maximum fee after the loop
        totalFee += maxFeeInWindow;

        return Math.Min(totalFee, MaxDailyFee);
    }
}