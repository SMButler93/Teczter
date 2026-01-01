using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Teczter.Domain.Entities.interfaces;

namespace Teczter.Infrastructure.Cache;
public class TeczterCache<T>(IDistributedCache cache) : ITeczterCache<T> where T : class, IHasIntId
{
    public string KeyPrefix => $"{typeof(T).Name}:";

    public DistributedCacheEntryOptions Options => DistributedCacheEntryOptionsProvider.GetOptionsForType<T>();

    public async Task<T?> GetCachedResult(int objectId, CancellationToken ct)
    {
        var key = KeyPrefix + objectId;
        var json = await cache.GetStringAsync(key, ct);

        return json is null ? null : JsonSerializer.Deserialize<T>(json);
    }

    public async Task RemoveCache(int objectId, CancellationToken ct)
    {
        var key = KeyPrefix + objectId;
        await cache.RemoveAsync(key, ct);
    }

    public async Task SetCache(T obj, CancellationToken ct)
    {
        var objectId = obj.Id;
        var key = KeyPrefix + objectId;
        var json = JsonSerializer.Serialize(obj);
        await cache.SetStringAsync(key, json, Options, ct);
    }
}
