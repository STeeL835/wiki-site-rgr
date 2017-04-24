using System.Web.Mvc;
using System.Web.Security;
using WikiSite.PL.ASP.Classes;
using WikiSite.PL.ASP.Models;

namespace WikiSite.PL.ASP.Controllers
{
    public class AuthController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
		[HttpPost][ValidateAntiForgeryToken]
	    public ActionResult Login(AuthVM model)
	    {
		    if (ModelState.IsValid)
		    {
			    if (UserCredentialsVM.AreCredentialsExist(model))
			    {
				    FormsAuthentication.SetAuthCookie(model.Login, !model.DontRememberMe);
				    return RedirectBack();
			    }
			    this.Alert("Не удалось войти, проверьте данные и попробуйте еще раз.", AlertType.Danger);
			}
			this.Alert("Данные заполнены неверно.", AlertType.Danger);
			return View(model);
	    }


	    public ActionResult Register()
	    {
		    return View();
	    }
		[HttpPost][ValidateAntiForgeryToken]
	    public ActionResult Register(RegisterVM model)
		{
			if (ModelState.IsValid)
			{
				if (UserVM.AddUser(model, model.GetCredentialsVM()))
				{
					return RedirectBack();
				}
				this.Alert("Произошла ошибка при регистрации.", AlertType.Danger);
			}
			else
			{
				this.Alert("Не вся модель заполнена верно. Проверьте поля и попробуйте снова.", AlertType.Warning);
			}
			return View(model);
		}


	    public ActionResult Logout()
	    {
		    FormsAuthentication.SignOut();
			return RedirectBack();
	    }


	    public ActionResult RedirectBack()
	    {
			if (Request.Params["ReturnUrl"] != null)
				return Redirect(Request.Params["ReturnUrl"]);

			return RedirectToAction("Index", "Home");
		}
    }
}