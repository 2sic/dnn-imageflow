using DotNetNuke.Web.Api;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Web;
using ToSic.Imageflow.Dnn.HybridCache;

//using ToSic.Imageflow.Dnn.HybridCache;

namespace ToSic.Imageflow.Dnn
{
    /// <summary>
    /// This configures .net Core Dependency Injection
    /// The StartUp is defined as an IServiceRouteMapper.
    /// This way DNN will auto-run this code before anything else
    /// </summary>
    public class StartupDnn : IServiceRouteMapper
    {
        /// <summary>
        /// This will be called by DNN when loading the assemblies.
        /// We just want to trigger the DI-Configure
        /// </summary>
        /// <param name="mapRouteManager"></param>
        public void RegisterRoutes(IMapRoute mapRouteManager) => DI.Configure();
    }
}
