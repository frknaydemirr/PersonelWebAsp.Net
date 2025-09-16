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
    [Authorize]
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
        // GET: Admin/Admin_Project/Create
        // GET: Admin/Admin_Project/Create
        public ActionResult Create()
        {
            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title");
            return View();
        }

        // POST: Admin/Admin_Project/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create( // İSİM Create OLMALI
            [Bind(Include = "ID,LanguageID,ProjectTitle,ProjectImg,ProjectDescription,ProjectEntryDescription,ProjectEntryTitle,GithubURL,isActive")]
    Tbl_Projects tbl_Projects,
            HttpPostedFileBase ProjectImg = null) // Parametre ismi View'daki file input name ile aynı olmalı
        {
            try
            {
                // Model validasyonu
                if (!ModelState.IsValid)
                {
                    ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Projects.LanguageID);
                    ViewBag.ErrorMessage = "Form verileri geçersiz";
                    return View(tbl_Projects);
                }

                // Resim yükleme kontrolü
                if (ProjectImg == null || ProjectImg.ContentLength == 0)
                {
                    ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Projects.LanguageID);
                    ViewBag.ErrorMessage = "Lütfen bir proje resmi seçiniz";
                    return View(tbl_Projects);
                }

                // Resim format kontrolü
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var extension = Path.GetExtension(ProjectImg.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Projects.LanguageID);
                    ViewBag.ErrorMessage = "Sadece JPG, JPEG, PNG, GIF veya WEBP formatında resimler yükleyebilirsiniz";
                    return View(tbl_Projects);
                }

                // Resim boyutu kontrolü (5MB)
                if (ProjectImg.ContentLength > 5 * 1024 * 1024)
                {
                    ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", tbl_Projects.LanguageID);
                    ViewBag.ErrorMessage = "Resim boyutu 5MB'tan büyük olamaz";
                    return View(tbl_Projects);
                }

                // Klasör işlemleri
                var uploadPath = Server.MapPath("~/Uploads/Projects");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Resmi kaydet
                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(uploadPath, fileName);
                ProjectImg.SaveAs(filePath);
                tbl_Projects.ProjectImg = "/Uploads/Projects/" + fileName;

                // Veritabanına kaydet
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
        // GET: Admin/Admin_Project/Edit/5
        // GET: Admin/Admin_Project/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Tbl_Projects project = db.Tbl_Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", project.LanguageID);
            return View(project);
        }

        // POST: Admin/Admin_Project/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Tbl_Projects model, HttpPostedFileBase ProjectImg = null)
        {
            if (model == null)
            {
                return HttpNotFound();
            }

            try
            {
                // ModelState'teki resim alanını temizle (gereksiz hatalar için)
                ModelState.Remove("ProjectImg");

                if (!ModelState.IsValid)
                {
                    ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", model.LanguageID);
                    return View(model);
                }

                var existingProject = db.Tbl_Projects.Find(model.ID);
                if (existingProject == null)
                {
                    return HttpNotFound();
                }

                // HTML temizleme (varsa)
                existingProject.ProjectDescription = StripHtmlTags(model.ProjectDescription);
                existingProject.ProjectEntryDescription = StripHtmlTags(model.ProjectEntryDescription);

                // Alan güncellemeleri
                existingProject.ProjectTitle = model.ProjectTitle;
                existingProject.ProjectEntryTitle = model.ProjectEntryTitle;
                existingProject.LanguageID = model.LanguageID;
                existingProject.GithubURL = model.GithubURL;
                existingProject.isActive = model.isActive ?? false; // nullable bool kontrolü

                // Resim güncelleme
                if (ProjectImg != null && ProjectImg.ContentLength > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                    var extension = Path.GetExtension(ProjectImg.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ViewBag.ErrorMessage = "Sadece JPG, JPEG, PNG, GIF veya WEBP formatında resimler yükleyebilirsiniz.";
                        ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", model.LanguageID);
                        return View(model);
                    }

                    if (ProjectImg.ContentLength > 5 * 1024 * 1024)
                    {
                        ViewBag.ErrorMessage = "Resim boyutu 5MB'tan büyük olamaz.";
                        ViewBag.LanguageID = new SelectList(db.Tbl_Language, "ID", "Title", model.LanguageID);
                        return View(model);
                    }

                    var uploadPath = Server.MapPath("~/Uploads/Projects");
                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    var fileName = Guid.NewGuid() + extension;
                    var filePath = Path.Combine(uploadPath, fileName);
                    ProjectImg.SaveAs(filePath);

                    // Eski resmi sil
                    if (!string.IsNullOrEmpty(existingProject.ProjectImg))
                    {
                        var oldFilePath = Server.MapPath(existingProject.ProjectImg);
                        if (System.IO.File.Exists(oldFilePath))
                            System.IO.File.Delete(oldFilePath);
                    }

                    existingProject.ProjectImg = "/Uploads/Projects/" + fileName;
                }

                db.Entry(existingProject).State = EntityState.Modified;
                db.SaveChanges();

                TempData["SuccessMessage"] = "Proje başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.DilId = new SelectList(db.Tbl_Language, "ID", "Title", model.LanguageID);
                ViewBag.ErrorMessage = "Güncelleme sırasında bir hata oluştu: " + ex.Message;
                return View(model);
            }
        }



        // HTML tag'larını temizleyen yardımcı metod
        private string StripHtmlTags(string html)
        {
            if (string.IsNullOrEmpty(html))
                return html;

            return System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", string.Empty);
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
