using BlogMVC.Models;


namespace BlogMVC.Services.Interfaces
{
    public interface IUserService
    {
        User FindBySecret(string str);
        User FindByToken(string token, bool isTracking = true);
        User FindUserByName(string name);
    }
}
