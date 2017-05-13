using System.Collections.Generic;
using System.Web.Mvc;
using System.Globalization;
using System.Linq;
using WikiSite.PL.ASP.Classes;
using WikiSite.PL.ASP.Models;

namespace WikiSite.PL.ASP.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ArticlesController : Controller
    {
        private bool AddPropertiesToViewBag()
        {
            ViewBag.Globalization = CultureInfo.CurrentCulture;
            ViewBag.Users = UserVM.GetAllUsers().ToDictionary(user => user.Id, user => UserVM.GetUser(user.Id));
            return true;
        }
        // GET: Admin/Articles
        public ActionResult Index()
        {
            this.CatchAlert();
            return View(ArticleVM.GetAllArticles());
        }

        public ActionResult Show(string shortUrl, int number = 0)
        {
            AddPropertiesToViewBag();
            ViewBag.ShortUrl = shortUrl;
            if (number == 0)
            {
                return View(ArticleVM.GetLastVersionOftArticle(ArticleVM.GetArticle(shortUrl).Id));
            }
            return View(ArticleVM.GetVersionOfArticle(ArticleVM.GetArticle(shortUrl).Id, number));
        }

        public ActionResult Details(string shortUrl)
        {
            AddPropertiesToViewBag();
            ViewBag.ShortUrl = shortUrl;
            var article = ArticleVM.GetArticle(shortUrl);
            ViewBag.Title = $"Все версии статьи \"{article.Heading}\"";
            var versions = ArticleVM.GetAllVersionOfArticle(article.Id).Reverse();
            return View(versions);
        }
        public ActionResult Delete(string shortUrl)
        {
            var article = ArticleVM.GetArticle(shortUrl);
            if (ArticleVM.RemoveArticle(article.Id))
            {
                this.AlertNextAction($"Статья {article.Heading}({article.ShortUrl}) успешно удалена.", AlertType.Success);
            }
            else
            {
                this.AlertNextAction($"Произошла ошибка при удалении статьи {article.Heading}({article.ShortUrl}). Проверьте выполнение вручную.", AlertType.Danger);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Update(string shortUrl)
        {
            ViewBag.Title = $"Редактрование статьи \"{ArticleVM.GetArticle(shortUrl).Heading}\"";
            ViewBag.ShortUrl = shortUrl;
            return View();
        }
    }
}