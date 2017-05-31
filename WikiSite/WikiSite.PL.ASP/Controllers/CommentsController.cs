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
			this.CatchAlert();

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
			var comment = CommentVM.GetComment(id);
			if (PrivilegesAreLegit(comment))
			{
				TempData["comment"] = comment;
				return View(comment);
			}
			else
				this.AlertNextAction("У вас нет доступа для редактирования этого комментария", AlertType.Danger);
				return RedirectToAction("CommentsByGuid", new { id = comment.ArticleId });
		}

	    [Authorize][HttpPost]
	    [ValidateAntiForgeryToken]
	    public ActionResult Edit(CommentVM model)
	    {
			if (ModelState.IsValid)
			{
				var comment = (CommentVM) TempData["comment"];
				if (PrivilegesAreLegit(comment))
				{
					comment.Text = model.Text;

					if (CommentVM.UpdateComment(comment))
						this.AlertNextAction("Комментарий изменен", AlertType.Success);
					else
						this.AlertNextAction("Не удалось изменить комментарий", AlertType.Danger);
				}
				else
					this.AlertNextAction("У вас нет доступа для редактирования этого комментария", AlertType.Danger);

				return RedirectToAction("CommentsByGuid", new { id = comment.ArticleId });
			}
			this.Alert("Неправильные вы комментарии пишете, проверьте, что его длина между 1 и 1500 символами", AlertType.Warning);
			TempData.Keep("comment"); // Saves the comment for next request
			return View(model);
		}

	    [Authorize]
	    public ActionResult Delete(Guid id)
		{
			var comment = CommentVM.GetComment(id);
			if (PrivilegesAreLegit(comment))
			{
				if (CommentVM.RemoveComment(id))
				{
					this.AlertNextAction("Комментарий удален", AlertType.Warning);
				}
				else
				{
					this.AlertNextAction("Не удалось удалить комментарий", AlertType.Danger);
				}
			}
			else
			{
				this.AlertNextAction("У вас нет доступа для удаления этого комментария", AlertType.Danger);
			}
			return RedirectToAction("CommentsByGuid", new { id = comment.ArticleId });
		}

		private bool PrivilegesAreLegit(CommentVM comment)
		{
			return Guid.Parse(User.Identity.Name) == comment.AuthorId || User.IsInRole("admin") || User.IsInRole("moderator");
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