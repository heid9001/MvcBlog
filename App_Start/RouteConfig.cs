﻿using System.Web.Mvc;
using System.Web.Routing;

namespace BlogMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Auth", action = "Login", id = UrlParameter.Optional }
            );

            /* routes.MapRoute(
                 name: "Home",
                 url:  "Home/{action}",
                 defaults: new { controller = "Home", action = "Index" }
             );

             routes.MapRoute(
                 name: "Articles",
                 url: "Articles/{action}",
                 defaults: new { controller = "Articles", action = "Index" }
             );

             routes.MapRoute(
                 name: "Users",
                 url: "Users/{action}",
                 defaults: new { controller = "Users", action = "Index" }
             );
             */

        }
    }
}
