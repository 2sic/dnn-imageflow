﻿namespace ToSic.Imageflow.Dnn
{
    public readonly struct PathMapping
    {
        public PathMapping(string virtualPath, string physicalPath)
        {
            VirtualPath = virtualPath;
            PhysicalPath = physicalPath.TrimEnd('/', '\\');
            IgnorePrefixCase = false;
        }

        public PathMapping(string virtualPath, string physicalPath, bool ignorePrefixCase)
        {
            VirtualPath = virtualPath;
            PhysicalPath = physicalPath.TrimEnd('/', '\\');
            IgnorePrefixCase = ignorePrefixCase;
        }
        public string VirtualPath { get; }
        public string PhysicalPath { get; }
        public bool IgnorePrefixCase { get; }
    }
}
