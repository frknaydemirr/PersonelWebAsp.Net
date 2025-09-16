using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using MyPeronalWebSite.Models.VT;

namespace MyPeronalWebSite.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private PersonelWebDbEntities db = new PersonelWebDbEntities();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Tbl_User model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var hashedPassword = HashPassword(model.Password);

            var user = db.Tbl_User.FirstOrDefault(u => u.UserName == model.UserName && u.Password == hashedPassword);

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.UserName, false);

                Session["Kullanici"] = user;
                Session["KullaniciAdi"] = user.UserName;

                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }

            ViewBag.Error = "Geçersiz kullanıcı adı veya şifre!";
            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        // SHA256 hash metodu
        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hashBytes = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        // Yeni: Tüm mevcut Passwordları hash'ler ve kaydeder
        public ActionResult HashExistingPasswords()
        {
            var users = db.Tbl_User.ToList();
            int updatedCount = 0;

            foreach (var user in users)
            {
                // Null veya boş Passwordları atla
                if (string.IsNullOrEmpty(user.Password))
                    continue;

                // Eğer Password 64 karakter değilse VEYA hexadecimal değilse, yeniden hash'le
                if (user.Password.Length != 64 || !IsValidSHA256(user.Password))
                {
                    user.Password = HashPassword(user.Password);
                    updatedCount++;
                }
            }

            db.SaveChanges();
            return Content($"{updatedCount} Password güncellendi.");
        }

        // SHA256 hash kontrolü (case-insensitive + hex format)
        private bool IsValidSHA256(string input)
        {
            if (string.IsNullOrEmpty(input) || input.Length != 64)
                return false;

            // Regex ile hexadecimal kontrol (büyük/küçük harf duyarsız)
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"^[0-9a-fA-F]{64}$");
        }

    }
}