using MyPeronalWebSite.Models.VT;
using System.Collections.Generic;

namespace MyPeronalWebSite.Models.ViewModel
{
    public class IndexViewModel
    {
        public Tbl_AboutMe Tbl_AboutMe { get; set; }

        public List<Tbl_Contact> Tbl_Contact { get; set; }

        public List<Tbl_CurrentProject> Tbl_CurrentProject { get; set; }

        public List<Tbl_Projects> Tbl_Projects { get; set; }
        public List<Tbl_Navbar> Tbl_Navbar { get; set; }

        public List<Tbl_Resource> Tbl_Resource { get; set; }

        public List<Tbl_Technologies> Tbl_Technologies { get; set; }

        public List<Tbl_Skills> Tbl_Skills { get; set; }
        public NavbarViewModel NavbarViewModel { get; internal set; }


    }
}