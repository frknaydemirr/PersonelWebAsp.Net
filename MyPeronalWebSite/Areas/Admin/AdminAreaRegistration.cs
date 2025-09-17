using System.Web.Mvc;

namespace MyPeronalWebSite.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            // "/Admin" yazıldığında "/Admin/Admin/Index"e yönlendirme
            context.MapRoute(
                "Admin_Home",
                "Admin",
                new { controller = "Admin", action = "Index" }
            );

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}