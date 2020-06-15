using System.Linq;
using System.Text;
using BlogMVC.Models;
using BlogMVC.Utils;
using BlogMVC.Services.Interfaces;
using System.Web.Mvc;


namespace BlogMVC.Services
{
    public class UserService: IUserService
    {
        public UserService()
        {
        }

        public ModelsContext Db => DependencyResolver.Current.GetService<ModelsContext>();

        public User FindBySecret(string str)
        {
            var key = Encoding.UTF8.GetString(JwtUtils.CreateSymmetricKey(str).Key);
            var user = (from u in Db.Users
                        where u.IdentityKey.Equals(key)
                        select u).SingleOrDefault<User>();
            return user;
        }

        public User FindByToken(string token, bool tracking = true)
        {
            User user;
            if (tracking)
            {
                user = (from u in Db.Users
                        where u.AuthorizeToken.Equals(token)
                        select u).SingleOrDefault<User>();
            } else
            {
                user = (from u in Db.Users.AsNoTracking()
                        where u.AuthorizeToken.Equals(token)
                        select u).SingleOrDefault<User>();
            }
            
            return user;
        }

        public User FindUserByName(string name)
        {
            var user = (from u in Db.Users
                        where u.Name.Equals(name)
                        select u).SingleOrDefault<User>();
            return user;
        }
    }
}
