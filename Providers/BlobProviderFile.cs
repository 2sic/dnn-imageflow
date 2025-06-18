using System;
using System.IO;
using Imazen.Common.Storage;

namespace ToSic.Imageflow.Dnn.Providers
{
    internal class BlobProviderFile : IBlobData
    {
        public string Path;
        public bool? Exists { get; set; }
        public DateTime? LastModifiedDateUtc { get; set; }

        public Stream OpenRead()
        {
            // Read the file into memory and return a MemoryStream to avoid file locks
            var bytes = File.ReadAllBytes(Path);
            return new MemoryStream(bytes, writable: false);
        }

        public void Dispose() { }
    }
}
