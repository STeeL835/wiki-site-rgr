using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WikiSite.DAL.Abstract;
using WikiSite.PL.ASP.Classes;
using WikiSite.PL.ASP.Models;

namespace WikiSite.PL.ASP.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Article

        public ActionResult ShowByGuid(Guid guid, int number = 0)
        {
            try
            {
                return RedirectToAction("Show", "Article", new {url = ArticleVM.GetArticle(guid).ShortUrl, number = number});
            }
            catch (EntryNotFoundException e)
            {
                var error = new ErrorVM(
                    header: "404",
                    title: "Статья не найдена",
                    message: $"Статья с Guid {guid} не найдена. " +
                             $"Проверьте url в адресной строке или, если это не ваша вина, " +
                             $"обратитесь к администратору.",
                    exceptionDetails: e.ToString()
                );
                return View("Error", error);
            }
        }
        
        public ActionResult Show(string url, int number = 0)
        {
            this.CatchAlert();
            ArticleVM article;
            try
            {
                ViewBag.ShortUrl = url;
                ViewBag.Number = number;
                if (number == 0)
                {
                    article = ArticleVM.GetLastApprovedVersionOfArticle(ArticleVM.GetArticle(url).Id);
                }
                else article = ArticleVM.GetVersionOfArticle(ArticleVM.GetArticle(url).Id, number);
            }
            catch (EntryNotFoundException e)
            {
                var error = new ErrorVM(
                    header: "404",
                    title: "Статья не найдена",
                    message: $"Статья с URL {url} не найдена. " +
                             $"Проверьте url в адресной строке или, если это не ваша вина, " +
                             $"обратитесь к администратору.",
                    exceptionDetails: e.ToString()
                );
                return View("Error", error);
            }
            return View(article);
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
            ViewBag.Title = $"Редактирование статьи \"{ArticleVM.GetArticle(url).Heading}\"";
            ViewBag.ShortUrl = url;

            var article = ArticleVM.GetArticle(url);
            TempData["Id"] = article.Id;
            TempData["AuthorId"] = article.AuthorId;
            TempData["CreationDate"] = article.CreationDate;
            TempData["Heading"] = article.Heading;
            TempData["ImageId"] = article.ImageId;

            return View(ArticleVM.GetLastApprovedVersionOfArticle(ArticleVM.GetArticle(url).Id));
        }

        [HttpPost][Authorize]
        public ActionResult Update(HttpPostedFileBase file, ArticleVM version, bool isApproved = true)
        {
            version.Id = (Guid)TempData.Peek("Id");
            version.AuthorId = (Guid)TempData.Peek("AuthorId");
            version.EditionAuthorId = Guid.Parse(User.Identity.Name);
            version.CreationDate = (DateTime)TempData.Peek("CreationDate");
            version.LastEditDate = DateTime.Now;
            version.IsApproved = isApproved;

            if (file != null)
                version.ImageId = ImageController.Add(file);
            else
                version.ImageId = (Guid)TempData.Peek("ImageId");

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

        public ActionResult Approve(Guid id, bool type = true)
        {
            var article = ArticleVM.GetArticle(ArticleVM.GetVersionOfArticle(id).Id);
            if (ArticleVM.ApproveVersionOfArticle(id, type))
            {
                this.AlertNextAction($"Подтверждённость версии \"{ArticleVM.GetNumberOfVersion(id)}\" для статьи \"{article.Heading} ({article.ShortUrl})\" успешно изменена.", AlertType.Success);
            }
            else
            {
                this.AlertNextAction(
                    $"Произошла ошибка при изменении подтверждённости версии \"{ArticleVM.GetNumberOfVersion(id)}\" для статьи \"{article.Heading} ({article.ShortUrl})\". Проверьте выполнение вручную.", AlertType.Danger);
            }
            return RedirectToAction("ShowByGuid", "Article", new { guid = article.Id });
        }

        public JsonResult IsHeadingExist(string heading)
        {
            var throwError = ArticleVM.IsShortUrlExist(HttpUtility.UrlEncode(heading.ToLower().Trim()));

            return Json(throwError, JsonRequestBehavior.AllowGet);
        }
    }
}