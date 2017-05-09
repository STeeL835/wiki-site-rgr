using System.Web.Mvc;
using WikiSite.PL.ASP.Models;

namespace WikiSite.PL.ASP.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ArticlesController : Controller
    {
        // GET: Admin/Articles
        public ActionResult Index()
        {
            return View(ArticleVM.GetAllArticles());
        }
    }
}