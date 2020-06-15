using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web.Mvc;
using BlogMVC.Models;
using BlogMVC.Services.Filters;
using BlogMVC.Utils;


namespace BlogMVC.Controllers
{
    [JwtAuthorize(Roles = "admin")]
    public class UsersController : Controller
    {
        public ModelsContext db => DependencyResolver.Current.GetService<ModelsContext>();

        public ActionResult Index()
        {

            return View(db.Users.ToList());
        }

        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Create()
        {
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var key = JwtUtils.CreateSymmetricKey(user.Name + user.Password);
                var token = JwtUtils.CreateJWSToken(key, new Claim("Role", user.Role));
                user.IdentityKey = Encoding.UTF8.GetString(key.Key);
                user.AuthorizeToken = token;
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            var old = db.Users.Where(a => a.Id == user.Id).SingleOrDefault();

            if (ModelState.IsValid)
            {
                var key = JwtUtils.CreateSymmetricKey(user.Name + user.Password);
                var token = JwtUtils.CreateJWSToken(key, new Claim("Role", user.Role));
                old.IdentityKey = Encoding.UTF8.GetString(key.Key);
                old.AuthorizeToken = token;
                old.Name = user.Name;
                old.IsAuthenticated = user.IsAuthenticated;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var currentUser = (User) HttpContext.User.Identity;

            if (currentUser.Id == id)
            {
                return new HttpNotFoundResult();
            }

            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            /*if (disposing)
            {
                db.Dispose();
            }*/
            base.Dispose(disposing);
        }
    }
}
