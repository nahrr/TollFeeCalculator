using TollFeeCalculatorApp.Core.Abstractions;

namespace TollFeeCalculatorApp.Core.Rules;

public sealed class TollFreeDateProvider : ITollFreeDateProvider
{
    private readonly Dictionary<int, List<DateTime>> _tollFreeHolidaysByYear = new()
    {
        {
            2013, [
                new DateTime(2013, 1, 1), new DateTime(2013, 3, 28), new DateTime(2013, 3, 29),
                new DateTime(2013, 4, 1), new DateTime(2013, 4, 30), new DateTime(2013, 5, 1),
                new DateTime(2013, 5, 8), new DateTime(2013, 5, 9), new DateTime(2013, 6, 5),
                new DateTime(2013, 6, 6), new DateTime(2013, 6, 21), new DateTime(2013, 7, 1),
                new DateTime(2013, 12, 24), new DateTime(2013, 12, 25), new DateTime(2013, 12, 26),
                new DateTime(2013, 12, 31)
            ]
        }
    };

    public bool IsTollFreeDate(DateTime date)
    {
        if (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
        {
            return true;
        }

        return _tollFreeHolidaysByYear.TryGetValue(date.Year, out var holidays) && holidays.Contains(date.Date);
    }
}