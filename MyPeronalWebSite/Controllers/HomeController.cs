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
            vm.Tbl_AboutMe = db.Tbl_AboutMe.FirstOrDefault(x => x.LanguageID == langId);
            vm.Tbl_Technologies = db.Tbl_Technologies.Where(x => x.LanguageID == langId).ToList();
            vm.Tbl_Contact = db.Tbl_Contact.Where(x => x.LanguageID == langId).ToList();
            vm.Tbl_Projects = db.Tbl_Projects.Where(x => x.LanguageID == langId).ToList();
            vm.Tbl_Skills = db.Tbl_Skills.Where(x => x.LanguageID == langId).ToList();

            return View(vm);
        }


        [Route("Project/{newTitle}-{id:int}")]
        public ActionResult ProjectDetail(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

            var dil = Request.Cookies["lang"]?.Value ?? "tr";
            int langId = db.Tbl_Language.FirstOrDefault(x => x.ShortTitle == dil)?.ID ?? 1;

            var project = db.Tbl_Projects.FirstOrDefault(x => x.ID == id && x.LanguageID == langId);

            if (project == null)
            {
                return HttpNotFound();
            }

            ProjectDetailViewModel vm = new ProjectDetailViewModel
            {
                Project = project,
                Resources = db.Tbl_Resource.Where(x => x.LanguageID == langId && x.Page == "ProjectDetail").ToList(),
                //Navbar = db.Tbl_Navbar.Where(x => x.LanguageID == langId).ToList()
                // ihtiyacına göre slider, firma bilgileri vb. ekleyebilirsin
            };

          

            return View(vm);
        }


    }
}
