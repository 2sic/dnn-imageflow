using Imazen.Common.Extensibility.StreamCache;
using Imazen.Common.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using ToSic.Imageflow.Dnn.Helpers;
using ToSic.Imageflow.Dnn.Job;
using ToSic.Imageflow.Dnn.Options;
using ToSic.Imageflow.Dnn.Providers;

namespace ToSic.Imageflow.Dnn
{
    /// <summary>
    /// ImageflowModule
    /// </summary>
    public class ImageflowModule : IHttpModule
    {
        public static IServiceProvider ServiceProvider;
        //private IClassicDiskCache diskCache;
        private IStreamCache _streamCache;
        private IEnumerable<IBlobProvider> _blobProviders;
        private BlobProvider _blobProvider;
        private readonly ImageflowModuleOptions _options = new ImageflowModuleOptions();

        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
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

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">
        /// true if the object is currently disposing.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            // left empty by design
        }

        private async Task ImageflowRequestAsync(object source, EventArgs e)
        {
            var context = ((HttpApplication)source).Context;

            var request = context.Request;

            if (!RequestIsValid(request)) return;



            // TODO: DNN Security ...
            // TODO: DNN File providers..

            // Caching
            PrepareHybridCacheDependencies();

            var imageJobInfo = new ImageJobInfo(context, _options, _blobProvider);

            // Skip if the file is definitely missing.
            // Remote providers will fail late rather than make 2 requests
            if (!imageJobInfo.PrimaryBlobMayExist()) return;

            // Skip when caching is not needed.
            if (!imageJobInfo.NeedsCaching()) return;

            var taskCacheKey = imageJobInfo.GetFastCacheKey();
            var cacheKey = taskCacheKey.GetAwaiter().GetResult();
            var etag = request.Headers["If-None-Match"];

            if (!string.IsNullOrEmpty(etag) && cacheKey == etag)
            {
                NotModified(context);
                return;
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

        private void PrepareHybridCacheDependencies()
        {
            _streamCache = ServiceProvider.GetService<IStreamCache>();

            _blobProviders = ServiceProvider.GetService<IEnumerable<IBlobProvider>>();

            var mappedPaths = new List<PathMapping>
            {
                new PathMapping("/", HttpContext.Current.Server.MapPath("~"))
            };

            _blobProvider = new BlobProvider(_blobProviders, mappedPaths);
        }

        private async Task ProcessWithStreamCache(HttpContext context, string cacheKey, ImageJobInfo info)
        {
            var key = Encoding.UTF8.GetBytes(cacheKey);
            var typeName = _streamCache.GetType().Name;
            var cacheResult = await _streamCache.GetOrCreateBytes(key, async (cancellationToken) =>
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
                    context.Response.End();
                }
            }
            else
            {
                // TODO explore this failure path better
                throw new NullReferenceException("Caching failed: " + cacheResult.Status);
            }
        }

        private static void NotModified(HttpContext context)
        {
            context.Response.StatusCode = 304;
            context.Response.ContentType = null;
            context.Response.End();
        }

        private static void NotFound(HttpContext context, BlobMissingException e)
        {
            var message = "The specified resource does not exist.\r\n" + e.Message;
            context.Response.StatusCode = 404;
            context.Response.ContentType = "text/plain; charset=utf-8";
            context.Response.Write(message);
            context.Response.End();
        }

        private void SetCachingHeaders(HttpContext context, string etag)
        {
            context.Response.Headers["ETag"] = etag;
            if (_options.DefaultCacheControlString != null)
                context.Response.Headers["Cache-Control"] = _options.DefaultCacheControlString;
        }


    }
}
