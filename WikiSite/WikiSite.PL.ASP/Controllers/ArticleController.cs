using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WikiSite.PL.ASP.Classes;
using WikiSite.PL.ASP.Models;

namespace WikiSite.PL.ASP.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Article
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult ShowByGuid(Guid articleId, int number = 0)
        {
            return RedirectToAction("Show", "Article", new { url = ArticleVM.GetArticle(articleId).ShortUrl, number = number });
        }
        
        public ActionResult Show(string url, int number = 0)
        {
            ViewBag.ShortUrl = url;
            if (number == 0)
            {
                return View(ArticleVM.GetLastVersionOftArticle(ArticleVM.GetArticle(url).Id));
            }
            return View(ArticleVM.GetVersionOfArticle(ArticleVM.GetArticle(url).Id, number));
        }

        [HttpGet][Authorize]
        public ActionResult Create()
        {
            return View(new ArticleVM());
        }

        [HttpPost][Authorize]
        public ActionResult Create(ArticleVM article)
        {
            article.Id = Guid.NewGuid();
            article.AuthorId = Guid.Parse(User.Identity.Name);
            article.EditionAuthorId = article.AuthorId;
            article.CreationDate = DateTime.Now;
            article.LastEditDate = article.CreationDate;
            article.IsApproved = true;
            if (ArticleVM.AddArticle(article))
            {
                this.AlertNextAction($"Статья \"{article.Heading} ({article.ShortUrl})\" успешно добавлена.", AlertType.Success);
            }
            else
            {
                this.AlertNextAction($"Произошла ошибка при добавлении статьи \"{article.Heading} ({article.ShortUrl})\". Проверьте выполнение вручную.", AlertType.Danger);
            }
            return RedirectToAction("Show", "Articles", new { url = ArticleVM.GetArticle(article.Id).ShortUrl });
        }

        [HttpGet][Authorize]
        public ActionResult Update(string url)
        {
            ViewBag.Title = $"Редактрование статьи \"{ArticleVM.GetArticle(url).Heading}\"";
            ViewBag.ShortUrl = url;

            var article = ArticleVM.GetArticle(url);
            TempData["Id"] = article.Id;
            TempData["AuthorId"] = article.AuthorId;
            TempData["CreationDate"] = article.CreationDate;
            TempData["Heading"] = article.Heading;

            return View(ArticleVM.GetLastVersionOftArticle(ArticleVM.GetArticle(url).Id));
        }

        [HttpPost][Authorize]
        public ActionResult Update(ArticleVM version)
        {
            version.Id = (Guid)TempData.Peek("Id");
            version.AuthorId = (Guid)TempData.Peek("AuthorId");
            version.EditionAuthorId = Guid.Parse(User.Identity.Name);
            version.CreationDate = (DateTime)TempData.Peek("CreationDate");
            version.LastEditDate = DateTime.Now;
            version.IsApproved = false;
            if (ArticleVM.UpdateArticle(version))
            {
                this.AlertNextAction($"Статья \"{(string)TempData.Peek("Heading")} ({version.ShortUrl})\" успешно изменена.", AlertType.Success);
            }
            else
            {
                this.AlertNextAction(
                    $"Произошла ошибка при изменении статьи \"{(string)TempData.Peek("Heading")} ({version.ShortUrl})\". Проверьте выполнение вручную.", AlertType.Danger);
            }
            return RedirectToAction("Show", "Articles", new { url = ArticleVM.GetArticle(version.Id).ShortUrl});
        }

        [HttpGet][Authorize]
        public ActionResult UpdateByGuid(Guid articleId)
        {
            return RedirectToAction("Update", "Article", new { url = ArticleVM.GetArticle(articleId).ShortUrl });
        }

        public ActionResult DetailsByGuid(Guid articleId)
        {
            return RedirectToAction("Details", "Article", new { url = ArticleVM.GetArticle(articleId).ShortUrl });
        }

        public ActionResult Details(string url)
        {
            ViewBag.ShortUrl = url;
            var article = ArticleVM.GetArticle(url);
            ViewBag.Title = $"Все версии статьи \"{article.Heading}\"";
            var versions = ArticleVM.GetAllVersionOfArticle(article.Id);
            return View(versions);
        }

        public JsonResult IsHeadingExist(string heading)
        {
            var throwError = ArticleVM.IsShortUrlExist(HttpUtility.UrlEncode(heading.ToLower().Trim()));

            return Json(throwError, JsonRequestBehavior.AllowGet);
        }
    }
}