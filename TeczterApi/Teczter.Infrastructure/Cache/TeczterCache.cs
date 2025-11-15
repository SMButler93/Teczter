using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Teczter.Domain.Entities.interfaces;

namespace Teczter.Infrastructure.Cache;
public class TeczterCache<T>(IDistributedCache _cache) : ITeczterCache<T> where T : class, IHasIntId
{
    public string KeyPrefix => $"{typeof(T).Name}:";

    public DistributedCacheEntryOptions Options => DistributedCacheEntryOptionsProvider.GetOptionsForType<T>();

    public async Task<T?> GetCachedResult(int objectId)
    {
        var key = KeyPrefix + objectId;
        var json = await _cache.GetStringAsync(key);

        if (json is null)
        {
            return null;
        }

        return JsonSerializer.Deserialize<T>(json);
    }

    public async Task RemoveCache(int objectId)
    {
        var key = KeyPrefix + objectId;
        await _cache.RemoveAsync(key);
    }

    public async Task SetCache(T obj)
    {
        var objectId = obj.Id;
        var key = KeyPrefix + objectId;
        var json = JsonSerializer.Serialize(obj);
        await _cache.SetStringAsync(key, json, Options);
    }
}
