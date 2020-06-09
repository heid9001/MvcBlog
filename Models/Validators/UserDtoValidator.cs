using BlogMVC.Models.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogMVC.Models.Validators
{
    public static class UserDtoValidator
    {
        public static bool ValidateForRegistration(this UserDto user)
        {
            return user.Name != null
                   && user.Password != null
                   && user.Role != null;
        }

        public static bool ValidateForLogin(this UserDto user)
        {
            return user.Name != null
                   && user.Password != null;
        }
    }
}