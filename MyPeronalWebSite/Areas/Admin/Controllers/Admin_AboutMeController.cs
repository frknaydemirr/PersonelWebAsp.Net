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
    public class Admin_AboutMeController : Controller
    {
        private PersonelWebDbEntities db = new PersonelWebDbEntities();

        // GET: Admin/Admin_AboutMe
        public ActionResult Index()
        {
            var tbl_AboutMe = db.Tbl_AboutMe.Include(t => t.Tbl_Language);
            return View(tbl_AboutMe.ToList());
        }

        // GET: Admin/Admin_AboutMe/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_AboutMe tbl_AboutMe = db.Tbl_AboutMe.Find(id);
            if (tbl_AboutMe == null)
            {
                return HttpNotFound();
            }
            return View(tbl_AboutMe);
        }

        // GET: Admin/Admin_AboutMe/Create
        public ActionResult Create()
        {
            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title");
            return View();
        }

        // POST: Admin/Admin_AboutMe/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ImageURL,Description,ShortDescription,LanguageID,LinkedlnURL,GithubURL,EmailAdress,PhoneNumber,Adress,Cv")] Tbl_AboutMe tbl_AboutMe)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_AboutMe.Add(tbl_AboutMe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_AboutMe.LanguageID);
            return View(tbl_AboutMe);
        }

        // GET: Admin/Admin_AboutMe/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_AboutMe tbl_AboutMe = db.Tbl_AboutMe.Find(id);
            if (tbl_AboutMe == null)
            {
                return HttpNotFound();
            }
            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_AboutMe.LanguageID);
            return View(tbl_AboutMe);
        }

        // POST: Admin/Admin_AboutMe/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ImageURL,Description,ShortDescription,LanguageID,LinkedlnURL,GithubURL,EmailAdress,PhoneNumber,Adress,Cv")] Tbl_AboutMe tbl_AboutMe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_AboutMe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_AboutMe.LanguageID);
            return View(tbl_AboutMe);
        }

        // GET: Admin/Admin_AboutMe/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_AboutMe tbl_AboutMe = db.Tbl_AboutMe.Find(id);
            if (tbl_AboutMe == null)
            {
                return HttpNotFound();
            }
            return View(tbl_AboutMe);
        }

        // POST: Admin/Admin_AboutMe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_AboutMe tbl_AboutMe = db.Tbl_AboutMe.Find(id);
            db.Tbl_AboutMe.Remove(tbl_AboutMe);
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
