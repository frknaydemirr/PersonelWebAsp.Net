using MyPeronalWebSite.Models.VT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPeronalWebSite.Models.ViewModel
{
    public class ProjectDetailViewModel
    {


        public Tbl_Projects Project { get; set; }
        public List<Tbl_Resource> Resources { get; set; }
        public List<Tbl_Navbar> Navbar { get; set; }
        public Tbl_AboutMe Tbl_AboutMe { get; set; }


    }
}