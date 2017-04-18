using System;
using System.Web.Mvc;
using WikiSite.PL.ASP.Models;

namespace WikiSite.PL.ASP.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        // GET: Admin/Users
        public ActionResult Index()
        {
			// Receiving alert from deleting user
	        ViewBag.AlertMessage = TempData["AlertMessage"];
	        ViewBag.AlertClass = TempData["AlertClass"];

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


	    public ActionResult EditUser(int id)
	    {
		    var user = UserVM.GetUser(id);
		    var cred = UserCredentialsVM.GetCredentials(user.CredentialsId);
		    var model = new UserEditModel(user);

		    TempData["user"] = user;
		    TempData["cred"] = cred;

			return View(model);
		}
		[HttpPost][ValidateAntiForgeryToken]
		public ActionResult EditUser(UserEditModel model)
		{
			if (ModelState.IsValid)
			{
				if (UserVM.UpdateUser(model.GetUserVM((UserVM) TempData["user"])))
				{
					ViewBag.AlertMessage = "Информация успешно изменена";
					ViewBag.AlertClass = "alert-success";
				}
				else
				{
					ViewBag.AlertMessage = "Произошла ошибка при изменении информации о пользователе." +
										   "Проверьте правильность данных.";
					ViewBag.AlertClass = "alert-danger";
				}

				if (model.ChangePassword)
				{
					if (UserCredentialsVM.UpdateCredentials(model.GetCredentialsVM((UserCredentialsVM) TempData["cred"])))
					{
						ViewBag.AlertMessage += "Пароль успешно изменён";
						ViewBag.AlertClass = ViewBag.AlertClass == "alert-danger" ? "alert-warning" : "alert-success";
					}
					else
					{
						ViewBag.AlertMessage += "Произошла ошибка при изменении пароля";
						ViewBag.AlertClass = ViewBag.AlertClass == "alert-success" ? "alert-warning" : "alert-danger";
					}
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


	    public ActionResult UserDetails(int id)
	    {
		    var user = UserVM.GetUser(id);
		    return View(user);
	    }


	    public ActionResult DeleteUser(int id)
	    {
		    var user = UserVM.GetUser(id);
			if (UserVM.RemoveUser(user.Id))
			{
				// TempData - short-life data transfering between requests 
				TempData["AlertMessage"] = $"Пользователь {user.Nickname}({user.ShortId}) успешно удален"; // is is vulnerable for XSS?
				TempData["AlertClass"] = "alert-success";
			}
			else
			{
				TempData["AlertMessage"] = $"Произошла ошибка при удалении пользователя {user.Nickname}({user.ShortId})." +
									   $"Проверьте выполнение вручную"; // is is vulnerable for XSS?
				TempData["AlertClass"] = "alert-danger";
			}
			return RedirectToAction("Index");
		}


	    public JsonResult IsLoginExist(string login)
	    {
		    var throwError = !UserCredentialsVM.IsLoginExist(login);

			return Json(throwError, JsonRequestBehavior.AllowGet);
	    }

	    public JsonResult CheckPassword(string password)
	    {
		    var throwError =
			    UserVM.GetCheckCredentials(new UserCredentialsVM(Guid.Empty, ((UserCredentialsVM) TempData["cred"]).Login,
				    UserCredentialsVM.ComputeHashForPassword(password)));
			return Json(throwError, JsonRequestBehavior.AllowGet);
	    }
    }
}