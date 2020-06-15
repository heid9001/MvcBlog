using System;
using System.Web.Mvc;
using BlogMVC.Models.Transfer;
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

        [HttpGet]
        public ActionResult Logout()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                _authService.Logout();
            }
            return RedirectToAction("Login", "Auth");
        }

        // GET: Auth
        [HttpGet]
        public ActionResult Login()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return View();
            }

            return RedirectToRoutePermanent("Home", new { action = "Index" });
        }

        [HttpPost]
        public ActionResult Login(UserDto user)
        {
            if (! ModelState.IsValid)
            {
                return View(user);
            }
            return RedirectToRoute("Home", new { action = "Index" });
        }

        [HttpGet]
        public ActionResult Register()
        {
            if (! HttpContext.User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToRoute("Home", new { action = "Index" });
        }

        [HttpPost]
        public ActionResult Register(UserDto user)
        {
            if (! ModelState.IsValid)
            {
                return View(user); ;
            }
            return RedirectToAction("Login", "Auth");
        }
    }
}
