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
        [ValidateInput(false)] // HTML içeriğine izin vermek istersen
        public ActionResult Create([Bind(Include = "ID,LanguageID,ProjectTitle,ProjectImg,ProjectDescription,ProjectEntryTitle,ProjectEntryDescription,GithubURL,isActive")] Tbl_Projects tbl_Projects, HttpPostedFileBase ProjectImgFile = null)
        {
            try
            {
                // 1. Model validasyonu
                if (!ModelState.IsValid)
                {
                    ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Projects.LanguageID);
                    ViewBag.ErrorMessage = "Form verileri geçersiz";
                    return View(tbl_Projects);
                }

                // 2. Resim yükleme zorunluluğu
                if (ProjectImgFile == null || ProjectImgFile.ContentLength == 0)
                {
                    ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Projects.LanguageID);
                    ViewBag.ErrorMessage = "Lütfen bir proje resmi seçiniz";
                    return View(tbl_Projects);
                }

                // 3. Resim format kontrolü
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var extension = Path.GetExtension(ProjectImgFile.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Projects.LanguageID);
                    ViewBag.ErrorMessage = "Sadece JPG, JPEG, PNG, GIF veya WEBP formatında resimler yükleyebilirsiniz";
                    return View(tbl_Projects);
                }

                // 4. Resim boyutu kontrolü (5MB)
                if (ProjectImgFile.ContentLength > 5 * 1024 * 1024)
                {
                    ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Projects.LanguageID);
                    ViewBag.ErrorMessage = "Resim boyutu 5MB'tan büyük olamaz";
                    return View(tbl_Projects);
                }

                // 5. Klasör işlemleri
                var uploadPath = Server.MapPath("~/Uploads/Projects");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // 6. Resmi kaydet
                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(uploadPath, fileName);
                ProjectImgFile.SaveAs(filePath);
                tbl_Projects.ProjectImg = "/Uploads/Projects/" + fileName;

                // 7. Veritabanına kaydet
                db.Tbl_Projects.Add(tbl_Projects);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Proje başarıyla oluşturuldu!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Hata yakalama
                string errorMessage = "Kayıt oluşturulurken hata oluştu: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " | Inner Exception: " + ex.InnerException.Message;
                }

                ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Projects.LanguageID);
                ViewBag.ErrorMessage = errorMessage;
                return View(tbl_Projects);
            }
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
