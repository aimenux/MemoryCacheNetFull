using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace MemoryCacheNetFull
{
    public sealed class InMemoryCache : IInMemoryCache, IDisposable
    {
        private readonly MemoryCache _cache;
        private readonly int _cacheDurationInSeconds;

        public InMemoryCache(string name, int cacheDurationInSeconds)
        {
            _cache = new MemoryCache(name);
            _cacheDurationInSeconds = cacheDurationInSeconds;
        }

        public long Size => _cache.GetCount();

        public async Task<T> AddOrGetAsync<T>(string key, Func<Task<T>> func)
        {
            var newValue = new Lazy<Task<T>>(func);
            var expiration = DateTimeOffset.UtcNow.AddSeconds(_cacheDurationInSeconds);
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

        public Task ClearCacheEntriesAsync()
        {
            FlushCacheMonitor.Flush();
            Console.WriteLine("Flushing cache");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _cache?.Dispose();
        }
    }
}