using BlogMVC.Models;
using BlogMVC.Services;
using BlogMVC.Services.Interfaces;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace BlogMVC
{
    public static class UnityConfig
    {
        public static void RegisterPerApp(UnityContainer appContainer)
        {
            appContainer.RegisterType<ModelsContext>(TypeLifetime.PerThread);
            appContainer.RegisterType<ITokenService, JwtService>();
            appContainer.RegisterType<IUserService, UserService>();
            appContainer.RegisterType<IAuthService, AuthService>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(appContainer));
        }
    }
}
