using BlogMVC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace BlogMVC.Models.Transfer
{
    public class UserDto: IValidatableObject
    {
        IAuthService _authService = DependencyResolver.Current.GetService<IAuthService>();

        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            var routeInfo = HttpContext.Current.Request.RequestContext.RouteData.Values;
            var methodType = HttpContext.Current.Request.HttpMethod;

            if (! routeInfo["controller"].Equals("Auth") && methodType != HttpMethod.Post.ToString())
            {
                return errors;
            }

            var action = routeInfo["action"];

            if (action.Equals("Login"))
            {
                ValidateForLogin(errors);
            }
            else if (action.Equals("Register"))
            {
                ValidateForRegister(errors);
            }

            return errors;
        }

        void ValidateForLogin(List<ValidationResult> errors)
        {
            if (Name == null)
            {
                errors.Add(new ValidationResult("Введите имя"));
            }
            if (Password == null)
            {
                errors.Add(new ValidationResult("Введите пароль"));
            }
            if (errors.Count == 0)
            {
                if (!_authService.Authenticate(Name, Password))
                {
                    errors.Add(new ValidationResult("Пользователь не найден"));
                }
            }
        }

        void ValidateForRegister(List<ValidationResult> errors)
        {
            if (Name == null)
            {
                errors.Add(new ValidationResult("Введите имя"));
            }
            if (Password == null)
            {
                errors.Add(new ValidationResult("Введите пароль"));
            }
            if (Role == null)
            {
                errors.Add(new ValidationResult("Введите роль"));
            }

            if (errors.Count == 0)
            {
                if (_authService.Register(Name, Password, Role) == null)
                {
                    errors.Add(new ValidationResult("Пользователь уже существует"));
                }
            }
        }
    }
}
