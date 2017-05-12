using System;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace WikiSite.PL.ASP.Classes
{
	public class HandleAllErrorsAttribute : HandleErrorAttribute
	{
		public static ILog Log = LogManager.GetLogger(typeof(HandleAllErrorsAttribute));

		public string Message { get; set; }

		public override void OnException(ExceptionContext filterContext)
		{
			
			if (filterContext == null)
			{
				throw new ArgumentNullException(nameof(filterContext));
			}

			// If custom errors are disabled, we need to let the normal ASP.NET exception handler
			// execute so that the user can see useful debugging information.
			if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
			{
				return;
			}

			Exception exception = filterContext.Exception;

			if (!ExceptionType.IsInstanceOfType(exception))
			{
				return;
			}
			
			string controllerName = (string)filterContext.RouteData.Values["controller"];
			string actionName = (string)filterContext.RouteData.Values["action"];
			string areaName = (string) filterContext.RouteData.DataTokens["area"];

			Log.Error(
				Message != null
					? $"{areaName}/{controllerName}/{actionName} - {Message}"
					: $"{areaName}/{controllerName}/{actionName}", exception);

			HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
			filterContext.Result = new ViewResult
			{
				ViewName = View,
				MasterName = Master,
				ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
				TempData = filterContext.Controller.TempData
			};
			filterContext.ExceptionHandled = true;
			filterContext.HttpContext.Response.Clear();
			filterContext.HttpContext.Response.StatusCode = new HttpException(null, exception).GetHttpCode();

			// Certain versions of IIS will sometimes use their own error page when
			// they detect a server error. Setting this property indicates that we
			// want it to try to render ASP.NET MVC's error page instead.
			filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
		}
	}
}