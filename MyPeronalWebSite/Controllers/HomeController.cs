using MyPeronalWebSite.Models.ViewModel;
using MyPeronalWebSite.Models.VT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPeronalWebSite.Controllers
{
    public class HomeController : Controller
    {
        PersonelWebDbEntities db = new PersonelWebDbEntities();
        public ActionResult Index()
        {
            var dil = Request.Cookies["lang"]?.Value ?? "tr";
            int langId = db.Tbl_Language.FirstOrDefault(x => x.ShortTitle == dil)?.ID ?? 1;
            IndexViewModel vm = new IndexViewModel();
            vm.Tbl_Resource=db.Tbl_Resource.Where(x=>x.LanguageID==langId).ToList();
            vm.Tbl_CurrentProject=db.Tbl_CurrentProject.Where(x => x.LanguageID == langId).ToList();




            return View();
        }


        public ActionResult ProjectDetail()
        {
            return View();
        }
    }
}