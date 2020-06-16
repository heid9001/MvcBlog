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
            
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // объекты ограниченные приложением
            UnityConfig.RegisterPerApp(_container);
        }

        protected void Application_BeginRequest()
        {
            // объекты ограниченные запросом
            UnityConfig.RegisterPerRequestStart(_container);
        }

        protected void Application_EndRequest()
        {
            UnityConfig.RegisterPerRequestEnd(_container);
        }
    }
}
