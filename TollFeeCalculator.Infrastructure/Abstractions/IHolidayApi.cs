namespace TollFeeCalculator.Infrastructure.Abstractions;

public interface IHolidayApi
{
    Task<IEnumerable<DateTime>> GetHolidaysAsync(int year);
}