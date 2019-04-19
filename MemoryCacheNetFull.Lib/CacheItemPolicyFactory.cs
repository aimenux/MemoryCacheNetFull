using System;
using System.Runtime.Caching;

namespace MemoryCacheNetFull.Lib
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