using System;
using System.Linq;
using System.Web.Mvc;
using WikiSite.PL.ASP.Classes;
using WikiSite.PL.ASP.Models;

namespace WikiSite.PL.ASP.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return RedirectToAction("Details", "User", new { id = UserVM.GetUser(Guid.Parse(User.Identity.Name)).ShortId});
        }

	    public ActionResult Details(int id)
	    {
		    var user = UserVM.GetUser(id);
		    return View(user);
	    }

		[Authorize]
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

		[HttpPost][Authorize]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(UserEditModel model)
		{
			if (ModelState.IsValid)
			{
				var user = (UserVM) TempData.Peek("user");
				var login = (string) TempData.Peek("login");
				var email = (string) TempData.Peek("email");

				if (model.RoleId == Guid.Empty) model.RoleId = user.RoleId;

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


	}
}