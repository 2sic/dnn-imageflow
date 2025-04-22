using Imazen.Common.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ToSic.Imageflow.Dnn.Providers
{
    internal class BlobProvider
    {
        private readonly List<IBlobProvider> _blobProviders = new List<IBlobProvider>();
        private readonly List<string> _blobPrefixes = new List<string>();
        public BlobProvider(IEnumerable<IBlobProvider> blobProviders)
        {
            foreach (var provider in blobProviders)
            {
                _blobProviders.Add(provider);
                foreach (var prefix in provider.GetPrefixes())
                {
                    var conflictingPrefix =
                        _blobPrefixes.FirstOrDefault(p =>
                            prefix.StartsWith(p, StringComparison.OrdinalIgnoreCase) ||
                            p.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
                    if (conflictingPrefix != null)
                    {
                        throw new InvalidOperationException($"Blob Provider failure: Prefix {{prefix}} conflicts with prefix {conflictingPrefix}");
                    }
                    // We don't check for conflicts with PathMappings because / is a path mapping usually, 
                    // and we simply prefer blobs over files if there are overlapping prefixes.
                    _blobPrefixes.Add(prefix);
                }
            }
        }

        public BlobProviderResult? GetResult(string virtualPath)
        {
            return GetBlobResult(virtualPath) ?? GetFileResult(virtualPath);
        }

        private BlobProviderResult? GetBlobResult(string virtualPath)
        {
            if (_blobPrefixes.Any(p => virtualPath.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            {
                foreach (var provider in _blobProviders)
                {
                    if (provider.SupportsPath(virtualPath))
                    {
                        return new BlobProviderResult()
                        {
                            IsFile = false,
                            GetBlob = () => provider.Fetch(virtualPath)
                        };
                    }
                }
            }
            return null;
        }

        private BlobProviderResult? GetFileResult(string virtualPath)
        {
            var physicalPath = SafeMapPath(virtualPath);
            if (physicalPath == null)
                return null; // We stopped a directory traversal attack (most likely)

            var lastWriteTimeUtc = File.GetLastWriteTimeUtc(physicalPath);
            if (lastWriteTimeUtc.Year == 1601) // file doesn't exist
                return null;

            return new BlobProviderResult()
            {
                IsFile = true,
                GetBlob = () => Task.FromResult(new BlobProviderFile()
                {
                    Path = physicalPath,
                    Exists = true,
                    LastModifiedDateUtc = lastWriteTimeUtc
                } as IBlobData)
            };
        }

        private static string SafeMapPath(string virtualPath)
        {
            if (string.IsNullOrEmpty(virtualPath))
                return null;

            // Sanitize the virtualPath to remove any ".." segments
            virtualPath = VirtualPathUtility.ToAppRelative(virtualPath);

            // Check if the sanitized path is within the application's virtual directory
            if (!virtualPath.StartsWith("~/"))
                return null;

            try
            {
                // Safely map the path
                return HttpContext.Current.Server.MapPath(virtualPath);
            }
            catch (HttpException)
            {
                return null;
            }
        }
    }
}
