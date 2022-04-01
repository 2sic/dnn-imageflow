using System;
using System.Collections.Specialized;
using ToSic.Imageflow.Dnn.Job;

namespace ToSic.Imageflow.Dnn
{
    /// <summary>
    /// StartUp is helper class to enable registration of QueryStringRewrite functionality from
    /// main 2sxc dnn module
    /// </summary>
    public static class StartUp
    {
        /// <summary>
        /// Register QueryStringRewrite function for use in ImageJobInfo.
        /// This registration should be called from the main 2sxc dnn module
        /// before we use dnn imageflow.
        /// </summary>
        /// <param name="queryStringRewrite"></param>
        public static void RegisterQueryStringRewrite(Func<NameValueCollection, NameValueCollection> queryStringRewrite) 
            => ImageJobInfo.QueryStringRewrite = queryStringRewrite;
    }
}
