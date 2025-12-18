using Microsoft.Extensions.Caching.Distributed;

namespace Controllers.Services;

public class DistributedCacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public DistributedCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public Task<string?> GetStringAsync(string key, CancellationToken cancellationToken = default)
    {
        return _cache.GetStringAsync(key, cancellationToken);
    }

    public Task SetStringAsync(
        string key,
        string value,
        DistributedCacheEntryOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var cacheOptions = options ?? new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };

        return _cache.SetStringAsync(key, value, cacheOptions, cancellationToken);
    }
}
