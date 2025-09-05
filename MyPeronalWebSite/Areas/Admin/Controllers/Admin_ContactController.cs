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
    public class Admin_ContactController : Controller
    {
        private PersonelWebDbEntities db = new PersonelWebDbEntities();

        // GET: Admin/Admin_Contact
        public ActionResult Index()
        {
            var tbl_Contact = db.Tbl_Contact.Include(t => t.Tbl_Language);
            return View(tbl_Contact.ToList());
        }

        // GET: Admin/Admin_Contact/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Contact tbl_Contact = db.Tbl_Contact.Find(id);
            if (tbl_Contact == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Contact);
        }

        // GET: Admin/Admin_Contact/Create
        public ActionResult Create()
        {
            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title");
            return View();
        }

        // POST: Admin/Admin_Contact/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LanguageID,Name,Email,PhoneNumber,Subcejt,Message")] Tbl_Contact tbl_Contact)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Contact.Add(tbl_Contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Contact.LanguageID);
            return View(tbl_Contact);
        }

        // GET: Admin/Admin_Contact/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Contact tbl_Contact = db.Tbl_Contact.Find(id);
            if (tbl_Contact == null)
            {
                return HttpNotFound();
            }
            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Contact.LanguageID);
            return View(tbl_Contact);
        }

        // POST: Admin/Admin_Contact/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LanguageID,Name,Email,PhoneNumber,Subcejt,Message")] Tbl_Contact tbl_Contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Contact.LanguageID);
            return View(tbl_Contact);
        }

        // GET: Admin/Admin_Contact/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Contact tbl_Contact = db.Tbl_Contact.Find(id);
            if (tbl_Contact == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Contact);
        }

        // POST: Admin/Admin_Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Contact tbl_Contact = db.Tbl_Contact.Find(id);
            db.Tbl_Contact.Remove(tbl_Contact);
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
