using BlogMVC.Models;
using BlogMVC.Services;
using BlogMVC.Services.Interfaces;
using System.Diagnostics;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace BlogMVC
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            //container.RegisterType<>();
            container.RegisterType<ModelsContext>();

            container.RegisterInstance<ITokenService>(GetTokenService(container));
            container.RegisterInstance<IUserService>(GetUserService(container));
            container.RegisterInstance<IAuthService>(GetAuthService(container));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        static ITokenService GetTokenService(IUnityContainer container)
        {
            return new JwtService();
        }

        static IUserService GetUserService(IUnityContainer container)
        {
            return new UserService(container.Resolve<ModelsContext>());
        }

        static IAuthService GetAuthService(IUnityContainer container)
        {
            return new AuthService(
                    container.Resolve<IUserService>(),
                    container.Resolve<ITokenService>(),
                    container.Resolve<ModelsContext>()
                );
        }
        
    }
}