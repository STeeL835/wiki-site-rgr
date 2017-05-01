using System.Web.Mvc;

namespace WikiSite.PL.ASP.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Admin";

	    public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "Admin_default",
                url: "Admin/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
				namespaces: new []{ "WikiSite.PL.ASP.Areas.Admin.Controllers" }
            );
        }
    }
}