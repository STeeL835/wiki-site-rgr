using System.Web.Mvc;
using WikiSite.PL.ASP.Models;

namespace WikiSite.PL.ASP.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        // GET: Admin/Users
        public ActionResult Index()
        {
            return View(UserVM.GetAllUsers());
        }


	    public ActionResult CreateUser()
	    {
		    return View();
	    }

		[HttpPost][ValidateAntiForgeryToken]
	    public ActionResult CreateUser(SignupModel model)
	    {
		    if (ModelState.IsValid)
		    {
			    if (UserVM.AddUser(model.GetUserVM(), model.GetCredentialsVM()))
			    {
				    ViewBag.AlertMessage = $"Пользователь {model.Nickname} успешно добавлен"; // is is vulnerable for XSS?
				    ViewBag.AlertClass = "alert-success";
			    }
			    else
			    {
					ViewBag.AlertMessage = $"Произошла ошибка при добавлении пользователя {model.Nickname}." +
					                       $"Пользователь не был добавлен"; // is is vulnerable for XSS?
					ViewBag.AlertClass = "alert-danger";
				}
		    }
		    return View(model);
	    }


	    public ActionResult CheckCredentials()
	    {
		    return View();
	    }

		[HttpPost][ValidateAntiForgeryToken]
	    public ActionResult CheckCredentials(CredentialsUserModel model)
	    {
		    model.User = UserVM.GetCheckCredentials(model.GetCredentials());
			return View(model);
	    }


	    public JsonResult IsLoginExist(string login)
	    {
		    var throwError = !UserCredentialsVM.IsLoginExist(login);

			return Json(throwError, JsonRequestBehavior.AllowGet);
	    }
    }
}