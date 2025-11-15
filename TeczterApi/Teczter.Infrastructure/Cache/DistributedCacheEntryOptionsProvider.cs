using Microsoft.Extensions.Caching.Distributed;
using Teczter.Domain.Entities;

namespace Teczter.Infrastructure.Cache;
internal static class DistributedCacheEntryOptionsProvider
{
    private static readonly Dictionary<string, int> TimeToLive = new()
    {
        [nameof(TestEntity)] = 30
    };

    internal static DistributedCacheEntryOptions GetOptionsForType<T>()
    {        
        if (TimeToLive.TryGetValue(typeof(T).Name, out var value))
        {
            return new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(value)
            };
        }

        throw new ArgumentException($"Time to live configuration does not exist for {typeof(T).Name} type.");
    }
}
