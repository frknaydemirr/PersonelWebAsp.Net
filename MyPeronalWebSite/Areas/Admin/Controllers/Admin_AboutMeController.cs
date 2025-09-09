using MyPeronalWebSite.Models.VT;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult Edit(Tbl_AboutMe tbl_AboutMe, HttpPostedFileBase ImageURL = null, HttpPostedFileBase CvPDF = null)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_AboutMe.LanguageID);
                    return View(tbl_AboutMe);
                }

                // --- Profil Resmi Güncelleme ---
                if (ImageURL != null && ImageURL.ContentLength > 0)
                {
                    // 1. Format kontrolü
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                    var extension = Path.GetExtension(ImageURL.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_AboutMe.LanguageID);
                        ViewBag.ErrorMessage = "Sadece JPG, JPEG, PNG, GIF veya WEBP formatında resimler yükleyebilirsiniz";
                        return View(tbl_AboutMe);
                    }

                    // 2. Boyut kontrolü (2 MB)
                    if (ImageURL.ContentLength > 2 * 1024 * 1024)
                    {
                        ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_AboutMe.LanguageID);
                        ViewBag.ErrorMessage = "Resim boyutu 2MB'tan büyük olamaz";
                        return View(tbl_AboutMe);
                    }

                    // 3. Klasör kontrolü
                    var uploadPath = Server.MapPath("~/Uploads/AboutMe");
                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    // 4. Yeni resmi kaydet
                    var fileName = Guid.NewGuid().ToString() + extension;
                    var filePath = Path.Combine(uploadPath, fileName);
                    ImageURL.SaveAs(filePath);

                    // 5. Eski resmi sil
                    if (!string.IsNullOrEmpty(tbl_AboutMe.ImageURL))
                    {
                        var oldFilePath = Server.MapPath(tbl_AboutMe.ImageURL);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    tbl_AboutMe.ImageURL = "/Uploads/AboutMe/" + fileName;
                }
                else
                {
                    // Resim değişmediyse mevcut URL'yi koru
                    var existingEntity = db.Tbl_AboutMe.AsNoTracking().FirstOrDefault(m => m.ID == tbl_AboutMe.ID);
                    if (existingEntity != null)
                    {
                        tbl_AboutMe.ImageURL = existingEntity.ImageURL;
                    }
                }

                // --- CV PDF Güncelleme ---
                if (CvPDF != null && CvPDF.ContentLength > 0)
                {
                    // 1. Format kontrolü
                    var allowedPdfExtensions = new[] { ".pdf" };
                    var pdfExtension = Path.GetExtension(CvPDF.FileName).ToLower();

                    if (!allowedPdfExtensions.Contains(pdfExtension))
                    {
                        ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_AboutMe.LanguageID);
                        ViewBag.ErrorMessage = "Sadece PDF formatında dosya yükleyebilirsiniz";
                        return View(tbl_AboutMe);
                    }

                    // 2. Boyut kontrolü (5 MB)
                    if (CvPDF.ContentLength > 5 * 1024 * 1024)
                    {
                        ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_AboutMe.LanguageID);
                        ViewBag.ErrorMessage = "PDF boyutu 5MB'tan büyük olamaz";
                        return View(tbl_AboutMe);
                    }

                    // 3. Klasör kontrolü
                    var cvUploadPath = Server.MapPath("~/Uploads/CV");
                    if (!Directory.Exists(cvUploadPath))
                        Directory.CreateDirectory(cvUploadPath);

                    // 4. Yeni PDF kaydet
                    var pdfFileName = Guid.NewGuid().ToString() + pdfExtension;
                    var pdfFilePath = Path.Combine(cvUploadPath, pdfFileName);
                    CvPDF.SaveAs(pdfFilePath);

                    // 5. Eski PDF sil
                    if (!string.IsNullOrEmpty(tbl_AboutMe.Cv))
                    {
                        var oldPdfPath = Server.MapPath(tbl_AboutMe.Cv);
                        if (System.IO.File.Exists(oldPdfPath))
                        {
                            System.IO.File.Delete(oldPdfPath);
                        }
                    }

                    tbl_AboutMe.Cv = "/Uploads/CV/" + pdfFileName;
                }
                else
                {
                    // CV değişmediyse mevcut PDF yolunu koru
                    var existingEntity = db.Tbl_AboutMe.AsNoTracking().FirstOrDefault(m => m.ID == tbl_AboutMe.ID);
                    if (existingEntity != null)
                    {
                        tbl_AboutMe.Cv = existingEntity.Cv;
                    }
                }

                // --- Veritabanını Güncelle ---
                db.Entry(tbl_AboutMe).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_AboutMe.LanguageID);
                ViewBag.ErrorMessage = "Güncelleme sırasında hata oluştu: " + ex.Message;
                return View(tbl_AboutMe);
            }
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
