using MyPeronalWebSite.Models.VT;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MyPeronalWebSite.Areas.Admin.Controllers
{
    public class Admin_BlogController : Controller
    {
        private PersonelWebDbEntities db = new PersonelWebDbEntities();

        // GET: Admin/Admin_Blog
        public ActionResult Index()
        {
            var tbl_Blog = db.Tbl_Blog.Include(t => t.Tbl_Language);
            return View(tbl_Blog.ToList());
        }

        // GET: Admin/Admin_Blog/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Blog tbl_Blog = db.Tbl_Blog.Find(id);
            if (tbl_Blog == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Blog);
        }

        // GET: Admin/Admin_Blog/Create
        public ActionResult Create()
        {
            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title");
            return View();
        }

        // POST: Admin/Admin_Blog/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Description,Date,LanguageID,ImageURL,MediumURL")] Tbl_Blog tbl_Blog)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Blog.Add(tbl_Blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Blog.LanguageID);
            return View(tbl_Blog);
        }

        // GET: Admin/Admin_Blog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Blog tbl_Blog = db.Tbl_Blog.Find(id);
            if (tbl_Blog == null)
            {
                return HttpNotFound();
            }
            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Blog.LanguageID);
            return View(tbl_Blog);
        }

        // POST: Admin/Admin_Blog/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Description,Date,LanguageID,ImageURL,MediumURL")] Tbl_Blog tbl_Blog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Blog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Blog.LanguageID);
            return View(tbl_Blog);
        }

        // GET: Admin/Admin_Blog/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Blog tbl_Blog = db.Tbl_Blog.Find(id);
            if (tbl_Blog == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Blog);
        }

        // POST: Admin/Admin_Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Blog tbl_Blog = db.Tbl_Blog.Find(id);
            db.Tbl_Blog.Remove(tbl_Blog);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
