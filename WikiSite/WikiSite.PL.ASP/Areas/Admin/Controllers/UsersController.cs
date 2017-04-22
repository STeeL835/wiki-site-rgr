using System.Web.Mvc;
using WikiSite.PL.ASP.Classes;
using WikiSite.PL.ASP.Models;

namespace WikiSite.PL.ASP.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        // GET: Admin/Users
        public ActionResult Index()
        {
			// Receiving alert from deleting user
			this.CatchAlert();

			return View(UserVM.GetAllUsers());
        }


	    public ActionResult CreateUser()
	    {
		    return View(new SignupModel());
	    }
		[HttpPost][ValidateAntiForgeryToken]
	    public ActionResult CreateUser(SignupModel model)
	    {
		    if (ModelState.IsValid)
		    {
			    if (UserVM.AddUser(model, model.GetCredentialsVM()))
			    {
					this.Alert($"Пользователь {model.Nickname} успешно добавлен.", AlertType.Success);
			    }
			    else
			    {
				    this.Alert($"Произошла ошибка при добавлении пользователя {model.Nickname}. Пользователь не был добавлен.",
					    AlertType.Danger);
				}
		    }
		    else
		    {
				this.Alert($"Не вся модель заполнена верно. Пользователь не был добавлен.",
						AlertType.Danger);
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
				var user = (UserVM) TempData.Peek("user");
				var cred = (UserCredentialsVM) TempData.Peek("cred");

				if (UserVM.UpdateUser(model.GetUserVM(user)))
				{
					this.Alert("Информация успешно изменена.", AlertType.Success);
				}
				else
				{
					this.Alert("Произошла ошибка при изменении информации о пользователе. Проверьте правильность данных.",
						AlertType.Danger);
				}

				if (model.ChangePassword)
				{
					if (UserCredentialsVM.GetCredentials(cred.Id).PasswordHash
						.Equals(UserCredentialsVM.ComputeHashForPassword(model.OldPassword)))
					{
						if (UserCredentialsVM.UpdateCredentials(model.GetCredentialsVM(cred)))
						{
							this.AppendAlert("Пароль успешно изменён.", AlertType.Success);
						}
						else
						{
							this.AppendAlert("Произошла ошибка при изменении пароля.", AlertType.Danger);
						}
					}
					else
					{
						this.AppendAlert("Текущий пароль неверен, пароль не изменен. Проверьте данные и попробуйте еще раз", AlertType.Danger);
					}
				}
			}
			else
			{
				this.Alert("Проверьте введенные данные", AlertType.Warning);
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
				this.AlertNextAction($"Пользователь {user.Nickname}({user.ShortId}) успешно удален", AlertType.Success);
			}
			else
			{
				this.AlertNextAction($"Произошла ошибка при удалении пользователя {user.Nickname}({user.ShortId}). Проверьте выполнение вручную.",
					AlertType.Danger);
			}
			return RedirectToAction("Index");
		}


	    public JsonResult IsLoginExist(string login)
	    {
		    var throwError = !UserCredentialsVM.IsLoginExist(login);

			return Json(throwError, JsonRequestBehavior.AllowGet);
	    }

    }
}