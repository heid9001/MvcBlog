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
            UnityConfig.RegisterPerApp(_container);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
