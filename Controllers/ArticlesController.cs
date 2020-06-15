using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BlogMVC.Models;
using BlogMVC.Services.Filters;
using BlogMVC.Services.Interfaces;

namespace BlogMVC.Controllers
{
    [JwtAuthorize(Roles = "admin,user")]
    public class ArticlesController : Controller
    {
        private ModelsContext _db;
        private IAuthService _auth;


        public ArticlesController(ModelsContext db, IAuthService auth)
        {
            _db = db;
            _auth = auth;
        }

        // GET: Articles
        public ActionResult Index()
        {
            var user = (User)HttpContext.User.Identity;
            _db.Entry(user).State = EntityState.Modified;

            return View(user.Articles.ToList());
        }

        // GET: Articles/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = _db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: Articles/Create
        public ActionResult Create()
        {
            var user = (User)HttpContext.User.Identity;
            return View(new Article(user));
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content")] Article article)
        {
            var user = (User) HttpContext.User.Identity as User;
            _db.Entry(user).State = EntityState.Unchanged;
            article.User = user;


            if (ModelState.IsValid)
            {
                _db.Articles.Add(article);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(article);
        }

        // GET: Articles/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = (User)HttpContext.User.Identity;
            _db.Entry(user).State = EntityState.Unchanged;

            Article article = _db.Articles.Find(id);
            article.User = user;
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content")] Article article)
        {
            var user = (User) HttpContext.User.Identity;
            // TODO: refactor
            // _db.Entry(user).State = EntityState.Detached;
            _db.Entry(user).State = EntityState.Unchanged;
            article.User = user;
            if (ModelState.IsValid)
            {
                _db.Entry(article).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(article);
        }

        // GET: Articles/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = _db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Article article = _db.Articles.Find(id);
            _db.Articles.Remove(article);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
