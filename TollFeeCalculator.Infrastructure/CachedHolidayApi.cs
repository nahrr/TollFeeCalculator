using TollFeeCalculator.Infrastructure.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace TollFeeCalculator.Infrastructure;

public sealed class CachedHolidayApi(IMemoryCache cache, IHolidayApi innerApi) : IHolidayApi
{
    public async Task<IEnumerable<DateTime>> GetHolidaysAsync(int year)
    {
        var cacheKey = $"Holidays_{year}";
        if (cache.TryGetValue<List<DateTime>>(cacheKey, out var cachedHolidays) && cachedHolidays is not null)
        {
            return cachedHolidays;
        }

        var holidays = await innerApi.GetHolidaysAsync(year);

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromDays(30))
            .SetAbsoluteExpiration(TimeSpan.FromDays(365));

        var holidaysAsync = holidays.ToList();
        cache.Set(cacheKey, holidaysAsync.ToList(), cacheEntryOptions);
        return holidaysAsync;
    }
}