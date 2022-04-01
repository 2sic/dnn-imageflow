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
using ToSic.Sxc.Services;

namespace ToSic.Imageflow.Dnn.Job
{
    internal class ImageJobInfo
    {
        public ImageJobInfo(HttpContext context, ImageflowModuleOptions options, BlobProvider provider)
        {
            Process(context, options);
            HasParams = PathHelpers.SupportedQuerystringKeys.Any(FinalQuery.ContainsKey);
            if (HasParams) CommandString = PathHelpers.SerializeCommandString(FinalQuery);

            _primaryBlob = new BlobFetchCache(FinalVirtualPath, provider);
            _allBlobs = new List<BlobFetchCache>(1) { _primaryBlob };

            // TODO: watermark, presets
        }


        private string FinalVirtualPath { get; set; }
        private Dictionary<string, string> FinalQuery { get; set; }
        public bool HasParams { get; }
        private string CommandString { get; } = "";
        private readonly List<BlobFetchCache> _allBlobs;
        private readonly BlobFetchCache _primaryBlob;

        private void Process(HttpContext context, ImageflowModuleOptions options)
        {
            var imageflowRewriteService = DependencyInjection.Resolve<IImageflowRewriteService>();
            var qs = imageflowRewriteService.QueryStringRewrite(context.Request.QueryString);
            var args = new UrlEventArgs(context, context.Request.Path, PathHelpers.ToQueryDictionary(qs));
            FinalVirtualPath = args.VirtualPath;
            FinalQuery = args.Query;
        }

        public bool PrimaryBlobMayExist()
        {
            // Just returns a lambda for performing the actual fetch, does not actually call .Fetch() on providers
            return _primaryBlob.GetBlobResult() != null;
        }

        public bool NeedsCaching()
        {
            return HasParams || _primaryBlob?.GetBlobResult()?.IsFile == false;
        }

        private Task<IBlobData> GetPrimaryBlob()
        {
            return _primaryBlob.GetBlob();
        }

        private string HashStrings(IEnumerable<string> strings)
        {
            return PathHelpers.Base64Hash(string.Join("|", strings));
        }

        public async Task<byte[]> GetPrimaryBlobBytesAsync()
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
                _allBlobs
                    .Where(b => b.GetBlobResult()?.IsFile == true)
                    .Select(async b =>
                        (await b.GetBlob())?.LastModifiedDateUtc?.ToBinary().ToString()));

            return HashStrings(new[] { FinalVirtualPath, CommandString }.Concat(dateTimes));
        }

        public override string ToString()
        {
            return CommandString;
        }

        public async Task<ImageData> ProcessUncached()
        {
            //Fetch all blobs simultaneously
            var blobs = await Task.WhenAll(
                _allBlobs
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
                        .InProcessAsync().Result.First;

                    // POC GRAVITY
                    //Constraint constraint =
                    //    new Constraint(ConstraintMode.Fit_Crop, 640, 480)
                    //        .SetGravity(new ConstraintGravity(25, 25));

                    ////constraint.Gravity = new ConstraintGravity(25, 25);

                    //var jobResult = buildJob.Decode(blobs[0].GetBytesSource()).Constrain(constraint)
                    //    .ResizerCommands(CommandString)
                    //    .EncodeToBytes(new GifEncoder())
                    //    .Finish()
                    //    .InProcessAsync().Result.First;

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
