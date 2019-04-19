using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace MemoryCacheNetFull.Lib
{
    public sealed class InMemoryCache : IDisposable
    {
        private readonly MemoryCache _cache;
        private const int CacheSecondsDuration = 5;

        public InMemoryCache(string name)
        {
            _cache = new MemoryCache(name);
        }

        public long Count()
        {
            return _cache.GetCount();
        }

        public async Task<T> AddOrGetAsync<T>(string key, Func<Task<T>> func)
        {
            var newValue = new Lazy<Task<T>>(func);
            var expiration = DateTimeOffset.UtcNow.AddSeconds(CacheSecondsDuration);
            var cacheItemPolicy = CacheItemPolicyFactory.GetFlushCachePolicy(expiration);
            var oldValue = _cache.AddOrGetExisting(key, newValue, cacheItemPolicy) as Lazy<Task<T>>;
            try
            {
                return await (oldValue ?? newValue).Value;
            }
            catch
            {
                _cache.Remove(key);
                throw;
            }
        }

        public override string ToString()
        {
            return $"{_cache.Name} ({Count()} entries)";
        }

        public void Dispose()
        {
            _cache?.Dispose();
        }
    }
}