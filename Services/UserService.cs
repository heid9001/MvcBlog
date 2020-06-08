using System.Linq;
using System.Text;
using BlogMVC.Models;
using BlogMVC.Utils;
using BlogMVC.Services.Interfaces;


namespace BlogMVC.Services
{
    public class UserService: IUserService
    {
        ModelsContext _db;

        public UserService(ModelsContext ctx)
        {
            _db = ctx;
        }

        public User FindBySecret(string str)
        {
            var key = Encoding.UTF8.GetString(JwtUtils.CreateSymmetricKey(str).Key);
            var user = (from u in _db.Users
                        where u.IdentityKey.Equals(key)
                        select u).SingleOrDefault<User>();
            return user;
        }

        public User FindByToken(string token)
        {
            var user = (from u in _db.Users
                        where u.AuthorizeToken.Equals(token)
                        select u).SingleOrDefault<User>();
            return user;
        }

        public User FindUserByName(string name)
        {
            var user = (from u in _db.Users
                        where u.Name.Equals(name)
                        select u).SingleOrDefault<User>();
            return user;
        }
    }
}
