using MyPeronalWebSite.Models.ViewModel;
using MyPeronalWebSite.Models.VT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyPeronalWebSite.Controllers
{
    public class HomeController : Controller
    {
        PersonelWebDbEntities db = new PersonelWebDbEntities();

        [Route("")]
        [Route("Anasayfa")]
        [Route("Home")]
        public ActionResult Index()
        {
            var dil = (Request.Cookies["lang"]?.Value ?? "tr").ToLower(); // Cookie'den dil al, yoksa 'tr'

            int langId = db.Tbl_Language.FirstOrDefault(x => x.ShortTitle.ToLower() == dil)?.ID ?? 1;

            ViewBag.DilId = langId;


            IndexViewModel vm = new IndexViewModel();
            vm.Tbl_Resource = db.Tbl_Resource.Where(x => x.LanguageID == langId).ToList();
            vm.Tbl_AboutMe = db.Tbl_AboutMe.FirstOrDefault(x => x.LanguageID == langId);
            vm.Tbl_Technologies = db.Tbl_Technologies.Where(x => x.LanguageID == langId).ToList();
            vm.Tbl_Contact = db.Tbl_Contact.Where(x => x.LanguageID == langId).ToList();
            vm.Tbl_Projects = db.Tbl_Projects.Where(x => x.LanguageID == langId).ToList();
            vm.Tbl_Skills = db.Tbl_Skills.Where(x => x.LanguageID == langId).ToList();
            vm.Tbl_Navbar = db.Tbl_Navbar.Where(x => x.LanguageID == langId).ToList();
            MetaBilgiler(langId);
            return View(vm);
        }

        public ActionResult Blog()
        {
            var dil = Request.Cookies["lang"]?.Value ?? "tr";
            int dilId = DilId(dil);
            ViewBag.DilId = dilId;

            int langId = db.Tbl_Language.FirstOrDefault(x => x.ShortTitle == dil)?.ID ?? 1;
            var navbarItems = db.Tbl_Navbar
            .Where(x => x.Tbl_Language.ShortTitle.ToLower() == dil.ToLower())
            .ToList();
            var Blog = db.Tbl_Blog.Where(x => x.LanguageID == langId).ToList();
            BlogViewModel vm = new BlogViewModel
            {
                Tbl_Blog = Blog,
                Resources = db.Tbl_Resource.Where(x => x.LanguageID == langId && x.Page == "ProjectDetail").ToList(),
                Tbl_Navbar = navbarItems


            };
            MetaBilgiler(langId);
            return View(vm);
        }


        public ActionResult BlogDetail()
        {
            var dil = Request.Cookies["lang"]?.Value ?? "tr";
            int dilId = DilId(dil);
            ViewBag.DilId = dilId;

            int langId = db.Tbl_Language.FirstOrDefault(x => x.ShortTitle == dil)?.ID ?? 1;
            var navbarItems = db.Tbl_Navbar
            .Where(x => x.Tbl_Language.ShortTitle.ToLower() == dil.ToLower())
            .ToList();
            var Blog = db.Tbl_Blog.Where(x => x.LanguageID == langId).ToList();
            BlogViewModel vm = new BlogViewModel
            {
                Tbl_Blog = Blog,
                Resources = db.Tbl_Resource.Where(x => x.LanguageID == langId && x.Page == "ProjectDetail").ToList(),
                Tbl_Navbar = navbarItems


            };
            MetaBilgiler(langId);
            return View(vm);
        }




        private void MetaBilgiler(int langId)
        {
            string url = Request.Url.AbsolutePath.ToLower(); // Örneğin "/anasayfa", "/project-details/some-title-3"

            // Varsayılan meta değerler (sayfa bulunamazsa gösterilecek)
            string defaultTitle = "Varsayılan Başlık";
            string defaultKeyword = "Varsayılan Keyword";
            string defaultDescription = "Varsayılan Açıklama";

            Tbl_Navbar navbar = null;

            if (url == "/" || url == "/anasayfa" || url == "/home")
            {
                navbar = db.Tbl_Navbar.FirstOrDefault(x =>
                    x.LanguageID == langId &&
                    x.Turn == true &&  // Turn bool değil int ise 1 olarak kontrol
                    (x.URL.ToLower() == "/anasayfa" || x.URL.ToLower() == "/home" || x.URL.ToLower() == "/"));
            }
            else
            {
                navbar = db.Tbl_Navbar.FirstOrDefault(x =>
                    x.LanguageID == langId &&
                    x.Turn == true &&
                    (x.URL.ToLower() == url || (x.SeoUrl != null && x.SeoUrl.ToLower() == url))
                );
            }

            if (navbar != null)
            {
                ViewBag.Title = navbar.MetaTitle ?? defaultTitle;
                ViewBag.Keyword = navbar.MetaKeyword ?? defaultKeyword;
                ViewBag.Description = navbar.MetaDescription ?? defaultDescription;
            }
            else
            {
                ViewBag.Title = defaultTitle;
                ViewBag.Keyword = defaultKeyword;
                ViewBag.Description = defaultDescription;
            }
        }




        [Route("project-details/{title}-{id:int}")]
        public ActionResult ProjectDetail(string title, int id)
        {
            var dil = Request.Cookies["lang"]?.Value ?? "tr";
            int dilId = DilId(dil);
            ViewBag.DilId = dilId;

            int langId = db.Tbl_Language.FirstOrDefault(x => x.ShortTitle == dil)?.ID ?? 1;
            var navbarItems = db.Tbl_Navbar
    .Where(x => x.Tbl_Language.ShortTitle.ToLower() == dil.ToLower())
    .ToList();
            var project = db.Tbl_Projects.FirstOrDefault(x => x.ID == id && x.LanguageID == langId);
            if (project == null)
            {
                return HttpNotFound();
            }
           

            ProjectDetailViewModel vm = new ProjectDetailViewModel
            {
                Project = project,
                Resources = db.Tbl_Resource.Where(x => x.LanguageID == langId && x.Page == "ProjectDetail").ToList(),
                Navbar = navbarItems


            };
            MetaBilgiler(langId);
            return View(vm);
        }


      




        [HttpPost]
        public JsonResult SendMessage(string name, string message, string phoneNumber, string email, string subject, int langId)
        {
            try
            {
                // Zorunlu alanları kontrol et
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(message))
                {
                    return Json(new { success = false, message = "Required fields cannot be empty." });
                }

                // Yeni kayıt ekleme
                db.Tbl_Contact.Add(new Tbl_Contact
                {
                    Name = name,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Message = message,
                    Subcejt = subject,
                    LanguageID = langId
                });

                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Dil_Degistir(string dil)
        {
            if (!string.IsNullOrEmpty(dil))
            {
                //yeni cookie oluşturuluyor
                var cookie = new HttpCookie("lang", dil)
                {
                    Expires = DateTime.Now.AddYears(1) //1 yıl tarayıca kalacak
                };

                Response.Cookies.Add(cookie);
            }
            // Yönlendirme işlemi sadece burada yapılacak
            return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
        }


        public int DilId(string Dil)
        {
            if (string.IsNullOrEmpty(Dil))
                Dil = "TR"; // Varsayılan dil kodu

            var dilKaydi = db.Tbl_Language.FirstOrDefault(x => x.ShortTitle.ToLower() == Dil.ToLower());

            if (dilKaydi == null)
                return 1; // Varsayılan dil ID'si

            return dilKaydi.ID;
        }




    }







}