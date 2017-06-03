using System;
using System.Collections.Generic;
using System.IO;
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

        public ActionResult ShowByGuid(Guid guid, int number = 0)
        {
            return RedirectToAction("Show", "Article", new { url = ArticleVM.GetArticle(guid).ShortUrl, number = number });
        }
        
        public ActionResult Show(string url, int number = 0)
        {
            ViewBag.ShortUrl = url;
            ViewBag.Number = number;
            if (number == 0)
            {
                return View(ArticleVM.GetLastApprovedVersionOfArticle(ArticleVM.GetArticle(url).Id));
            }
            return View(ArticleVM.GetVersionOfArticle(ArticleVM.GetArticle(url).Id, number));
        }

        [HttpGet][Authorize]
        public ActionResult Create()
        {
            return View(new ArticleVM());
        }

        [HttpPost][Authorize]
        public ActionResult Create(HttpPostedFileBase file, ArticleVM article)
        {
            article.ImageId = ImageController.Add(file);
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
            return RedirectToAction("ShowByGuid", "Article", new { guid = article.Id });
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

            return View(ArticleVM.GetLastApprovedVersionOfArticle(ArticleVM.GetArticle(url).Id));
        }

        [HttpPost][Authorize]
        public ActionResult Update(HttpPostedFileBase file, ArticleVM version, bool isApproved = true)
        {
            version.ImageId = ImageController.Add(file);
            version.Id = (Guid)TempData.Peek("Id");
            version.AuthorId = (Guid)TempData.Peek("AuthorId");
            version.EditionAuthorId = Guid.Parse(User.Identity.Name);
            version.CreationDate = (DateTime)TempData.Peek("CreationDate");
            version.LastEditDate = DateTime.Now;
            version.IsApproved = isApproved;
            if (ArticleVM.UpdateArticle(version))
            {
                this.AlertNextAction($"Статья \"{(string)TempData.Peek("Heading")} ({version.ShortUrl})\" успешно изменена.", AlertType.Success);
            }
            else
            {
                this.AlertNextAction(
                    $"Произошла ошибка при изменении статьи \"{(string)TempData.Peek("Heading")} ({version.ShortUrl})\". Проверьте выполнение вручную.", AlertType.Danger);
            }
            return RedirectToAction("ShowByGuid", "Article", new { guid = version.Id });
        }

        [HttpGet][Authorize]
        public ActionResult UpdateByGuid(Guid guid)
        {
            return RedirectToAction("Update", "Article", new { url = ArticleVM.GetArticle(guid).ShortUrl });
        }

        public ActionResult DetailsByGuid(Guid guid)
        {
            return RedirectToAction("Details", "Article", new { url = ArticleVM.GetArticle(guid).ShortUrl });
        }

        public ActionResult Details(string url)
        {
			this.CatchAlert();
            ViewBag.ShortUrl = url;
            var article = ArticleVM.GetArticle(url);
            ViewBag.Title = $"Все версии статьи \"{article.Heading}\"";
            var versions = ArticleVM.GetAllVersionOfArticle(article.Id).Reverse().ToArray();
            return View(versions);
        }

        public JsonResult IsHeadingExist(string heading)
        {
            var throwError = ArticleVM.IsShortUrlExist(HttpUtility.UrlEncode(heading.ToLower().Trim()));

            return Json(throwError, JsonRequestBehavior.AllowGet);
        }
    }
}