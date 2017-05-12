using System;
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

	        var users = UserVM.GetAllUsers();
			ViewBag.Roles = users.ToDictionary(user => user.Id, user => RoleVM.GetRole(user.RoleId));

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


	    public ActionResult Details(int id)
	    {
		    var user = UserVM.GetUser(id);
		    ViewBag.Role = RoleVM.GetRole(user.RoleId);
		    ViewBag.ArticlesCount = 13;
		    ViewBag.VersionsCount = 37;
		    ViewBag.CommentsCount = 228;

			return View(user);
	    }

		[HandleAllErrors(Message = "Deleting User")]
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
    }
}