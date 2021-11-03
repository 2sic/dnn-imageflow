using Imageflow.Fluent;
using Imazen.Common.Extensibility.StreamCache;
using Imazen.Common.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ToSic.Imageflow.Dnn
{
    /// <summary>
    /// ImageflowModule
    /// </summary>
    public class ImageflowModule : IHttpModule
    {
        //private IClassicDiskCache diskCache;
        private IStreamCache streamCache;
        private IEnumerable<IBlobProvider> blobProviders;
        private BlobProvider blobProvider;
        private GlobalInfoProvider globalInfoProvider;
        private ImageflowOptions options = new ImageflowOptions(); // TODO
        /// <summary>
        /// Dispose Imageflow HttpModule
        /// </summary>
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Init Imageflow HttpModule
        /// </summary>
        /// <param name="application"></param>
        public void Init(HttpApplication application)
        {
            var wrapper = new EventHandlerTaskAsyncHelper(ImageflowRequestAsync);
            application.AddOnBeginRequestAsync(wrapper.BeginEventHandler, wrapper.EndEventHandler);
        }

        private async Task ImageflowRequestAsync(object source, EventArgs e)
        {
            // TODO: Imageflow configuration

            var context = ((HttpApplication)source).Context;

            if (!RequestIsValid(context.Request)) return;

            // TODO: DNN Security ...

            // TODO: Caching
            streamCache = DI.Resolve<IStreamCache>();
            blobProviders = DI.Resolve<IEnumerable<IBlobProvider>>();
            var providers = blobProviders.ToList();
            var mappedPaths = options.MappedPaths.ToList();
            if (options.MapWebRoot) mappedPaths.Add(new PathMapping("/", HttpContext.Current.Server.MapPath("~")));

            blobProvider = new BlobProvider(providers, mappedPaths);
            globalInfoProvider = new GlobalInfoProvider(options, streamCache, null, providers);

            // TODO: DNN File providers..
            //SimpleInvoke(context);

            var imageJobInfo = new ImageJobInfo(context, options, blobProvider);

            // If the file is definitely missing hand to the next middleware
            // Remote providers will fail late rather than make 2 requests
            if (!imageJobInfo.PrimaryBlobMayExist()) return;

            string cacheKey = null;
            var cachingPath = imageJobInfo.NeedsCaching() ? options.ActiveCacheBackend : CacheBackend.NoCache;
            if (cachingPath != CacheBackend.NoCache)
            {
                var taskCacheKey = imageJobInfo.GetFastCacheKey();
                cacheKey = taskCacheKey.GetAwaiter().GetResult();

                var etag = context.Request.Headers["If-None-Match"];
                if (!string.IsNullOrEmpty(etag) && cacheKey == etag)
                {

                    context.Response.StatusCode = 304;
                    // context.Response.ContentLength = 0;
                    context.Response.ContentType = null;
                    context.Response.End();
                    return;
                }
            }

            try
            {
                await ProcessWithStreamCache(context, cacheKey, imageJobInfo);
            }
            catch (BlobMissingException ex)
            {
                NotFound(context, ex);
            }
        }

        private bool RequestIsValid(HttpRequest request)
        {
            var path = request.Path;
            var queryString = request.QueryString;

            // TODO: Extensionless images...

            // check for accepted image extension in image path.
            if (!PathHelpers.IsImagePath(path)) return false;

            // skip if query string is missing.
            if (!queryString.HasKeys()) return false;

            // check for supported query string keys
            if (!queryString.AllKeys.Any(key => PathHelpers.SupportedQuerystringKeys.Contains(key))) return false;

            // request is valid
            return true;
        }

        //private void SimpleInvoke(HttpContext context)
        //{
        //    // simple case when image is on file system
        //    var imageBytes = GetImageBytesFromFileSystem(context);

        //    // do work
        //    BuildImage(imageBytes, context);
        //}

        //private byte[] GetImageBytesFromFileSystem(HttpContext context)
        //{
        //    var fullPath = context.Request.MapPath(context.Request.Path);
        //    return File.Exists(fullPath) ? File.ReadAllBytes(fullPath) : null;
        //}

        //private void BuildImage(byte[] imageBytes, HttpContext context)
        //{
        //    if (imageBytes == null) return;

        //    using (var imageJob = new ImageJob())
        //    {
        //        var buildEncodeResult = imageJob.BuildCommandString(
        //                new BytesSource(imageBytes),
        //                new BytesDestination(),
        //                context.Request.QueryString.ToString())
        //            .Finish().InProcessAsync().Result.First;

        //        var processedImage = buildEncodeResult.TryGetBytes();

        //        if (processedImage?.Array == null) return;

        //        // return imageflow processed image 
        //        context.Response.ContentType = buildEncodeResult.PreferredMimeType;
        //        context.Response.BinaryWrite(processedImage.Value.Array);

        //        context.Response.End(); // terminate request
        //    }
        //}

        private async Task ProcessWithStreamCache(HttpContext context, string cacheKey, ImageJobInfo info)
        {
            var keyBytes = Encoding.UTF8.GetBytes(cacheKey);
            var typeName = streamCache.GetType().Name;
            var cacheResult = await streamCache.GetOrCreateBytes(keyBytes, async (cancellationToken) =>
            {
                if (info.HasParams)
                {
                    var result = await info.ProcessUncached();
                    if (result.ResultBytes.Array == null)
                    {
                        throw new InvalidOperationException("Image job returned zero bytes.");
                    }
                    return new Tuple<string, ArraySegment<byte>>(result.ContentType, result.ResultBytes);
                }

                var bytes = await info.GetPrimaryBlobBytesAsync();
                return new Tuple<string, ArraySegment<byte>>(null, new ArraySegment<byte>(bytes));

            }, CancellationToken.None, false);

            if (cacheResult.Data != null)
            {
                using (cacheResult.Data)
                {
                    if (cacheResult.Data.Length < 1)
                    {
                        throw new InvalidOperationException($"{typeName} returned cache entry with zero bytes");
                    }
                    SetCachingHeaders(context, cacheKey);
                    await MagicBytes.ProxyToStream(cacheResult.Data, context.Response);
                    context.Response.End(); // terminate request
                }
            }
            else
            {
                // TODO explore this failure path better
                throw new NullReferenceException("Caching failed: " + cacheResult.Status);
            }
        }

        private void NotFound(HttpContext context, BlobMissingException e)
        {
            // We allow 404s to be cached, but not 403s or license errors
            var s = "The specified resource does not exist.\r\n" + e.Message;
            context.Response.StatusCode = 404;
            context.Response.ContentType = "text/plain; charset=utf-8";
            var bytes = Encoding.UTF8.GetBytes(s);
            context.Response.BinaryWrite(bytes);
            context.Response.End(); // terminate request
        }

        private void SetCachingHeaders(HttpContext context, string etag)
        {
            context.Response.Headers["ETag"] = etag;
            if (options.DefaultCacheControlString != null)
                context.Response.Headers["Cache-Control"] = options.DefaultCacheControlString;
        }
    }
}
