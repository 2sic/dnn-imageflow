using DotNetNuke.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Web;
using DotNetNuke.Services.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ToSic.Imageflow.Dnn.Cache;

namespace ToSic.Imageflow.Dnn
{
    public class Startup : IDnnStartup
    {
        private static bool _alreadyConfigured;
        private static readonly object ConfigureLock = new object();

        /// <summary>
        /// Configure IoC. If it's already configured, do nothing.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            if (_alreadyConfigured) return;

            // ensure that native assemblies are in place, before we start to use them
            UpgradeUtil.UpgradeNativeAssemblies();

            lock (ConfigureLock)
            {
                // after waiting..
                if (_alreadyConfigured) return;

                var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IScopeAccessor));
                var found = serviceDescriptor != null;
                if (found)
                {
                    services.Remove(serviceDescriptor);
                    services.AddSingleton<IScopeAccessor, ScopeAccessor>(container =>
                    {
                        ImageflowModule.SetServiceProvider(container);
                        return new ScopeAccessor();
                    });
                }

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

                _alreadyConfigured = true;
            }
        }
    }
}
