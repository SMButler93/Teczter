using Microsoft.Extensions.Caching.Distributed;
using Teczter.Domain.Entities.interfaces;

namespace Teczter.Infrastructure.Cache;
public interface ITeczterCache<T> where T : class, IHasIntId
{
    public string KeyPrefix { get; }
    public DistributedCacheEntryOptions Options { get; }

    Task SetCache(T obj, CancellationToken ct);

    Task RemoveCache(int objectId, CancellationToken ct);

    Task<T?> GetCachedResult(int objectId, CancellationToken ct);
}
