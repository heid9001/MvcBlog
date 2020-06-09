using System.Web.Mvc;
using System.Web.Routing;

namespace BlogMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Auth",
                url: "Auth/{action}",
                defaults: new { controller = "Auth", action = "Login" }
            );

            routes.MapRoute(
                name: "Home",
                url:  "Home/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );

        }
    }
}
