using MyPeronalWebSite.Models.VT;
using System.Collections.Generic;

namespace MyPeronalWebSite.Models.ViewModel
{
    public class BlogViewModel
    {
        public Tbl_AboutMe Tbl_AboutMe { get; set; }
        public List<Tbl_Contact> Tbl_Contact { get; set; }
        public List<Tbl_Navbar> Tbl_Navbar { get; set; }
        public List<Tbl_Resource> Resources { get; set; }

        // Blog listesi için
        public List<Tbl_Blog> Tbl_Blog { get; set; }

        // Blog detayı için
        public Tbl_Blog Blog { get; set; }

        // Diğer bloglar için (isteğe bağlı)
        public List<Tbl_Blog> AllBlogs { get; set; }

        public NavbarViewModel NavbarViewModel { get; internal set; }


    }
}