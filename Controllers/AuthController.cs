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
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
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
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Login()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return View();
            }

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        [HttpPost]
        public ActionResult Login(UserDto user)
        {
            if (! ModelState.IsValid)
            {
                return View(user);
            }
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        /* asp может закешировать страницу перед удалением cookie */
        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Register()
        {
            if (! HttpContext.User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToRoute(new { controller = "Auth", action = "Login" });
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
