namespace ToSic.Imageflow.Dnn.Helpers
{
    internal readonly struct PathMapping
    {
        public PathMapping(string virtualPath, string physicalPath)
        {
            VirtualPath = virtualPath;
            PhysicalPath = physicalPath.TrimEnd('/', '\\');
            IgnorePrefixCase = false;
        }

        public string VirtualPath { get; }
        public string PhysicalPath { get; }
        public bool IgnorePrefixCase { get; }
    }
}
