using System.Collections.Generic;
using System.Web;

namespace ToSic.Imageflow.Dnn.Options
{
    internal class UrlEventArgs
    {
        public UrlEventArgs(HttpContext context, string virtualPath, Dictionary<string, string> query)
        {
            VirtualPath = virtualPath;
            Context = context;
            Query = query;
        }

        public string VirtualPath { get; set; }

        public Dictionary<string, string> Query { get; set; }

        public HttpContext Context { get; }
    }
}
