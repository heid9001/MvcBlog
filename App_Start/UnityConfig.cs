using BlogMVC.Models;
using BlogMVC.Services;
using BlogMVC.Services.Interfaces;
using System.Web;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace BlogMVC
{
    public static class UnityConfig
    {

        public static void RegisterPerApp(UnityContainer appContainer)
        {
            appContainer.RegisterInstance<ITokenService>(GetTokenService(appContainer));
            appContainer.RegisterInstance<IUserService>(GetUserService(appContainer));
            appContainer.RegisterInstance<IAuthService>(GetAuthService(appContainer));
            DependencyResolver.SetResolver(new UnityDependencyResolver(appContainer));
        }

        public static void RegisterPerRequestStart(UnityContainer container)
        {
            container.RegisterInstance<ModelsContext>(GetDbContext(container));
        }

        public static void RegisterPerRequestEnd(UnityContainer container)
        {
            ModelsContext db;
            if ( ( db = container.Resolve<ModelsContext>()) != null)
            {
                db.Dispose();
            }
        }

        static ModelsContext GetDbContext(IUnityContainer container)
        {
            return new ModelsContext();
        }

        static ITokenService GetTokenService(IUnityContainer container)
        {
            return new JwtService();
        }

        static IUserService GetUserService(IUnityContainer container)
        {
            return new UserService();
        }

        static IAuthService GetAuthService(IUnityContainer container)
        {
            return new AuthService(
                    container.Resolve<IUserService>(),
                    container.Resolve<ITokenService>()
                );
        }

    }
}
