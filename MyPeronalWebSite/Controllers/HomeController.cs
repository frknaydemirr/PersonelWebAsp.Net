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
        PersonelWebDbEntities Db = new PersonelWebDbEntities();
        public ActionResult Index()
        {
            return View();
        }
    }
}