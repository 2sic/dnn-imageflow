namespace ToSic.Imageflow.Dnn.Options
{
    /// <summary>
    /// ImageflowModuleOptions
    /// </summary>
    internal class ImageflowModuleOptions
    {
        /// <summary>
        /// Use "public, max-age=2592000" to cache for 30 days and cache on CDNs and proxies.
        /// </summary>
        public string DefaultCacheControlString { get; set; }
    }
}
