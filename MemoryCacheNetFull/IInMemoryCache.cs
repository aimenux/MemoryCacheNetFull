using System;
using System.Threading.Tasks;

namespace MemoryCacheNetFull
{
    public interface IInMemoryCache
    {
        Task<T> AddOrGetAsync<T>(string key, Func<Task<T>> func);

        Task ClearCacheEntriesAsync();
    }
}