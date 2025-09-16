using MyPeronalWebSite.Models.VT;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyPeronalWebSite.Areas.Admin.Controllers
{
    [Authorize]
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
        public ActionResult Create()
        {
            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title");
            return View();
        }

        // POST: Admin/Admin_Technologies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tbl_Technologies tbl_Technologies, HttpPostedFileBase techImage)
        {
            try
            {
                // Model state'i temizle (file input için)
                ModelState.Remove("techImage");

                // Dosya kontrolü
                if (techImage == null || techImage.ContentLength == 0)
                {
                    ModelState.AddModelError("techImage", "Lütfen bir teknoloji görseli seçin");
                    ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Technologies.LanguageID);
                    return View(tbl_Technologies);
                }

                // Dosya uzantısı kontrolü
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".svg" };
                var fileExtension = Path.GetExtension(techImage.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("techImage", "Sadece JPG, JPEG, PNG, GIF, WebP veya SVG formatları kabul edilir");
                    ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Technologies.LanguageID);
                    return View(tbl_Technologies);
                }

                // Dosya boyutu kontrolü (max 2MB)
                if (techImage.ContentLength > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("techImage", "Dosya boyutu 2MB'dan büyük olamaz");
                    ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Technologies.LanguageID);
                    return View(tbl_Technologies);
                }

                // Model state geçerli mi kontrol et
                if (ModelState.IsValid)
                {
                    // Dosyayı kaydet
                    string fileName = Guid.NewGuid().ToString() + fileExtension;
                    string directoryPath = Server.MapPath("~/Content/Images/Technologies");
                    string path = Path.Combine(directoryPath, fileName);

                    // Klasör yoksa oluştur
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    techImage.SaveAs(path);
                    tbl_Technologies.ImageURL = "/Content/Images/Technologies/" + fileName;

                    // Veritabanı işlemi
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.Tbl_Technologies.Add(tbl_Technologies);
                            db.SaveChanges();
                            transaction.Commit();

                            TempData["SuccessMessage"] = "Teknoloji başarıyla eklendi";
                            return RedirectToAction("Index");
                        }
                        catch (Exception dbEx)
                        {
                            transaction.Rollback();
                            // Dosyayı sil (geri alma)
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                            ViewBag.ErrorMessage = "Veritabanı hatası: " + dbEx.Message;
                        }
                    }
                }
                else
                {
                    // Model state hatalarını göster
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.ErrorMessage = "Form doğrulama hataları: " + string.Join(", ", errors.Select(e => e.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Bir hata oluştu: " + ex.Message;
            }

            // Hata durumunda view'i tekrar yükle
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



        [HttpPost]
        public JsonResult DeleteTechnology(int id)
        {
            try
            {
                Tbl_Technologies tbl_Technologies = db.Tbl_Technologies.Find(id);
                if (tbl_Technologies == null)
                {
                    return Json(new { success = false, message = "User not found." });
                }
                db.Tbl_Technologies.Remove(tbl_Technologies);
                db.SaveChanges();
                return Json(new { success = true, message = "User deleted successfully." });

            }
            catch
            {
                return Json(new { success = false, message = "An error occurred while deleting the User." });
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
