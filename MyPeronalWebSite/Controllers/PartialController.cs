using MyPeronalWebSite.Models.ViewModel;
using MyPeronalWebSite.Models.VT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPeronalWebSite.Controllers
{
    public class PartialController : Controller
    {

        PersonelWebDbEntities db = new PersonelWebDbEntities();
        // GET: Partial


        public PartialViewResult NavbarPartial()
        {

            var dil = Request.Cookies["lang"]?.Value ?? "tr";
            int langId = db.Tbl_Language.FirstOrDefault(x => x.ShortTitle == dil)?.ID ?? 1;
            NavbarViewModel vm = new NavbarViewModel();
            vm.Tbl_Resource = db.Tbl_Resource.Where(x => x.LanguageID == langId).ToList();
         
            vm.Tbl_AboutMe = db.Tbl_AboutMe.FirstOrDefault(x => x.LanguageID == langId);
            vm.Tbl_Technologies = db.Tbl_Technologies.Where(x => x.LanguageID == langId).ToList();
            vm.Tbl_Contact = db.Tbl_Contact.Where(x => x.LanguageID == langId).ToList();
            vm.Tbl_Projects = db.Tbl_Projects.Where(x => x.LanguageID == langId).ToList();
            return PartialView(vm);
        }



    }
}