using System.Linq;
using System.Net;
using System.Web.Mvc;
using BlogMVC.Models;
using BlogMVC.Services.Filters;


namespace BlogMVC.Controllers
{
    [JwtAuthorize(Roles = "admin,user")]
    public class ArticlesController : Controller
    {
        public ModelsContext db => DependencyResolver.Current.GetService<ModelsContext>();


        // GET: Articles
        public ActionResult Index()
        {
            var user = (User)HttpContext.User.Identity;

            return View(user.Articles.ToList());
        }

        // GET: Articles/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
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
            var user = (User) HttpContext.User.Identity;
            article.User = user;


            if (ModelState.IsValid)
            {
                db.Articles.Add(article);
                db.SaveChanges();
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
            
            Article article = db.Articles.Find(id);
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
        public ActionResult Edit(Article article)
        {
            var user = (User)HttpContext.User.Identity;
            if (ModelState.IsValid)
            {
                var old = db.Articles.Where(a => a.Id == article.Id).SingleOrDefault();
                old.Title = article.Title;
                old.Content = article.Content;

                db.SaveChanges();
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
            Article article = db.Articles.Find(id);
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
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
/*            if (disposing)
            {
                _db.Dispose();
            }*/
            base.Dispose(disposing);
        }
    }
}
