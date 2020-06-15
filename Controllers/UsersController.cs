using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
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

        // GET: Users
        public ActionResult Index()
        {

            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
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

        // GET: Users/Create
        public ActionResult Create()
        {
            return View(new User());
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,IsAuthenticated,Password,Role")] User user)
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

        // GET: Users/Edit/5
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

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,IsAuthenticated,Password,Role")] User user)
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

        // GET: Users/Delete/5
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
