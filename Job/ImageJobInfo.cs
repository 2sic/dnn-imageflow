using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Imageflow.Fluent;
using Imazen.Common.Storage;
using ToSic.Imageflow.Dnn.Helpers;
using ToSic.Imageflow.Dnn.Options;
using ToSic.Imageflow.Dnn.Providers;

namespace ToSic.Imageflow.Dnn.Job
{
    public class ImageJobInfo
    {
        public ImageJobInfo(HttpContext context, ImageflowModuleOptions moduleOptions, BlobProvider blobProvider)
        {
            this._moduleOptions = moduleOptions;
            Authorized = ProcessRewrites(context, moduleOptions);

            HasParams = PathHelpers.SupportedQuerystringKeys.Any(FinalQuery.ContainsKey);

            var extension = Path.GetExtension(FinalVirtualPath);
            if (FinalQuery.TryGetValue("format", out var newExtension)) extension = newExtension;

            EstimatedFileExtension = PathHelpers.SanitizeImageExtension(extension) ?? "jpg";

            primaryBlob = new BlobFetchCache(FinalVirtualPath, blobProvider);
            allBlobs = new List<BlobFetchCache>(1) { primaryBlob };

            if (HasParams) CommandString = PathHelpers.SerializeCommandString(FinalQuery);

            // TODO: watermark, presets
        }

        private readonly ImageflowModuleOptions _moduleOptions;
        public string FinalVirtualPath { get; private set; }
        private Dictionary<string, string> FinalQuery { get; set; }
        public bool HasParams { get; }
        public bool Authorized { get; }
        public string EstimatedFileExtension { get; }
        private readonly List<BlobFetchCache> allBlobs;
        private readonly BlobFetchCache primaryBlob;
        public string CommandString { get; } = "";

        private bool ProcessRewrites(HttpContext context, ImageflowModuleOptions moduleOptions)
        {
            var path = context.Request.Path;
            var args = new UrlEventArgs(context, context.Request.Path, PathHelpers.ToQueryDictionary(context.Request.QueryString));

            // Apply rewrite handlers
            foreach (var handler in moduleOptions.Rewrite)
            {
                var matches = string.IsNullOrEmpty(handler.PathPrefix) ||
                              path.StartsWith(handler.PathPrefix, StringComparison.OrdinalIgnoreCase);
                if (matches)
                {
                    handler.Handler(args);
                    path = args.VirtualPath;
                }
            }

            // Set defaults if keys are missing, but at least 1 supported key is present
            if (PathHelpers.SupportedQuerystringKeys.Any(args.Query.ContainsKey))
            {
                foreach (var pair in moduleOptions.CommandDefaults)
                {
                    if (!args.Query.ContainsKey(pair.Key))
                    {
                        args.Query[pair.Key] = pair.Value;
                    }
                }
            }

            FinalVirtualPath = args.VirtualPath;
            FinalQuery = args.Query;
            return true;
        }

        public bool PrimaryBlobMayExist()
        {
            // Just returns a lambda for performing the actual fetch, does not actually call .Fetch() on providers
            return primaryBlob.GetBlobResult() != null;
        }

        public bool NeedsCaching()
        {
            return HasParams || primaryBlob?.GetBlobResult()?.IsFile == false;
        }

        public Task<IBlobData> GetPrimaryBlob()
        {
            return primaryBlob.GetBlob();
        }

        private string HashStrings(IEnumerable<string> strings)
        {
            return PathHelpers.Base64Hash(string.Join("|", strings));
        }

        internal async Task CopyPrimaryBlobToAsync(Stream stream)
        {
            using (var sourceStream = (await GetPrimaryBlob()).OpenRead())
            {
                var oldPosition = stream.Position;
                await sourceStream.CopyToAsync(stream);
                if (stream.Position - oldPosition == 0)
                {
                    throw new InvalidOperationException("Source blob has zero bytes; will not proxy.");
                }
            }
        }

        internal async Task<byte[]> GetPrimaryBlobBytesAsync()
        {
            using (var sourceStream = (await GetPrimaryBlob()).OpenRead())
            {
                var ms = new MemoryStream(sourceStream.CanSeek ? (int)sourceStream.Length : 4096);
                await sourceStream.CopyToAsync(ms);
                var buffer = ms.ToArray();
                if (buffer.Length == 0)
                {
                    throw new InvalidOperationException("Source blob has length of zero bytes; will not proxy.");
                }
                return buffer;
            }
        }

        public async Task<string> GetFastCacheKey()
        {
            // Only get DateTime values from local files
            var dateTimes = await Task.WhenAll(
                allBlobs
                    .Where(b => b.GetBlobResult()?.IsFile == true)
                    .Select(async b =>
                        (await b.GetBlob())?.LastModifiedDateUtc?.ToBinary().ToString()));

            return HashStrings(new[] { FinalVirtualPath, CommandString }.Concat(dateTimes));
        }

        public override string ToString()
        {
            return CommandString;
        }

        public async Task<string> GetExactCacheKey()
        {
            var dateTimes = await Task.WhenAll(
                allBlobs
                    .Select(async b =>
                        (await b.GetBlob())?.LastModifiedDateUtc?.ToBinary().ToString()));

            return HashStrings(new[] { FinalVirtualPath, CommandString }.Concat(dateTimes));
        }

        public async Task<ImageData> ProcessUncached()
        {
            //Fetch all blobs simultaneously
            var blobs = await Task.WhenAll(
                allBlobs
                    .Select(BlobFetchResult.FromCache));
            try
            {
                using (var buildJob = new ImageJob())
                {
                    var jobResult = buildJob.BuildCommandString(
                            blobs[0].GetBytesSource(),
                            new BytesDestination(), 
                            CommandString)
                        .Finish()
                        .InProcessAsync().Result.First; ;

                    // TryGetBytes returns the buffer from a regular MemoryStream, not a recycled one
                    var resultBytes = jobResult.TryGetBytes();

                    if (!resultBytes.HasValue || resultBytes.Value.Count < 1 || resultBytes.Value.Array == null)
                    {
                        throw new InvalidOperationException("Image job returned zero bytes.");
                    }

                    return new ImageData
                    {
                        ContentType = jobResult.PreferredMimeType,
                        ResultBytes = resultBytes.Value
                    };
                }
            }
            finally
            {
                foreach (var b in blobs)
                {
                    b?.Dispose();
                }
            }
        }
    }
}
