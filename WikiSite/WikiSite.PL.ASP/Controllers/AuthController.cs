using System;
using System.Collections.Generic;
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

		private readonly TimeSpan LoginAttemptCooldown = TimeSpan.FromMinutes(3);
		private const int LoginAttemptsAllowed = 5;
	    private List<DateTime> LoginAttempts { get; set; } = new List<DateTime>(LoginAttemptsAllowed);


		// GET: Login
		[AllowAnonymousOnly]
		public ActionResult Login()
		{
			CheckLoginAttempts();
            return View();
        }

		[HttpPost][ValidateAntiForgeryToken]
		[AllowAnonymousOnly]
		public ActionResult Login(AuthVM model)
	    {
		    if (ModelState.IsValid)
		    {
			    if (CheckLoginAttempts()) return View(model);

			    if (LoginAttempts.Count > 5)
			    {
				    this.Alert("Вы ввели данные неправильно более 5 раз, подождите 3 минуты, прежде чем вы сможете попытаться войти снова", AlertType.Info);
				    return View(model);
			    }

			    LoginAttempts.Add(DateTime.Now);
			    Session[nameof(LoginAttempts)] = LoginAttempts;

			    var user = UserVM.GetUser(model);
			    if (user != null)
			    {
					// also gives him auth-cookies
					FormsAuthentication.SetAuthCookie(user.Id.ToString(), !model.DontRememberMe);
				    return Redirect(Request.Params["ReturnUrl"] ?? FormsAuthentication.DefaultUrl);
			    }
			    this.Alert("Не удалось войти, проверьте данные и попробуйте еще раз.", AlertType.Danger);
			}
			this.Alert("Данные заполнены неверно.", AlertType.Danger);
			return View(model);
	    }

		/// <summary>
		/// Counts amount of login attempts and alerts if there are more than allowed
		/// </summary>
		/// <returns>whether user has exceeded amount of attemptsto login</returns>
	    private bool CheckLoginAttempts()
	    {
		    if (Session[nameof(LoginAttempts)] == null)
			    Session[nameof(LoginAttempts)] = LoginAttempts;
		    else
		    {
			    LoginAttempts = ((List<DateTime>) Session[nameof(LoginAttempts)]);
		    }

		    LoginAttempts.RemoveAll(attemptTime => DateTime.Now - attemptTime > LoginAttemptCooldown);

		    if (LoginAttempts.Count > LoginAttemptsAllowed)
		    {
			    this.Alert("Вы ввели данные неправильно более 5 раз, подождите 3 минуты, прежде чем вы сможете попытаться войти снова", AlertType.Info);
			    return true;
		    }

		    return false;
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