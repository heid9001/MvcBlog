using System;
using System.Security.Claims;
using System.Text;
using System.Web;
using BlogMVC.Utils;
using BlogMVC.Services.Interfaces;


namespace BlogMVC.Services
{
    public class JwtService: ITokenService
    {
        const string TOKEN_NAME = "_token";

        public string GetTokenFromCookie()
        {
            var request = HttpContext.Current.Request;
            var tokenCookie = request.Cookies[TOKEN_NAME];
            string token;
            if (tokenCookie == null) return null;
            token = tokenCookie.Value;
            if (!JwtUtils.ValidateToken(token)) return null;
            return token;
        }

        public void StoreUserToken(string token)
        {
            var response = HttpContext.Current.Response;
            response.SetCookie(new HttpCookie(TOKEN_NAME, token));
        }

        // удаление токена из кук
        public void RemoveTokenCookie()
        {
            var response = HttpContext.Current.Response;
            var cookie = new HttpCookie(TOKEN_NAME, "");
            cookie.Expires = DateTime.Now - TimeSpan.FromDays(1);
            response.SetCookie(cookie);
        }

        public object CreateUserToken(string login, string password, string role)
        {
            var secret = JwtUtils.CreateSymmetricKey(login + password);
            var token = JwtUtils.CreateJWSToken(secret, new Claim("Role", role));
            return new JwtPair() { Secret = Encoding.UTF8.GetString(secret.Key), Token = token };
        }
    }
}
