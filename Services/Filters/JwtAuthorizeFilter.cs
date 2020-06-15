using BlogMVC.Services.Interfaces;
using System.Web.Mvc;


namespace BlogMVC.Services.Filters
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {
        IAuthService _service = DependencyResolver.Current.GetService<IAuthService>();

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var authorized = false;

            foreach(var role in Roles.Split(','))
            {
                if (_service.Authorize(role))
                {
                    authorized = true;
                    break;
                }
            }

            if (! authorized)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}
