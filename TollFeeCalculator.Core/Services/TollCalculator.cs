using TollFeeCalculatorApp.Core.Abstractions;

namespace TollFeeCalculatorApp.Core.Services;

public sealed class TollCalculator(ITollFeeRules tollFeeRules)
{
    private readonly ITollFeeRules
        _tollFeeRules = tollFeeRules ?? throw new ArgumentNullException(nameof(tollFeeRules));

    private const int MaxDailyFee = 60;
    private const int TollFree = 0;

    /**
 * Calculate the total toll fee for one day
 *
 * @param vehicle - the vehicle
 * @param dates   - date and time of all passes on one day
 * @return - the total toll fee for that day
 */
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
                maxFeeInWindow = Math.Max(maxFeeInWindow, currentFee);
            }
            else
            {
                totalFee += maxFeeInWindow;
                maxFeeInWindow = currentFee;
                intervalStart = date;
            }

            if (totalFee >= MaxDailyFee)
            {
                return MaxDailyFee;
            }
        }

        totalFee += maxFeeInWindow;

        return Math.Min(totalFee, MaxDailyFee);
    }
}