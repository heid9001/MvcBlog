using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using SymmetricSecurityKey = Microsoft.IdentityModel.Tokens.SymmetricSecurityKey;
using SigningCredentials = Microsoft.IdentityModel.Tokens.SigningCredentials;
using SecurityAlgorithms = Microsoft.IdentityModel.Tokens.SecurityAlgorithms;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using System;


namespace BlogMVC.Utils
{
    public static class JwtUtils
    {
        static JwtSecurityTokenHandler _handler = new JwtSecurityTokenHandler();

        // создание закрытого ключа
        public static SymmetricSecurityKey CreateSymmetricKey(string word)
        {
            return new SymmetricSecurityKey(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(word)));
        }

        // создание токена
        public static string CreateJWSToken(SymmetricSecurityKey secret, IEnumerable<Claim> claims)
        {
            var creds = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256Signature);
            JwtHeader header = new JwtHeader(creds);
            JwtPayload payload = new JwtPayload(claims);
            JwtSecurityToken token = new JwtSecurityToken(header, payload);
            return _handler.WriteToken(token);
        }
        
        public static string CreateJWSToken(SymmetricSecurityKey secret, Claim claim)
        {
            return CreateJWSToken(secret, new Claim[] { claim });
        }

        // проверка формата токена
        public static bool ValidateToken(string token)
        {
            return _handler.CanReadToken(token);
        }

        // проверка подписи токена 
        public static JwtSecurityToken VerifyToken(SymmetricSecurityKey secret, string token)
        {
            SecurityToken validatedToken;
            try
            {
                var validationParams = new TokenValidationParameters()
                {
                    IssuerSigningKey = secret,
                    ValidateLifetime = false,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true
                };
                new JwtSecurityTokenHandler().ValidateToken(token, validationParams, out validatedToken);
            }
            catch (SecurityTokenInvalidSignatureException e)
            {
                return null;
            }
            return (JwtSecurityToken)validatedToken;
        }
    }
}