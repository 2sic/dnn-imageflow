using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Web;
using ToSic.Imageflow.Dnn.Cache;

namespace ToSic.Imageflow.Dnn
{
    internal class DependencyInjection
    {
        private static bool _alreadyConfigured;
        private static readonly IServiceCollection ServiceCollection = new ServiceCollection();
        private static IServiceProvider _serviceProvider;

        private static readonly object ConfigureLock = new object();

        public static IServiceProvider GetServiceProvider()
        {
            Configure();
            return _serviceProvider;
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
            if (_alreadyConfigured) return;

            // ensure that native assemblies are in place, before we start to use them
            UpgradeUtil.UpgradeNativeAssemblies();

            lock (ConfigureLock)
            {
                // after waiting..
                if (_alreadyConfigured) return;

                //setup our DependencyInjection
                ConfigureServices(ServiceCollection);
                _serviceProvider = ServiceCollection.BuildServiceProvider();

                _alreadyConfigured = true;
            }
        }

        /// <summary>
        /// ConfigureServices for DI
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServices(IServiceCollection services)
        {
            var homeFolder = HttpContext.Current.Server.MapPath("~");

            // You can add a hybrid cache (in-memory persisted database for tracking filenames, but files used for bytes)
            // Provide a disk cache folder inside App_Data to prevent the app from recycling whenever folders are created.
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
