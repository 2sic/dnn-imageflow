using Imageflow.Fluent;
using System;
using System.IO;
using System.Linq;
using System.Web;

namespace ToSic.Imageflow.Dnn
{
    /// <summary>
    /// ImageflowModule
    /// </summary>
    public class ImageflowModule : IHttpModule
    {
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
            application.BeginRequest += (new EventHandler(this.Application_BeginRequest));
        }

        private void Application_BeginRequest(object source, EventArgs e)
        {
            // TODO: Imageflow configuration

            var context = ((HttpApplication)source).Context;

            if (!RequestIsValid(context.Request)) return;

            // TODO: DNN Security ...

            // TODO: Caching

            // TODO: DNN File providers..

            // simple case when image is on file system
            var imageBytes = GetImageBytesFromFileSystem(context);

            // do work
            BuildImage(imageBytes, context);
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

        private byte[] GetImageBytesFromFileSystem(HttpContext context)
        {
            var fullPath = context.Request.MapPath(context.Request.Path);
            return File.Exists(fullPath) ? File.ReadAllBytes(fullPath) : null;
        }

        private void BuildImage(byte[] imageBytes, HttpContext context)
        {
            if (imageBytes == null) return;

            using (var imageJob = new ImageJob())
            {
                var buildEncodeResult = imageJob.BuildCommandString(
                        new BytesSource(imageBytes),
                        new BytesDestination(),
                        context.Request.QueryString.ToString())
                    .Finish().InProcessAsync().Result.First;

                var processedImage = buildEncodeResult.TryGetBytes();

                if (processedImage?.Array == null) return;

                // return imageflow processed image 
                context.Response.ContentType = buildEncodeResult.PreferredMimeType;
                context.Response.BinaryWrite(processedImage.Value.Array);

                context.Response.End(); // terminate request
            }
        }
    }
}
