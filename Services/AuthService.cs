using System.Linq;
using System.Web;
using BlogMVC.Utils;
using BlogMVC.Models;
using BlogMVC.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Web.Mvc;

namespace BlogMVC.Services
{

    public class AuthService: IAuthService
    {
        IUserService _userService;
        ITokenService _tokenService;

        public AuthService(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        public ModelsContext Db => DependencyResolver.Current.GetService<ModelsContext>();
        

        public bool IsAuthenticated()
        {
            if (_tokenService.GetTokenFromCookie() != null)
            {
                return true;
            }
            return false;
        }

        public User Register(string login, string password, string role)
        {
            var ctx = HttpContext.Current;
            var response = ctx.Response;

            var user = _userService.FindUserByName(login);
            if (user != null)
            {
                response.StatusCode = 401;
                return null;
            }
            var pair = (JwtPair) _tokenService.CreateUserToken(login, password, role);
            user = new User(login, pair, true);
            Db.Users.Add(user);
            Db.SaveChanges();
            return user;
        }

        public bool Authenticate(string login, string password)
        {
            var ctx = HttpContext.Current;
            var response = ctx.Response;

            if (ctx.User != null && ctx.User.Identity.IsAuthenticated)
            {
                return true;
            }
            var token = (string) _tokenService.GetTokenFromCookie();
            if (token != null)
            {
                if (JwtUtils.VerifyToken(JwtUtils.CreateSymmetricKey(login + password), token) == null)
                {
                    _tokenService.RemoveTokenCookie();
                    response.StatusCode = 401;
                    return false;
                }
                var usr = _userService.FindByToken(token);
                usr.IsAuthenticated = true;
                ctx.User = new UserPrincipal(usr);
                Db.SaveChanges();
                return true;
            }
            var user = _userService.FindBySecret(login+password);
            if (user == null)
            {
                response.StatusCode = 401;
                return false;
            }
            user.IsAuthenticated = true;
            Db.SaveChanges();
            ctx.User = new UserPrincipal(user);
            _tokenService.StoreUserToken(user.AuthorizeToken);
            return true;
        }

        public UserPrincipal GetUserByToken(string token)
        {
            var user = _userService.FindByToken(token);
            if (user != null)
            {
                return new UserPrincipal(user);
            }
            return null;
        }

        public bool Authorize(string role)
        {
            var ctx = HttpContext.Current;
            var response = ctx.Response;
            var request =  ctx.Request;

            if (!(ctx.User is UserPrincipal))
            {
                return false;
            }

            UserPrincipal user = (UserPrincipal) ctx.User;
            

            string token;

            if (user == null)
            {
                token = _tokenService.GetTokenFromCookie();
                if (token == null)
                {
                    response.StatusCode = 401;
                    return false;
                }
                user = GetUserByToken(token);
                if (user == null || ! ((User) user.Identity).AuthorizeToken.Equals(token) || ! user.Identity.IsAuthenticated)
                {
                    response.StatusCode = 401;
                    return false;
                }
            }
            else
            {
                token = ((User)user.Identity).AuthorizeToken;
            }
            var result = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string match = (from c in result.Claims
                        where c.Type.Equals("Role") && c.Value.Equals(role)
                        select c.Value).SingleOrDefault<string>();
            if (match == null)
            {
                response.StatusCode = 401;
                return false;
            }

            return true;
        }

        public void Logout()
        {
            var ctx = HttpContext.Current;
            var token = _tokenService.GetTokenFromCookie();

            if (ctx.User != null && ctx.User.Identity.IsAuthenticated)
            {
                var user = (User)ctx.User.Identity;
                user.IsAuthenticated = false;
                Db.SaveChanges();
                ctx.User = null;
                _tokenService.RemoveTokenCookie();
                return;
            }

            if (token != null)
            {
                var user = _userService.FindByToken(token);
                user.IsAuthenticated = false;
                Db.SaveChanges();
                _tokenService.RemoveTokenCookie();
                return;
            }
        }
    }
}
