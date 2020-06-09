using System.Linq;
using System.Web;
using BlogMVC.Utils;
using BlogMVC.Models;
using BlogMVC.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;


namespace BlogMVC.Services
{

    public class AuthService: IAuthService
    {
        ModelsContext _db;
        IUserService _userService;
        ITokenService _tokenService;

        public AuthService(IUserService userService, ITokenService tokenService, ModelsContext ctx)
        {
            _db = ctx;
            _userService = userService;
            _tokenService = tokenService;
        }

        // заведение учетной записи
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
            _db.Users.Add(user);
            _db.SaveChanges();
            return user;
        }

        // установка токена в куки юзера в контекст
        public bool Authenticate(string login, string password)
        {
            var ctx = HttpContext.Current;
            var response = ctx.Response;

            if (ctx.User != null && ctx.User.Identity.IsAuthenticated)
            {
                return true;
            }

            var token = (string) _tokenService.GetTokenFromCookie();

            // если токен есть проверяем подпись
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
                _db.SaveChanges();
                return true;
            }

            // проверка по базе
            var user = _userService.FindBySecret(login+password);
            if (user == null)
            {
                response.StatusCode = 401;
                return false;
            }

            // обновление модели юзера
            user.IsAuthenticated = true;
            _db.SaveChanges();
            ctx.User = new UserPrincipal(user);
            _tokenService.StoreUserToken(user.AuthorizeToken);
            return true;
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

            // получаем токен из кук и по нему юзера из бд
            if (user == null)
            {
                token = _tokenService.GetTokenFromCookie();
                if (token == null)
                {
                    response.StatusCode = 401;
                    return false;
                }

                // поиск юзера по токену в бд
                user = new UserPrincipal(_userService.FindByToken(token));
                if (user == null || ! ((User) user.Identity).AuthorizeToken.Equals(token) || ! user.Identity.IsAuthenticated)
                {
                    response.StatusCode = 401;
                    return false;
                }
            }
            // получаем токен из HttpContext.User
            else
            {
                token = ((User)user.Identity).AuthorizeToken;
            }
            var result = new JwtSecurityTokenHandler().ReadJwtToken(token);

            // проверка роли
            string match = (from c in result.Claims
                        where c.Type.Equals("Role") && c.Value.Equals(role)
                        select c.Value).SingleOrDefault<string>();
            if (match == null)
            {
                response.StatusCode = 401;
                return false;
            }

            // устанавливаем юзера в контекст
            ctx.User = user;
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
                _db.Users.Add(user);
                _db.SaveChanges();
                ctx.User = null;
                _tokenService.RemoveTokenCookie();
                return;
            }

            if (token != null)
            {
                var user = _userService.FindByToken(token);
                user.IsAuthenticated = false;
                _db.SaveChanges();
                _tokenService.RemoveTokenCookie();
                return;
            }
        }
    }
}
