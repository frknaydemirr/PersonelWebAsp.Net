using MyPeronalWebSite.Models.VT;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;

namespace MyPeronalWebSite.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private PersonelWebDbEntities db = new PersonelWebDbEntities();

        // GET: Login
        public ActionResult Login()
        {
            return View(); 
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Tbl_User tbl_User)
        {
            if (!ModelState.IsValid)
                return View(tbl_User);

            var hashedPassword = HashPassword(tbl_User.Password);
            var user = db.Tbl_User.FirstOrDefault(u => u.UserName == tbl_User.UserName && u.Password == hashedPassword);




            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.UserName, false);
                Session["Kullanici"] = user;
                Session["KullaniciAdi"] = user.UserName;

                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }

            ViewBag.Error = "Geçersiz kullanıcı adı veya şifre!";
            return View(tbl_User);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
