using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WikiSite.DAL.Abstract;
using WikiSite.PL.ASP.Classes;
using WikiSite.PL.ASP.Models;

namespace WikiSite.PL.ASP.Controllers
{
    public class AVersionsVotesController : Controller
    {
		[Authorize]
        public ActionResult Vote(Guid versionId, bool voteIsLike)
		{
			if (versionId == Guid.Empty)
			{
				var error = new ErrorVM(
					header: "Ошибка",
					title: "Id статьи отсутствует",
					message: "В процессе оставления заметки о вашем голосе, наши эльфы забыли указать  " +
							 "на статью, к которой его надо было добавить(" +
							 "Но мы их накажем, обещаем.");

				return View("Error", error);
			}
			ArticleVM article;
			try
			{
				article = ArticleVM.GetVersionOfArticle(versionId);
			}
			catch (EntryNotFoundException)
			{
				var error = new ErrorVM(
					header: "Ошибка",
					title: "Такой версии статьи отсутствует",
					message: "Пришел запрос на оставление голоса " +
							 (voteIsLike ? "за версию" : "против версии") +
							 "определенной статьи, которой мы не нашли");

				return View("Error", error);
			}
			if (AVersionVoteVM.Vote(new AVersionVoteVM(versionId, Guid.Parse(User.Identity.Name), voteIsLike)))
				this.AlertNextAction("Ваш голос учтён", AlertType.Success);
			else
				this.AlertNextAction("Что-то пошло не так, ваш голос не был учтен", AlertType.Danger);

			return RedirectToAction("Details", "Article", new { url = article.ShortUrl});
		}

	    [Authorize]
		public ActionResult UnVote(Guid versionId)
	    {
			if (versionId == Guid.Empty)
			{
				var error = new ErrorVM(
					header: "Ошибка",
					title: "Id статьи отсутствует",
					message: "В процессе оставления заметки о снятии вашего голоса с учета, наши эльфы забыли указать  " +
							 "на статью, к которой его надо было добавить(" +
							 "Но мы их накажем, обещаем.");

				return View("Error", error);
			}
			ArticleVM article;
			try
			{
				article = ArticleVM.GetVersionOfArticle(versionId);
			}
			catch (EntryNotFoundException)
			{
				var error = new ErrorVM(
					header: "Ошибка",
					title: "Такой версии статьи отсутствует",
					message: "Пришел запрос на удаление голоса " +
							 "с версии определенной статьи, которой мы не нашли");

				return View("Error", error);
			}
			if (AVersionVoteVM.UnVote(new AVersionVoteVM(versionId, Guid.Parse(User.Identity.Name), true)))
			{
				this.AlertNextAction("Ваш голос был убран", AlertType.Success);
			}
			else
			{
				this.AlertNextAction("Что-то пошло не так, не удалось снять ваш голос", AlertType.Danger);
			}

			return RedirectToAction("Details", "Article", new { url = article.ShortUrl });
		}
    }
}