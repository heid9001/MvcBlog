using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogMVC.Models.Transfer
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}