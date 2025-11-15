using Microsoft.Extensions.Caching.Distributed;
using Teczter.Domain.Entities.interfaces;

namespace Teczter.Infrastructure.Cache;
public interface ITeczterCache<T> where T : class, IHasIntId
{
    public string KeyPrefix { get; }
    public DistributedCacheEntryOptions Options { get; }

    Task SetCache(T obj);

    Task RemoveCache(int objectId);

    Task<T?> GetCachedResult(int objectId);
}
