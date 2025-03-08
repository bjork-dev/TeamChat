using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using Moq;

public static class MockMemoryCacheService
{
    public static IMemoryCache GetMemoryCache()
    {
        var mockMemoryCache = new Mock<IMemoryCache>();
        var cacheStore = new ConcurrentDictionary<object, object>(); // Simulating cache storage

        // Setup TryGetValue to return stored values
        mockMemoryCache
            .Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
            .Returns((object key, out object value) =>
            {
                bool exists = cacheStore.TryGetValue(key, out var cachedValue);
                value = cachedValue;
                return exists;
            });

        // Setup CreateEntry to store values properly
        mockMemoryCache
            .Setup(m => m.CreateEntry(It.IsAny<object>()))
            .Returns((object key) =>
            {
                var cacheEntry = new Mock<ICacheEntry>();

                cacheEntry.SetupProperty(e => e.Value);

                cacheEntry.SetupSet(e => e.Value = It.IsAny<object>())
                    .Callback<object>(val => cacheStore[key] = val); // Persist value

                cacheEntry.Setup(e => e.Dispose())
                    .Callback(() => { }); // No-op to prevent disposal issues

                return cacheEntry.Object;
            });

        return mockMemoryCache.Object;
    }

    public static void MockSet<TItem>(this IMemoryCache cache, object key, TItem value)
    {
        var entry = cache.CreateEntry(key);
        entry.Value = value; // This will trigger our Callback and store value
        entry.Dispose(); // Simulates real cache behavior
    }
}