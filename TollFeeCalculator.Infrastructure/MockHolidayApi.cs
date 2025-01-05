using TollFeeCalculator.Infrastructure.Abstractions;

namespace TollFeeCalculator.Infrastructure;

public class MockHolidayApi : IHolidayApi
{
    /// <summary>
    /// Asynchronously retrieves a list of holidays for a given year, including the day before each holiday,
    /// unless it falls on a Sunday or is already a holiday.
    /// </summary>
    /// <param name="year">The year for which holidays are requested.</param>
    /// <returns>A task that represents the asynchronous operation and wraps the list of holidays.</returns>
    public Task<IEnumerable<DateTime>> GetHolidaysAsync(int year)
    {
        // Hardcoded list of holidays for 2025
        List<DateTime> holidays =
        [
            new DateTime(2025, 1, 1), // New Year's Day
            new DateTime(2025, 1, 6), // Epiphany
            new DateTime(2025, 4, 18), // Good Friday
            new DateTime(2025, 4, 20), // Easter Sunday
            new DateTime(2025, 4, 21), // Easter Monday
            new DateTime(2025, 5, 1), // May Day
            new DateTime(2025, 5, 29), // Ascension Day
            new DateTime(2025, 6, 6), // National Day of Sweden
            new DateTime(2025, 6, 8), // Pentecost
            new DateTime(2025, 6, 21), // Midsummer's Day
            new DateTime(2025, 11, 1), // All Saints' Day
            new DateTime(2025, 12, 25), // Christmas Day
            new DateTime(2025, 12, 26) // Boxing Day
        ];

        var holidaysWithDayBefore = holidays
            .Select(h => h.AddDays(-1))
            .Where(dayBefore => dayBefore.DayOfWeek != DayOfWeek.Sunday && !holidays.Contains(dayBefore));
        
        return Task.FromResult(holidaysWithDayBefore);
    }
}

