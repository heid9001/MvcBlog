using BlogMVC.Services.Filters;
using System.Web.Mvc;

namespace BlogMVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new JwtAuthenticateFilter());
        }
    }
}
