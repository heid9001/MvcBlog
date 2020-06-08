using System.Web.Mvc;
using BlogMVC.Services;
using BlogMVC.Models;
using BlogMVC.Services.Interfaces;
using Unity;

namespace BlogMVC.Controllers
{
    public class HomeController : Controller
    {
        IAuthService _authService;
        const string Role = "admin";

        public HomeController(IAuthService authService)
        {
            _authService = authService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            if (!_authService.Authorize(Role))
            {
                return Content("Denied");
            }
            return Content("Granted");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Register()
        {
            var request = HttpContext.Request;
            string login = request.Params["login"];
            string password = request.Params["password"];
            string role = request.Params["role"];

            User user = _authService.Register(login, password, role);
            return Content(user.AuthorizeToken);
        }

        public ActionResult Login()
        {
            var request = HttpContext.Request;
            string login = request.Params["login"];
            string password = request.Params["password"];

            if (!_authService.Authenticate(login, password))
            {
                return Content("Denied");
            }
            return Content("Granted");
        }

        public ActionResult Logout()
        {
            _authService.Logout();
            return Content("Logout");
        }
    }
}