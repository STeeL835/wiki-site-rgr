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

			if (string.IsNullOrEmpty(query))
			{
				this.Alert("Строка должна быть не пустой", AlertType.Info);
				return View();
			}

			// Users
			var users = UserVM.SearchUsers(query).ToArray();
		        ViewBag.Users = users;

		    // Articles
		    var articles = ArticleVM.SearchArticles(query).ToArray(); 
		    ViewBag.Articles = articles;

			return View();
        }
    }
}