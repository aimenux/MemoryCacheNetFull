using System;
using System.Diagnostics;
using System.Runtime.Caching;

namespace MemoryCacheNetFull
{
    public class FlushCacheMonitor : ChangeMonitor
    {
        private static event EventHandler<FlushChangeEventArgs> Flushed;

        public override string UniqueId { get; } = Guid.NewGuid().ToString();

        public FlushCacheMonitor()
        {
            Flushed += OnFlushRaised;
            base.InitializationComplete();
        }

        public static void Flush()
        {
            Flushed?.Invoke(null, new FlushChangeEventArgs());
        }

        protected override void Dispose(bool disposing)
        {
            Flushed -= OnFlushRaised;
        }

        private void OnFlushRaised(object sender, FlushChangeEventArgs e)
        {
            Debug.WriteLine("Flushing cache");
            base.OnChanged(null);
        }
    }
}