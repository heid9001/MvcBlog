using BlogMVC.Models;


namespace BlogMVC.Services.Interfaces
{
    public interface IAuthService
    {
        User Register(string login, string password, string role);
        bool Authenticate(string login, string password);
        bool IsAuthenticated();
        UserPrincipal GetUserByToken(string token);

        bool Authorize(string role);
        void Logout();
    }
}
