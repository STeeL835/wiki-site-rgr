﻿using System;
using System.Linq;
using System.Web.Mvc;
using WikiSite.PL.ASP.Classes;
using WikiSite.PL.ASP.Models;

namespace WikiSite.PL.ASP.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        // GET: Admin/Users
        public ActionResult Index()
        {
			// Receiving alert from deleting user
			this.CatchAlert();

	        var users = UserVM.GetAllUsers().ToArray();
            return View(users);
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

	    public ActionResult Edit(Guid guid)
	    {
		    return RedirectToAction("Edit", "Users", new {id = UserVM.GetUser(guid).ShortId});
	    }
	    public ActionResult Edit(int id)
	    {
		    var user = UserVM.GetUser(id);
		    var login = UserCredentialsVM.GetLogin(user.Id);
		    var email = UserCredentialsVM.GetEmail(user.Id);
		    var model = new UserEditModel(user);

		    //Saving data between requests (guids aren't in form)
		    TempData["user"] = user;
		    TempData["login"] = login;
		    TempData["email"] = email;

			return View(model);
		}
		[HttpPost][ValidateAntiForgeryToken]
		public ActionResult Edit(UserEditModel model)
		{
			if (ModelState.IsValid)
			{
				var user = (UserVM) TempData.Peek("user");
				var login = (string) TempData.Peek("login");
				var email = (string) TempData.Peek("email");

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
					if (UserCredentialsVM.IsPasswordMatch(new UserCredentialsVM(login, email, model.NewPassword)))
					{
						if (UserCredentialsVM.UpdateCredentials(model.GetCredentialsVM(user.CredentialsId, login, email)))
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
		    model.User = UserVM.GetUser(model.GetCredentials());
			return View(model);
	    }

		public ActionResult Details(Guid guid)
		{
			return RedirectToAction("Details", "Users", new { id = UserVM.GetUser(guid).ShortId });
		}
		public ActionResult Details(int id)
	    {
		    var user = UserVM.GetUser(id);
		    ViewBag.Role = RoleVM.GetRole(user.RoleId);
		    ViewBag.Articles = ArticleVM.GetAllArticles(user.Id);
		    ViewBag.Versions = ArticleVM.GetAllVersionByAuthor(user.Id);
		    ViewBag.CommentsCount = "228*";

			return View(user);
	    }

		public ActionResult Delete(Guid guid)
		{
			return RedirectToAction("Delete", "Users", new { id = UserVM.GetUser(guid).ShortId });
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

		// Admin/Users/Search?query=query
	    public ActionResult Search(string query)
	    {
		    ViewBag.SearchQuery = query;
		    if (query == null || query.Length < 3)
		    {
			    this.Alert("Длина строки должна быть больше 2 символов", AlertType.Info);
			    return View(new UserVM[0]);
		    }
		    return View(UserVM.SearchUsers(query));
	    }
    }
}