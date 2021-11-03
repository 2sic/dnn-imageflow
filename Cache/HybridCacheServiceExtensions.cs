﻿using Imazen.Common.Extensibility.StreamCache;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ToSic.Imageflow.Dnn.Cache
{
    internal static class HybridCacheServiceExtensions
    {
        public static IServiceCollection AddImageflowHybridCache(this IServiceCollection services, HybridCacheOptions options)
        {
            services.AddLogging();

            services.AddSingleton<IStreamCache>(container =>
            {
                var logger = container.GetRequiredService<ILogger<HybridCacheService>>();
                return new HybridCacheService(options, logger);
            });

            return services;
        }
    }
}
