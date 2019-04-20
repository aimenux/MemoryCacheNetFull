using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MemoryCacheNetFull.Lib.Tests
{
    [TestClass]
    public class InMemoryCacheTests
    {
        [TestMethod]
        public async Task Get_Old_Value_When_Delay_Is_Not_Expired()
        {
            var cache = new InMemoryCache($"{DateTime.UtcNow}");

            Assert.AreEqual(0, cache.Count());

            var xx = await cache.AddOrGetAsync<int>("foo",
                async () => await Task.FromResult(10));

            Assert.AreEqual(1, cache.Count());
            Assert.AreEqual(10, xx);

            await Task.Delay(1000);

            var yy = await cache.AddOrGetAsync<int>("foo",
                async () => await Task.FromResult(20));

            Assert.AreEqual(1, cache.Count());
            Assert.AreEqual(10, yy);

            Console.WriteLine(cache);
        }

        [TestMethod]
        public async Task Get_New_Value_When_Delay_Is_Expired()
        {
            var cache = new InMemoryCache($"{DateTime.UtcNow}");

            Assert.AreEqual(0, cache.Count());

            var xx = await cache.AddOrGetAsync<int>("foo",
                async () => await Task.FromResult(10));

            Assert.AreEqual(1, cache.Count());
            Assert.AreEqual(10, xx);

            await Task.Delay(6000);

            var yy = await cache.AddOrGetAsync<int>("foo",
                async () => await Task.FromResult(20));

            Assert.AreEqual(1, cache.Count());
            Assert.AreEqual(20, yy);

            Console.WriteLine(cache);
        }

        [TestMethod]
        public async Task Get_New_Value_When_Delay_Is_Not_Expired_And_Cache_Was_Flushed()
        {
            var cache = new InMemoryCache($"{DateTime.UtcNow}");

            Assert.AreEqual(0, cache.Count());

            var xx = await cache.AddOrGetAsync<int>("foo",
                async () => await Task.FromResult(10));

            Assert.AreEqual(1, cache.Count());
            Assert.AreEqual(10, xx);

            await Task.Delay(1000);
            cache.ClearCacheEntries();
            Assert.AreEqual(0, cache.Count());

            var yy = await cache.AddOrGetAsync<int>("foo",
                async () => await Task.FromResult(20));

            Assert.AreEqual(1, cache.Count());
            Assert.AreEqual(20, yy);

            Console.WriteLine(cache);
        }

        [TestMethod]
        public async Task Should_Not_Throw_Exception_When_Get_Or_Add_Or_Flush_Cache()
        {
            var cache = new InMemoryCache($"{DateTime.UtcNow}");

            var task1 = cache.AddOrGetAsync<int>("toto",
                async () => await Task.FromResult(10));

            var task2 = cache.AddOrGetAsync<int>("toto",
                async () => await Task.FromResult(10));

            var task3 = Task.Run(() =>
            {
                cache.ClearCacheEntries();
            });

            try
            {
                await Task.WhenAll(task1, task2, task3);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Console.WriteLine(cache);
        }
    }
}