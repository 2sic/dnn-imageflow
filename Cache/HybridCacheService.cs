using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Imazen.Common.Extensibility.StreamCache;
using Imazen.Common.Issues;
using Imazen.HybridCache;
using Imazen.HybridCache.MetaStore;
using Microsoft.Extensions.Logging;

namespace ToSic.Imageflow.Dnn.Cache
{
    internal class HybridCacheService : IStreamCache
    {
        private readonly HybridCache _cache;

        public HybridCacheService(HybridCacheOptions options, ILogger<HybridCacheService> logger)
        {
            var cacheOptions = new Imazen.HybridCache.HybridCacheOptions(options.DiskCacheDirectory)
            {
                AsyncCacheOptions = new AsyncCacheOptions()
                {
                    MaxQueuedBytes = Math.Max(0, options.QueueSizeLimitInBytes),
                    WriteSynchronouslyWhenQueueFull = true,
                    MoveFileOverwriteFunc = (from, to) =>
                    {
                        if (File.Exists(to)) File.Delete(to);
                        File.Move(@from, to);
                    }
                },
                CleanupManagerOptions = new CleanupManagerOptions()
                {
                    MaxCacheBytes = Math.Max(0, options.CacheSizeLimitInBytes),
                    MinCleanupBytes = Math.Max(0, options.MinCleanupBytes),
                    MinAgeToDelete = options.MinAgeToDelete.Ticks > 0 ? options.MinAgeToDelete : TimeSpan.Zero,
                }
            };

            var database = new MetaStore(new MetaStoreOptions(options.DiskCacheDirectory)
            {
                Shards = Math.Max(1, options.DatabaseShards),
                MaxLogFilesPerShard = 3,
            }, cacheOptions, logger);

            _cache = new HybridCache(database, cacheOptions, logger);
        }

        public IEnumerable<IIssue> GetIssues() => _cache.GetIssues();

        public Task StartAsync(CancellationToken cancellationToken) => _cache.StartAsync(cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken) => _cache.StopAsync(cancellationToken);

        public Task<IStreamCacheResult> GetOrCreateBytes(byte[] key, AsyncBytesResult dataProviderCallback, CancellationToken cancellationToken,
            bool retrieveContentType) =>
            _cache.GetOrCreateBytes(key, dataProviderCallback, cancellationToken, retrieveContentType);
    }
}
