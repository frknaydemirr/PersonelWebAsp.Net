using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyPeronalWebSite.Models.VT;

namespace MyPeronalWebSite.Areas.Admin.Controllers
{
    public class Admin_ProjectController : Controller
    {
        private PersonelWebDbEntities db = new PersonelWebDbEntities();

        // GET: Admin/Admin_Project
        public ActionResult Index()
        {
            var tbl_Projects = db.Tbl_Projects.Include(t => t.Tbl_Language).Include(t => t.Tbl_Language1);
            return View(tbl_Projects.ToList());
        }

        // GET: Admin/Admin_Project/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Projects tbl_Projects = db.Tbl_Projects.Find(id);
            if (tbl_Projects == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Projects);
        }

        // GET: Admin/Admin_Project/Create
        public ActionResult Create()
        {
            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title");
            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title");
            return View();
        }

        // POST: Admin/Admin_Project/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LanguageID,ProjectTitle,ProjectImg,ProjectDescription,ProjectEntryTitle,ProjectEntryDescription,GithubURL,isActive")] Tbl_Projects tbl_Projects)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Projects.Add(tbl_Projects);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Projects.LanguageID);
            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Projects.LanguageID);
            return View(tbl_Projects);
        }

        // GET: Admin/Admin_Project/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Projects tbl_Projects = db.Tbl_Projects.Find(id);
            if (tbl_Projects == null)
            {
                return HttpNotFound();
            }
            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Projects.LanguageID);
            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Projects.LanguageID);
            return View(tbl_Projects);
        }

        // POST: Admin/Admin_Project/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LanguageID,ProjectTitle,ProjectImg,ProjectDescription,ProjectEntryTitle,ProjectEntryDescription,GithubURL,isActive")] Tbl_Projects tbl_Projects)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Projects).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Projects.LanguageID);
            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Projects.LanguageID);
            return View(tbl_Projects);
        }

        // GET: Admin/Admin_Project/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Projects tbl_Projects = db.Tbl_Projects.Find(id);
            if (tbl_Projects == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Projects);
        }

        // POST: Admin/Admin_Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Projects tbl_Projects = db.Tbl_Projects.Find(id);
            db.Tbl_Projects.Remove(tbl_Projects);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult DeleteProject(int id)
        {
            try
            {
                Tbl_Projects tbl_project = db.Tbl_Projects.Find(id);
                if(tbl_project == null)
                {
                    return Json(new { success = false, message = "Project not found." });
                }
                db.Tbl_Projects.Remove(tbl_project);
                db.SaveChanges();
                return Json(new { success = true, message = "Project deleted successfully." });

            }
            catch
            {
                return Json(new { success = false, message = "An error occurred while deleting the project." });
            }
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
