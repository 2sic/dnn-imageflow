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
        // The HybridCache object that manages data caching.
        private readonly HybridCache _cache;

        /// <summary>
        /// Constructor for the Hybrid Cache service.
        /// </summary>
        /// <param name="options">Options for the Hybrid Cache service.</param>
        /// <param name="logger">Logger instance for logging events.</param>
        public HybridCacheService(HybridCacheOptions options, ILogger<HybridCacheService> logger)
        {
            // Configure options for the Hybrid Cache service.
            var cacheOptions = new Imazen.HybridCache.HybridCacheOptions(options.DiskCacheDirectory)
            {
                // Configure Async Cache Options.
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
                // Configure Cleanup Manager Options.
                CleanupManagerOptions = new CleanupManagerOptions()
                {
                    MaxCacheBytes = Math.Max(0, options.CacheSizeLimitInBytes),
                    MinCleanupBytes = Math.Max(0, options.MinCleanupBytes),
                    MinAgeToDelete = options.MinAgeToDelete.Ticks > 0 ? options.MinAgeToDelete : TimeSpan.Zero,
                }
            };

            // Create a new MetaStore database.
            var database = new MetaStore(new MetaStoreOptions(options.DiskCacheDirectory)
            {
                Shards = Math.Max(1, options.DatabaseShards),
                MaxLogFilesPerShard = 3,
            }, cacheOptions, logger);

            // Initialize the Hybrid Cache with the MetaStore database and other options.
            _cache = new HybridCache(database, cacheOptions, logger);
        }

        /// <summary>
        /// Get a list of issues within the Hybrid Cache.
        /// </summary>
        /// <returns>A list of issues within the cache.</returns>
        public IEnumerable<IIssue> GetIssues() => _cache.GetIssues();

        /// <summary>
        /// Start the Hybrid Cache.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A task representing the starting of the cache.</returns>
        public Task StartAsync(CancellationToken cancellationToken) => _cache.StartAsync(cancellationToken);

        /// <summary>
        /// Stop the Hybrid Cache.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A task representing the stopping of the cache.</returns>
        public Task StopAsync(CancellationToken cancellationToken) => _cache.StopAsync(cancellationToken);

        /// <summary>
        /// Retrieve or create a new set of bytes within the cache.
        /// </summary>
        /// <param name="key">The key to identify the cache entry.</param>
        /// <param name="dataProviderCallback">The function that retrieves missing data.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <param name="retrieveContentType">Whether or not to retrieve the content type of the data.</param>
        /// <returns>The result of the cache retrieval or creation.</returns>
        public Task<IStreamCacheResult> GetOrCreateBytes(byte[] key, AsyncBytesResult dataProviderCallback, CancellationToken cancellationToken,
            bool retrieveContentType) =>
            _cache.GetOrCreateBytes(key, dataProviderCallback, cancellationToken, retrieveContentType);
    }
}
