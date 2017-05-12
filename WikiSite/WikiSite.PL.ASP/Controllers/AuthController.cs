using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.Security;
using log4net;
using WikiSite.PL.ASP.Classes;
using WikiSite.PL.ASP.Models;

namespace WikiSite.PL.ASP.Controllers
{
    public class AuthController : Controller
    {
	    private static EmailAddressAttribute _emailChecker = new EmailAddressAttribute();
	    public static ILog Log = LogManager.GetLogger(typeof(AuthController));

		// GET: Login
		[AllowAnonymousOnly]
		public ActionResult Login()
        {
            return View();
        }

		[HttpPost][ValidateAntiForgeryToken]
		[AllowAnonymousOnly]
		public ActionResult Login(AuthVM model)
	    {
		    if (ModelState.IsValid)
		    {
			    var user = UserVM.GetUser(model);
			    if (user != null)
			    {
					// also gives him auth-cookies
				    FormsAuthentication.RedirectFromLoginPage(user.Id.ToString(), !model.DontRememberMe);
			    }
			    this.Alert("Не удалось войти, проверьте данные и попробуйте еще раз.", AlertType.Danger);
			}
			this.Alert("Данные заполнены неверно.", AlertType.Danger);
			return View(model);
	    }


		[AllowAnonymousOnly]
		public ActionResult Register()
	    {
		    return View();
	    }
		[HttpPost][ValidateAntiForgeryToken]
		[AllowAnonymousOnly]
		public ActionResult Register(RegisterVM model)
		{
			if (ModelState.IsValid)
			{
				if (UserVM.AddUser(model, model.GetCredentialsVM()))
				{
					FormsAuthentication.RedirectFromLoginPage(model.Id.ToString(), true);
				}
				this.Alert("Произошла ошибка при регистрации.", AlertType.Danger);
			}
			else
			{
				this.Alert("Не вся модель заполнена верно. Проверьте поля и попробуйте снова.", AlertType.Warning);
			}
			return View(model);
		}

		[Authorize]
	    public ActionResult Logout()
	    {
		    FormsAuthentication.SignOut();
			return RedirectBack();
	    }


		[AllowAnonymous]
		public JsonResult IsLoginExist(string login)
		{
			var throwError = !UserCredentialsVM.IsLoginExist(login);

			return Json(throwError, JsonRequestBehavior.AllowGet);
		}
		[AllowAnonymous]
		public JsonResult IsEmailExist(string email)
		{
			var throwError = !UserCredentialsVM.IsEmailExist(email);

			return Json(throwError, JsonRequestBehavior.AllowGet);
		}
		[AllowAnonymous]
		public JsonResult CheckLogin(string login)
		{
			var throwError = false;
			if (login.Contains("@"))
				throwError = !_emailChecker.IsValid(login);

			return Json(throwError, JsonRequestBehavior.AllowGet);
		}


		private ActionResult RedirectBack()
	    {
			if (Request.Params["ReturnUrl"] != null)
				return Redirect(Request.Params["ReturnUrl"]);

			return RedirectToAction("Index", "Home");
		}
    }
}