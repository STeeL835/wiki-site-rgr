using System.Web.Mvc;

namespace WikiSite.PL.ASP.Areas.Admin.Controllers
{
	[Authorize (Roles = "Admin")]
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}