using TollFeeCalculator.Infrastructure.Abstractions;
using TollFeeCalculatorApp.Core.Abstractions;

namespace TollFeeCalculatorApp.Core.Rules;

public sealed class TollFreeDateProvider(IHolidayApi holidayApi) : ITollFreeDateProvider
{
    private readonly IHolidayApi _holidayApi = holidayApi ?? throw new ArgumentNullException(nameof(holidayApi));

    public async Task<bool> IsTollFreeDate(DateTime date)
    {
        if (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
        {
            return true;
        }

        var holidays = await _holidayApi.GetHolidaysAsync(date.Year);
        return holidays.Contains(date.Date);
    }
}