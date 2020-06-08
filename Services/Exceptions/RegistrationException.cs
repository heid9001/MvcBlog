using BlogMVC.Models;
using System;


namespace BlogMVC.Services.Exceptions
{
    public class RegistrationException: Exception
    {
        public RegistrationException()
            : base("User already registered")
        {
        }
    }
}