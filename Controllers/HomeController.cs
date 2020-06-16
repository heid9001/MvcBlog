using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace BlogMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}