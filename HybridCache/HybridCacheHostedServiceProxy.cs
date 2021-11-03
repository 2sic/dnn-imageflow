using System.Threading;
using System.Threading.Tasks;
using Imazen.Common.Extensibility.StreamCache;
using Microsoft.Extensions.Hosting;

namespace ToSic.Imageflow.Dnn.HybridCache
{
    /// <summary>
    /// Imageflow.Server.HybridCache.HybridCacheHostedServiceProxy
    /// </summary>
    internal class HybridCacheHostedServiceProxy : IHostedService
    {

        private readonly IStreamCache cache;
        public HybridCacheHostedServiceProxy(IStreamCache cache)
        {
            this.cache = cache;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return cache.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return cache.StopAsync(cancellationToken);
        }

    }
}
