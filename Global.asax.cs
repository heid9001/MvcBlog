using BlogMVC.Models;
using BlogMVC.Models.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;

namespace BlogMVC
{
    public class MvcApplication : HttpApplication
    {
        protected static UnityContainer _container = new UnityContainer();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterPerApp(_container);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_EndRequest()
        {
            UnityConfig.RegisterPerRequest(_container);
        }
    }
}
