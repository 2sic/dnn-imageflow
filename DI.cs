using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Web;
using ToSic.Imageflow.Dnn.HybridCache;

namespace ToSic.Imageflow.Dnn
{
    public class DI
    {
        private static bool _alreadyConfigured;
        private static IServiceCollection _serviceCollection = new ServiceCollection();
        /// <summary>
        /// Dictionary key for keeping the Injection Service Provider in the Http-Context
        /// </summary>
        private const string ServiceProviderKey = "imageflow-scoped-serviceprovider";

        private static IServiceProvider _serviceProvider;
        public static IServiceProvider GetServiceProvider()
        {
            // Because Imageflow runs inside DNN as a webforms project and not asp.net core mvc, we have
            // to make sure the service-provider object is disposed correctly. If we don't do this,
            // connections to the database are kept open, and this leads to errors like "SQL timeout:
            // "All pooled connections were in use". https://github.com/2sic/2sxc/issues/1200
            // Work-around for issue https://github.com/2sic/2sxc/issues/1200
            // Scope service-provider based on request
            var httpCtx = HttpContext.Current;
            if (httpCtx == null) return _serviceProvider.CreateScope().ServiceProvider;

            if (httpCtx.Items[ServiceProviderKey] == null)
            {
                httpCtx.Items[ServiceProviderKey] = _serviceProvider.CreateScope().ServiceProvider;

                // Make sure service provider is disposed after request finishes
                httpCtx.AddOnRequestCompleted(context =>
                {
                    ((IDisposable)context.Items[ServiceProviderKey])?.Dispose();
                });
            }

            return (IServiceProvider)httpCtx.Items[ServiceProviderKey];
        }

        /// <summary>
        /// Dependency Injection resolver with a known type as a parameter.
        /// </summary>
        /// <typeparam name="T">The type / interface we need.</typeparam>
        public static T Resolve<T>()
        {
            var found = GetServiceProvider().GetService<T>();

            return found != null
                ? found
                // If it's an unregistered type, try to find in DLLs etc.
                : ActivatorUtilities.CreateInstance<T>(_serviceProvider);
        }

        /// <summary>
        /// Configure IoC. If it's already configured, do nothing.
        /// </summary>
        public static void Configure()
        {
            if (_alreadyConfigured)
                return;

            //setup our DI
            ConfigureServices(_serviceCollection);
            _serviceProvider = _serviceCollection.BuildServiceProvider();

            _alreadyConfigured = true;
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var homeFolder = HttpContext.Current.Server.MapPath("~");

            // You can add a hybrid cache (in-memory persisted database for tracking filenames, but files used for bytes)
            // But remember to call ImageflowOptions.SetAllowCaching(true) for it to take effect
            // If you're deploying to azure, provide a disk cache folder *not* inside ContentRootPath
            // to prevent the app from recycling whenever folders are created.
            services.AddImageflowHybridCache(
                new HybridCacheOptions(Path.Combine(homeFolder, "App_Data", "imageflow_hybrid_cache"))
                {
                    // How long after a file is created before it can be deleted
                    MinAgeToDelete = TimeSpan.FromSeconds(10),
                    // How much RAM to use for the write queue before switching to synchronous writes
                    QueueSizeLimitInBytes = 100 * 1000 * 1000,
                    // The maximum size of the cache (1GB)
                    CacheSizeLimitInBytes = 1024 * 1024 * 1024,
                });
        }
    }
}
