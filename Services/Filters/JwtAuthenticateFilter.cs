using BlogMVC.Models;
using BlogMVC.Services.Interfaces;
using System.Web.Mvc;
using System.Web.Mvc.Filters;


namespace BlogMVC.Services.Filters
{
    public class JwtAuthenticateFilter : IAuthenticationFilter
    {
        IAuthService _service = DependencyResolver.Current.GetService<IAuthService>();
        ModelsContext _db = DependencyResolver.Current.GetService<ModelsContext>();

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var token = filterContext.HttpContext.Request.Cookies["_token"];
            if (token == null)
            {
                return;
            }

            var principal = _service.GetUserByToken(token.Value);
            if (principal == null)
            {
                filterContext.Result = new HttpUnauthorizedResult();
                return;
            }

            var user = (User)principal.Identity;
            user.IsAuthenticated = true;
            _db.SaveChanges();
            filterContext.HttpContext.User = principal;
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }
    }
}