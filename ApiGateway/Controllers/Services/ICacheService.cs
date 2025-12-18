using Microsoft.Extensions.Caching.Distributed;

namespace Controllers.Services;

public interface ICacheService
{
    Task<string?> GetStringAsync(string key, CancellationToken cancellationToken = default);

    Task SetStringAsync(
        string key,
        string value,
        DistributedCacheEntryOptions? options = null,
        CancellationToken cancellationToken = default);
}
