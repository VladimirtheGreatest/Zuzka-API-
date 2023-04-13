using Microsoft.Extensions.Caching.Memory;
using Zuzka.Services.Contracts;

namespace Zuzka.Services
{
    /// <summary>
    /// To speed up the READ queries we will be using cache service, this implementation can be easily changed to use Redis or another production based scenario or add an extra provider.
    /// </summary>
    public class CacheService : ICacheService
    {
        private readonly MemoryCache _cache;
        public CacheService()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        public T Get<T>(string key)
        {
            if (_cache.TryGetValue(key, out T value))
            {
                return value;
            }
            return default(T);
        }

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            _cache.Set(key, value, expiration);
        }
    }
}
