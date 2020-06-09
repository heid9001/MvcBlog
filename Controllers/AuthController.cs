using System;
using System.Web.Mvc;
using BlogMVC.Models.Transfer;
using BlogMVC.Models.Validators;
using BlogMVC.Services.Filters;
using BlogMVC.Services.Interfaces;


namespace BlogMVC.Controllers
{
    public class AuthController : Controller
    {
        IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // GET: Auth
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserDto user)
        {
            if (user.ValidateForLogin())
            {
                _authService.Authenticate(user.Name, user.Password);
                return RedirectToRoute("Home", new { action = "Index"});
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserDto user)
        {
            if (user.ValidateForRegistration())
            {
                _authService.Register(user.Name, user.Password, user.Role);
            }

            return RedirectToRoute("Home", new { action = "Index" });
        }

        [JwtAuthorize(Roles = "admin")]
        public ActionResult Foo()
        {

            return Content("JwtAuthorize Granted!");
        }

        public ActionResult Bar()
        {
            return Content("No Authorize Attribute!");
        }
    }
}
