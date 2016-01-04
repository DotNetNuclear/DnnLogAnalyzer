using System;
using DotNetNuke.Web.Api;
using DotNetNuclear.Modules.LogAnalyzer.Components;

namespace DotNetNuclear.Modules.LogAnalyzer.Services
{
    public class APIServices : IServiceRouteMapper
    {
        public void RegisterRoutes(IMapRoute routeManager)
        {
            routeManager.MapHttpRoute(FeatureController.DESKTOPMODULE_NAME, "default", "{controller}/{action}",
                    new[] { "DotNetNuclear.Modules.LogAnalyzer.Services.Controllers" });
        }
    }
}