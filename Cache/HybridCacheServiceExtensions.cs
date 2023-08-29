using Imazen.Common.Extensibility.StreamCache;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ToSic.Imageflow.Dnn.Cache
{
    /// <summary>
    /// Extension method for adding hybrid cache service to the IServiceCollection.
    /// </summary>
    internal static class HybridCacheServiceExtensions
    {
        /// <summary>
        /// Adds image flow hybrid cache service to the IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <param name="options">The hybrid cache options.</param>
        /// <returns>The updated IServiceCollection instance.</returns>
        public static IServiceCollection AddImageflowHybridCache(this IServiceCollection services, HybridCacheOptions options)
        {
            services.AddSingleton<IStreamCache>(container =>
            {
                var logger = container.GetService<ILogger<HybridCacheService>>();
                return new HybridCacheService(options, logger);
            });

            return services;
        }
    }
}
