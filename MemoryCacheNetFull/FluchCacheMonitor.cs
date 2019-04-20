using System;
using System.Diagnostics;
using System.Runtime.Caching;

namespace MemoryCacheNetFull
{
    public class FluchCacheMonitor : ChangeMonitor
    {
        private static event EventHandler<FlushChangeEventArgs> Flushed;

        public override string UniqueId { get; } = Guid.NewGuid().ToString();

        public FluchCacheMonitor()
        {
            Flushed += OnFlushRaised;
            base.InitializationComplete();
        }

        public static void Fluch()
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