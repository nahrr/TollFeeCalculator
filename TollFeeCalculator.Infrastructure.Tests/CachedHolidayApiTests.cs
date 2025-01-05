using Microsoft.Extensions.Caching.Memory;
using Moq;
using TollFeeCalculator.Infrastructure.Abstractions;

namespace TollFeeCalculator.Infrastructure.Tests;

public class CachedHolidayApiTests
{
    private readonly Mock<IHolidayApi> _mockInnerApi;
    private readonly Mock<IMemoryCache> _mockCache;
    private readonly CachedHolidayApi _cachedApi;

    public CachedHolidayApiTests()
    {
        _mockInnerApi = new Mock<IHolidayApi>();
        _mockCache = new Mock<IMemoryCache>();
        var mockCacheEntry = new Mock<ICacheEntry>();
        _mockCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(mockCacheEntry.Object);

        _cachedApi = new CachedHolidayApi(_mockCache.Object, _mockInnerApi.Object);
    }

    [Fact]
    public async Task Should_Return_Holidays_From_Cache_If_Available()
    {
        // Arrange
        List<DateTime> cachedHolidays = [new DateTime(2025, 1, 1)];
        object? cachedHolidaysAsObject = cachedHolidays;
        _mockCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedHolidaysAsObject))
            .Returns(true);

        // Act
        _ = await _cachedApi.GetHolidaysAsync(2025);

        // Assert
        _mockInnerApi.Verify(api => api.GetHolidaysAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Should_Fetch_Holidays_From_Api_If_Not_In_Cache_And_Store_In_Cache()
    {
        // Arrange
        List<DateTime> holidays = [new DateTime(2025, 1, 1)];
        object? nullObject = null;
        _mockCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out nullObject)).Returns(false);
        _mockInnerApi.Setup(api => api.GetHolidaysAsync(2025)).ReturnsAsync(holidays);

        // Act
        var result = await _cachedApi.GetHolidaysAsync(2025);

        // Assert
        Assert.Equal(holidays, result);
        _mockInnerApi.Verify(api => api.GetHolidaysAsync(2025), Times.Once);
        _mockCache.Verify(m => m.CreateEntry(It.IsAny<object>()), Times.Once);
    }
}