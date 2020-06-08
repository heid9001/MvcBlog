using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using BlogMVC.Services;
using BlogMVC.Services.Interfaces;

namespace BlogMVC.Models
{
    public class UserPrincipal : IPrincipal
    {
        User _user;
        IAuthService _service;


        public UserPrincipal(User user)
        {
            _user = user;
            _service = DependencyResolver.Current.GetService<IAuthService>();
        }

        public UserPrincipal(User user, IAuthService service)
        {
            _user = user;
            _service = service;
        }

        public IIdentity Identity => _user;

        public bool IsInRole(string role)
        {
            return _service.Authorize(role);
        }
    }
}