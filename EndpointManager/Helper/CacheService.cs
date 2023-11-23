using Microsoft.Extensions.Caching.Memory;

namespace EndpointManager.Helper
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public bool TryGetValue<Item>(string key, out Item value)
        {
            return _memoryCache.TryGetValue(key, out value);
        }

        public void Set<Item>(string key, Item value)
        {
            _memoryCache.Set(key, value);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
