using System;
using System.Collections.Generic;

namespace ToSic.Imageflow.Dnn
{
    public class ImageflowOptions
    {
        internal CacheBackend ActiveCacheBackend { get; set; } = CacheBackend.StreamCache;

        private readonly List<PathMapping> mappedPaths = new List<PathMapping>();

        public IReadOnlyCollection<PathMapping> MappedPaths => mappedPaths;

        public bool MapWebRoot { get; set; } = true;

        public bool UsePresetsExclusively { get; set; }

        public string DefaultCacheControlString { get; set; }

        internal readonly List<UrlHandler<Action<UrlEventArgs>>> Rewrite = new List<UrlHandler<Action<UrlEventArgs>>>();

        internal readonly Dictionary<string, string> CommandDefaults = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        internal readonly Dictionary<string, PresetOptions> Presets = new Dictionary<string, PresetOptions>(StringComparer.OrdinalIgnoreCase);

        public ImageflowOptions MapPath(string virtualPath, string physicalPath)
            => MapPath(virtualPath, physicalPath, false);

        public ImageflowOptions MapPath(string virtualPath, string physicalPath, bool ignorePrefixCase)
        {
            mappedPaths.Add(new PathMapping(virtualPath, physicalPath, ignorePrefixCase));
            return this;
        }

        /// <summary>
        /// Use "public, max-age=2592000" to cache for 30 days and cache on CDNs and proxies.
        /// </summary>
        /// <param name="cacheControlString"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ImageflowOptions SetDefaultCacheControlString(string cacheControlString)
        {
            DefaultCacheControlString = cacheControlString;
            return this;
        }
    }
}
