﻿using System.Web.Mvc;
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


	    public ActionResult Create()
	    {
		    return View(new RegisterVM());
	    }
		[HttpPost][ValidateAntiForgeryToken]
	    public ActionResult Create(RegisterVM model)
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


	    public ActionResult Edit(int id)
	    {
		    var user = UserVM.GetUser(id);
		    var login = UserCredentialsVM.GetLogin(user.Id);
		    var model = new UserEditModel(user);

		    TempData["user"] = user;
		    TempData["login"] = login;

			return View(model);
		}
		[HttpPost][ValidateAntiForgeryToken]
		public ActionResult Edit(UserEditModel model)
		{
			if (ModelState.IsValid)
			{
				var user = (UserVM) TempData.Peek("user");
				var login = (string) TempData.Peek("login");

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
					if (UserCredentialsVM.IsPasswordMatch(new UserCredentialsVM(login, model.NewPassword)))
					{
						if (UserCredentialsVM.UpdateCredentials(model.GetCredentialsVM(user.CredentialsId, login)))
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


	    public ActionResult Details(int id)
	    {
		    var user = UserVM.GetUser(id);
		    return View(user);
	    }


	    public ActionResult Delete(int id)
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