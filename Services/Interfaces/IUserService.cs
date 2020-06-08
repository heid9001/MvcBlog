using BlogMVC.Models;


namespace BlogMVC.Services.Interfaces
{
    public interface IUserService
    {
        User FindBySecret(string str);
        User FindByToken(string token);
        User FindUserByName(string name);
    }
}
