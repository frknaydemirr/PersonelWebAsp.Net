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
    public class Admin_TechnologiesController : Controller
    {
        private PersonelWebDbEntities db = new PersonelWebDbEntities();

        // GET: Admin/Admin_Technologies
        public ActionResult Index()
        {
            var tbl_Technologies = db.Tbl_Technologies.Include(t => t.Tbl_Language);
            return View(tbl_Technologies.ToList());
        }

        // GET: Admin/Admin_Technologies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Technologies tbl_Technologies = db.Tbl_Technologies.Find(id);
            if (tbl_Technologies == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Technologies);
        }

        // GET: Admin/Admin_Technologies/Create
        public ActionResult Create()
        {
            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title");
            return View();
        }

        // POST: Admin/Admin_Technologies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ImageURL,LanguageID")] Tbl_Technologies tbl_Technologies)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Technologies.Add(tbl_Technologies);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Technologies.LanguageID);
            return View(tbl_Technologies);
        }

        // GET: Admin/Admin_Technologies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Technologies tbl_Technologies = db.Tbl_Technologies.Find(id);
            if (tbl_Technologies == null)
            {
                return HttpNotFound();
            }
            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Technologies.LanguageID);
            return View(tbl_Technologies);
        }

        // POST: Admin/Admin_Technologies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ImageURL,LanguageID")] Tbl_Technologies tbl_Technologies)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Technologies).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Technologies.LanguageID);
            return View(tbl_Technologies);
        }

        // GET: Admin/Admin_Technologies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Technologies tbl_Technologies = db.Tbl_Technologies.Find(id);
            if (tbl_Technologies == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Technologies);
        }

        // POST: Admin/Admin_Technologies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Technologies tbl_Technologies = db.Tbl_Technologies.Find(id);
            db.Tbl_Technologies.Remove(tbl_Technologies);
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
