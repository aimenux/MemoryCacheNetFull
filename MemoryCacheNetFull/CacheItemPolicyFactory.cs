using System;
using System.Runtime.Caching;

namespace MemoryCacheNetFull
{
    public static class CacheItemPolicyFactory
    {
        public static CacheItemPolicy GetFlushCachePolicy(DateTimeOffset absoluteExpiration)
        {
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = absoluteExpiration,
            };
            policy.ChangeMonitors.Add(new FluchCacheMonitor());
            return policy;
        }
    }
}