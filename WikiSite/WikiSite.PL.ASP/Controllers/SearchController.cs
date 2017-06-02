using System.Linq;
using System.Web.Mvc;
using WikiSite.PL.ASP.Classes;
using WikiSite.PL.ASP.Models;

namespace WikiSite.PL.ASP.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index(string query)
        {
		    ViewBag.SearchQuery = query;

			if (query == null || query.Length < 3)
			{
				this.Alert("Длина строки должна быть больше 2 символов", AlertType.Info);
				return View();
			}

			// Users
			var users = UserVM.SearchUsers(query).ToArray();
		        ViewBag.Users = users;

		    // Articles
		    var articles = ArticleVM.SearchArticles(query).ToArray(); // TODO: [Articles][Search] Implement when done
		    ViewBag.Articles = articles;

			return View();
        }
    }
}