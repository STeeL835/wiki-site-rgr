using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using WikiSite.PL.ASP.Classes;
using WikiSite.PL.ASP.Models;

namespace WikiSite.PL.ASP.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ArticlesController : Controller
    {
        // GET: Admin/Articles
        public ActionResult Index()
        {
            this.CatchAlert();
            return View(ArticleVM.GetAllArticles());
        }

        public ActionResult ShowByGuid(Guid articleId, int number = 0)
        {
            return RedirectToAction("Show", "Articles", new { shortUrl = ArticleVM.GetArticle(articleId).ShortUrl, number = number});
        }

        public ActionResult Show(string shortUrl, int number = 0)
        {
            ViewBag.ShortUrl = shortUrl;
            if (number == 0)
            {
                return View(ArticleVM.GetLastVersionOftArticle(ArticleVM.GetArticle(shortUrl).Id));
            }
            return View(ArticleVM.GetVersionOfArticle(ArticleVM.GetArticle(shortUrl).Id, number));
        }

        public ActionResult DetailsByGuid(Guid articleId)
        {
            return RedirectToAction("Details", "Articles", new { shortUrl = ArticleVM.GetArticle(articleId).ShortUrl});
        }

        public ActionResult Details(string shortUrl)
        {
            ViewBag.ShortUrl = shortUrl;
            var article = ArticleVM.GetArticle(shortUrl);
            ViewBag.Title = $"Все версии статьи \"{article.Heading}\"";
            var versions = ArticleVM.GetAllVersionOfArticle(article.Id).Reverse();
            return View(versions);
        }

        public ActionResult DeleteByGuid(Guid articleId)
        {
            return RedirectToAction("Delete", "Articles", new { shortUrl = ArticleVM.GetArticle(articleId).ShortUrl});
        }

        public ActionResult Delete(string shortUrl)
        {
            var article = ArticleVM.GetArticle(shortUrl);
            if (ArticleVM.RemoveArticle(article.Id))
            {
                this.AlertNextAction($"Статья \"{article.Heading} ({article.ShortUrl})\" успешно удалена.", AlertType.Success);
            }
            else
            {
                this.AlertNextAction(
                    $"Произошла ошибка при удалении статьи \"{article.Heading} ({article.ShortUrl})\". Проверьте выполнение вручную.", AlertType.Danger);
            }
            return RedirectToAction("Index","Home", new {area = "Default"});
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new ArticleVM());
        }

        [HttpPost]
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
                this.AlertNextAction(
                    $"Произошла ошибка при добавлении статьи \"{article.Heading} ({article.ShortUrl})\". Проверьте выполнение вручную.", AlertType.Danger);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Update(string shortUrl)
        {
            ViewBag.Title = $"Редактрование статьи \"{ArticleVM.GetArticle(shortUrl).Heading}\"";
            ViewBag.ShortUrl = shortUrl;

            var article = ArticleVM.GetArticle(shortUrl);
            TempData["Id"] = article.Id;
            TempData["AuthorId"] = article.AuthorId;
            TempData["CreationDate"] = article.CreationDate;
            TempData["Heading"] = article.Heading;

            return View(ArticleVM.GetLastVersionOftArticle(ArticleVM.GetArticle(shortUrl).Id));
        }

        [HttpPost]
        public ActionResult Update(ArticleVM version)
        {
            version.Id = (Guid) TempData.Peek("Id");
            version.AuthorId = (Guid) TempData.Peek("AuthorId");
            version.EditionAuthorId = Guid.Parse(User.Identity.Name);
            version.CreationDate = (DateTime) TempData.Peek("CreationDate");
            version.LastEditDate = DateTime.Now;
            version.IsApproved = false; if (ArticleVM.UpdateArticle(version))
            {
                this.AlertNextAction($"Статья \"{(string) TempData.Peek("Heading")} ({version.ShortUrl})\" успешно изменена.", AlertType.Success);
            }
            else
            {
                this.AlertNextAction(
                    $"Произошла ошибка при изменении статьи \"{(string)TempData.Peek("Heading")} ({version.ShortUrl})\". Проверьте выполнение вручную.", AlertType.Danger);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UpdateByGuid(Guid articleId)
        {
            return RedirectToAction("Update", "Articles", new { shortUrl = ArticleVM.GetArticle(articleId).ShortUrl});
        }

        [HttpGet]
        public ActionResult AddImage()
        {
            return View();
        }

        public ActionResult AddImage(HttpPostedFileBase upload)
        {
            if (upload != null)
            {
                //получаем имя файла полностью с расширением
                string filename = System.IO.Path.GetFileName(upload.FileName);
                //сохраняем изображение в папку Files, которую незабываем создать в проекте
                upload.SaveAs(Server.MapPath("~/Content/Images/Uploaded/" + filename));
                var drt = upload.ContentType;
            }
            return RedirectToAction("Index", "Articles");
        }
    }
}