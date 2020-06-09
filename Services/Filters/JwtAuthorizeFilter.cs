using BlogMVC.Models;
using BlogMVC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;
using System.Diagnostics;

namespace BlogMVC.Services.Filters
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {
        IAuthService _service = DependencyResolver.Current.GetService<IAuthService>();

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!IsMarked(filterContext)) return;

            var authorized = false;
            foreach(var role in Roles.Split(','))
            {
                authorized = _service.Authorize(role);
            }

            if (! authorized)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        public bool IsMarked(AuthorizationContext filterContext)
        {
            foreach (var attr in filterContext.ActionDescriptor.GetCustomAttributes(false))
            {
                if (attr.GetType() == typeof(JwtAuthorizeAttribute))
                {
                    return true;
                }
            }
            return false;
        }
    }
}