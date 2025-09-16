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
    [Authorize]
    public class Admin_ProjectDetailsController : Controller
    {
        private PersonelWebDbEntities db = new PersonelWebDbEntities();

        // GET: Admin/Admin_ProjectDetails
        public ActionResult Index()
        {
            var tbl_ProjectDetails = db.Tbl_ProjectDetails.Include(t => t.Tbl_Language).Include(t => t.Tbl_Projects);
            return View(tbl_ProjectDetails.ToList());
        }

        // GET: Admin/Admin_ProjectDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_ProjectDetails tbl_ProjectDetails = db.Tbl_ProjectDetails.Find(id);
            if (tbl_ProjectDetails == null)
            {
                return HttpNotFound();
            }
            return View(tbl_ProjectDetails);
        }

        // GET: Admin/Admin_ProjectDetails/Create
        public ActionResult Create()
        {
            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title");
            ViewBag.ProjectID = new SelectList(db.Tbl_Projects, "ID", "ProjectTitle");
            return View();
        }

        // POST: Admin/Admin_ProjectDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LanguageID,ProjectID,ImageURL,ProjectTitle,ProjectDescription,Date,Tags")] Tbl_ProjectDetails tbl_ProjectDetails)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_ProjectDetails.Add(tbl_ProjectDetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_ProjectDetails.LanguageID);
            ViewBag.ProjectID = new SelectList(db.Tbl_Projects, "ID", "ProjectTitle", tbl_ProjectDetails.ProjectID);
            return View(tbl_ProjectDetails);
        }

        // GET: Admin/Admin_ProjectDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_ProjectDetails tbl_ProjectDetails = db.Tbl_ProjectDetails.Find(id);
            if (tbl_ProjectDetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_ProjectDetails.LanguageID);
            ViewBag.ProjectID = new SelectList(db.Tbl_Projects, "ID", "ProjectTitle", tbl_ProjectDetails.ProjectID);
            return View(tbl_ProjectDetails);
        }

        // POST: Admin/Admin_ProjectDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LanguageID,ProjectID,ImageURL,ProjectTitle,ProjectDescription,Date,Tags")] Tbl_ProjectDetails tbl_ProjectDetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_ProjectDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_ProjectDetails.LanguageID);
            ViewBag.ProjectID = new SelectList(db.Tbl_Projects, "ID", "ProjectTitle", tbl_ProjectDetails.ProjectID);
            return View(tbl_ProjectDetails);
        }

        // GET: Admin/Admin_ProjectDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_ProjectDetails tbl_ProjectDetails = db.Tbl_ProjectDetails.Find(id);
            if (tbl_ProjectDetails == null)
            {
                return HttpNotFound();
            }
            return View(tbl_ProjectDetails);
        }

        [HttpPost]
        public JsonResult DeleteProjectDetail(int id)
        {
            try
            {
                Tbl_ProjectDetails tbl_ProjectDetails = db.Tbl_ProjectDetails.Find(id);
                if (tbl_ProjectDetails == null)
                {
                    return Json(new { success = false, message = "Project not found." });
                }
                db.Tbl_ProjectDetails.Remove(tbl_ProjectDetails);
                db.SaveChanges();
                return Json(new { success = true, message = "Project deleted successfully." });

            }
            catch
            {
                return Json(new { success = false, message = "An error occurred while deleting the project." });
            }
        }


        // POST: Admin/Admin_ProjectDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_ProjectDetails tbl_ProjectDetails = db.Tbl_ProjectDetails.Find(id);
            db.Tbl_ProjectDetails.Remove(tbl_ProjectDetails);
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
