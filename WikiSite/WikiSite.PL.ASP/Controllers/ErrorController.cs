using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WikiSite.PL.ASP.Models;

namespace WikiSite.PL.ASP.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult NotFound()
        {
			var error = new ErrorVM(
				header: "404",
				title: "Страница не найдена",
				message: "Кажется, вы не туда попали. Здесь ничего нет. " +
				         "Если вы уверены, что тут что-то обязано быть, " +
				         "свяжитесь с администратором. Как-нибудь.."
				);
            return View("Error", error);
        }

    }
}