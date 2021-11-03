using Imazen.Common.Extensibility.ClassicDiskCache;
using Imazen.Common.Extensibility.StreamCache;
using Imazen.Common.Instrumentation.Support.InfoAccumulators;
using Imazen.Common.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ToSic.Imageflow.Dnn
{
    internal class GlobalInfoProvider : IInfoProvider
    {
        private readonly IStreamCache streamCache;
        private readonly ImageflowOptions options;
        private readonly List<string> pluginNames;
        private readonly List<IInfoProvider> infoProviders;
        public GlobalInfoProvider(ImageflowOptions options,
            IStreamCache streamCache,
            IClassicDiskCache diskCache, 
            IList<IBlobProvider> blobProviders)
        {
            this.streamCache = streamCache;
            this.options = options;
            var plugins = new List<object>() { null, streamCache, diskCache }.Concat(blobProviders).ToList();
            infoProviders = plugins.OfType<IInfoProvider>().ToList();

            pluginNames = plugins
                .Where(p => p != null)
                .Select(p =>
                {
                    var t = p.GetType();
                    if (t.Namespace != null &&
                        (t.Namespace.StartsWith("Imazen") ||
                        t.Namespace.StartsWith("Imageflow") ||
                        t.Namespace.StartsWith("Microsoft.Extensions.Logging") ||
                        t.Namespace.StartsWith("Microsoft.Extensions.Caching")))
                    {
                        return t.Name;
                    }
                    else
                    {
                        return t.FullName;
                    }
                }).ToList();
        }

        public void Add(IInfoAccumulator query)
        {
            var q = query.WithPrefix("proc_");
            if (HttpContext.Current.Request.ServerVariables["SERVER_SOFTWARE"] != null)
                q.Add("iis", HttpContext.Current.Request.ServerVariables["SERVER_SOFTWARE"]);

            q.Add("default_commands", PathHelpers.SerializeCommandString(options.CommandDefaults));
            q.Add("info_version", Assembly.GetAssembly(this.GetType()).GetInformationalVersion());
            q.Add("file_version", Assembly.GetAssembly(this.GetType()).GetFileVersion());
            query.Add("imageflow", 1);
            query.AddString("enabled_cache", options.ActiveCacheBackend.ToString());
            query.AddString("stream_cache", streamCache.GetType().Name);
            query.Add("map_web_root", options.MapWebRoot);
            query.Add("request_signing_default", "never");
            query.Add("default_cache_control", options.DefaultCacheControlString);

            foreach (var s in pluginNames)
            {
                query.Add("p", s);
            }
            foreach (var p in infoProviders)
            {
                p?.Add(query);
            }
        }
    }
}
