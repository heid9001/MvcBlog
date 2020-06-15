using System.Security.Principal;
using System.Web.Mvc;
using BlogMVC.Services.Interfaces;


namespace BlogMVC.Models
{
    public class UserPrincipal : IPrincipal
    {
        User _user;
        IAuthService _service = DependencyResolver.Current.GetService<IAuthService>();


        public UserPrincipal(User user)
        {
            _user = user;
        }

        public IIdentity Identity => _user;

        // 
        public bool IsInRole(string role)
        {
            return _service.Authorize(role);
        }
    }
}
