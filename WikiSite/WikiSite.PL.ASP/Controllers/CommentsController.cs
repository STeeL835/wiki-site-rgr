using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WikiSite.PL.ASP.Classes;
using WikiSite.PL.ASP.Models;

namespace WikiSite.PL.ASP.Controllers
{
    public class CommentsController : Controller
    {
        // GET: /Comments?article=kukuepta
        public ActionResult Index(string article)
        {
			this.CatchAlert();

	        var theArticle = ArticleVM.GetArticle(article);
			var comments = CommentVM.GetCommentsForArticle(theArticle.Id);
	        var articleVersions = ArticleVM.GetAllVersionOfArticle(theArticle.Id);
	        var comboMix = Combine(articleVersions.ToArray(), comments.ToArray());
	        ViewBag.Article = theArticle;

            return View(new CommentsShowAndAdd(new CommentVM(), comboMix));
        }


	    public ActionResult CommentsByGuid(Guid id) // TODO: [Article] Replace with Redirect to Index when url decoding done
	    {
			var theArticle = ArticleVM.GetArticle(id);
			var comments = CommentVM.GetCommentsForArticle(theArticle.Id);
			var articleVersions = ArticleVM.GetAllVersionOfArticle(theArticle.Id);
			var comboMix = Combine(articleVersions.ToArray(), comments.ToArray());
			ViewBag.Article = theArticle;

			return View("Index", new CommentsShowAndAdd(new CommentVM(), comboMix));
		}


		[Authorize]
		[HttpPost][ValidateAntiForgeryToken]
	    public ActionResult Add(CommentsShowAndAdd model)
		{
			if (ModelState.IsValid)
			{
				var comment = new CommentVM(model.ArticleId, Guid.Parse(User.Identity.Name), model.Text);
				if (CommentVM.AddComment(comment))
				{
					this.AlertNextAction("Комментарий добавлен", AlertType.Success);
				}
				else
				{
					this.AlertNextAction("Не удалось добавить комментарий", AlertType.Danger);
				}

				return RedirectToAction("CommentsByGuid", new { id = model.ArticleId });
			}
			if (model.ArticleId == Guid.Empty)
			{
				var error = new ErrorVM(
					header: "Ошибка",
					title: "Id статьи отсутствует",
					message: "При отправке комментария, наши эльфы забыли упаковать вместе с ним " +
					         "указание на статью, к которой его надо было добавить, а то и сам комментарий(" +
					         "Но мы их накажем, обещаем.");

				return View("Error", error);
			}
			this.AlertNextAction("Неправильные вы комментарии пишете, проверьте, что его длина между 1 и 1500 символами", AlertType.Warning);
			return RedirectToAction("CommentsByGuid", new { id = model.ArticleId });
		}

		[Authorize]
	    public ActionResult Edit(Guid id)
	    {
		    return View();
	    }

	    [Authorize]
	    [HttpPost]
	    [ValidateAntiForgeryToken]
	    public ActionResult Edit(Guid articleId, string text)
	    {
		    throw new NotImplementedException();
	    }

	    [Authorize(Roles = "admin")]
	    [Authorize(Roles = "moderator")]
	    public ActionResult Delete(Guid id)
	    {
		    throw new NotImplementedException();
	    }


	    private CommentArticleVersionModel[] Combine(ArticleVM[] versions, CommentVM[] comments)
	    {
		    var blob = new List<CommentArticleVersionModel>(versions.Length + comments.Length);
		    blob.AddRange(versions.Select(vm => new CommentArticleVersionModel(vm)));
		    blob.AddRange(comments.Select(vm => new CommentArticleVersionModel(vm)));
		    blob.Sort((model1, model2) => DateTime.Compare(model1.DateOfCreation, model2.DateOfCreation));

			return blob.ToArray();
	    }
	}
}