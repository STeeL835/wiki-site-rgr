using System;
using System.Web.Mvc;
using WikiSite.PL.ASP.Models;

namespace WikiSite.PL.ASP.Controllers
{
    public class CommentsController : Controller
    {
        // GET: /Comments?article=kukuepta
        public ActionResult Index(Guid article)
        {
	        var comments = CommentVM.GetCommentsForArticle(ArticleVM.GetArticle(article).Id);

            return View(comments);
        }

		[Authorize]
		[HttpPost][ValidateAntiForgeryToken]
	    public ActionResult Add(Guid articleId, string text)
	    {
		    var comment = new CommentVM(articleId, Guid.Parse(User.Identity.Name), text);
		    CommentVM.AddComment(comment);

		    return RedirectToAction("Index", new {article = articleId});
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
	}
}