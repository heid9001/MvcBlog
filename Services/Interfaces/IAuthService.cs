using BlogMVC.Models;


namespace BlogMVC.Services.Interfaces
{
    public interface IAuthService
    {
        User Register(string login, string password, string role);
        bool Authenticate(string login, string password);
        bool Authorize(string role);
        void Logout();
    }
}
