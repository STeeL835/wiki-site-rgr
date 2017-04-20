using System;
using System.Web.Mvc;

namespace WikiSite.PL.ASP.Classes
{
	public static class ControllerExtentions
	{
		/// <summary>
		/// Adds an alert to a page using ViewBag
		/// </summary>
		/// <remarks>
		/// It's only a helper, to make this work, assure that
		/// layout checks ViewBag.
		/// </remarks>
		/// <param name="controller">Controller</param>
		/// <param name="message">message that should appear in the alert</param>
		/// <param name="alertType">colour of the alert</param>
		public static void Alert(this Controller controller, string message, AlertType alertType)
		{
			ClearAlert(controller);
			controller.ViewBag.AlertMessage = message;
			controller.ViewBag.AlertClass = GetAlertClass(alertType);
		}

		/// <summary>
		/// Appends new info to an alert that is going to be added on a page.
		/// Colour will be mixed
		/// </summary>
		/// <remarks>
		/// It's only a helper, to make this work, assure that
		/// layout checks ViewBag.
		/// </remarks>
		/// <param name="controller">Controller</param>
		/// <param name="message">message that should appear in the alert</param>
		/// <param name="alertType">colour of the alert</param>
		public static void AppendAlert(this Controller controller, string message, AlertType alertType)
		{
			if (controller.ViewBag.AlertMessage == null || controller.ViewBag.AlertClass == null)
			{
				ClearAlert(controller);
				Alert(controller, message, alertType);
			}
			controller.ViewBag.AlertMessage += $" {message}";
			controller.ViewBag.AlertClass = MixAlertTypes(controller.ViewBag.AlertClass, alertType);
		}

		/// <summary>
		/// Adds an alert to a page after redirect using TempData.
		/// Action-recipient must use <see cref="CatchAlert"/>() method
		/// </summary>
		/// <remarks>
		/// It's only a helper, to make this work, assure that
		/// layout checks ViewBag.
		/// </remarks>
		/// <param name="controller">Controller</param>
		/// <param name="message">message that should appear in the alert</param>
		/// <param name="alertType">colour of the alert</param>
		public static void AlertNextAction(this Controller controller, string message, AlertType alertType)
		{
			ClearAlert(controller);
			controller.TempData["AlertMessage"] = message;
			controller.TempData["AlertClass"] = GetAlertClass(alertType);
		}

		/// <summary>
		/// Adds an alert to a page using ViewBag
		/// </summary>
		/// <remarks>
		/// It's only a helper, to make this work, assure that
		/// layout checks ViewBag.
		/// </remarks>
		/// <param name="controller">Controller</param>
		public static void CatchAlert(this Controller controller)
		{
			ClearAlert(controller);
			controller.ViewBag.AlertMessage = controller.TempData["AlertMessage"];
			controller.ViewBag.AlertClass = controller.TempData["AlertClass"];
		}

		/// <summary>
		/// Clears alert info
		/// </summary>
		/// <param name="controller"></param>
		public static void ClearAlert(this Controller controller)
		{
			controller.ViewBag.AlertMessage = null;
			controller.ViewBag.AlertClass = null;
		}


		/// <summary>
		/// Return a CSS class for an <see cref="AlertType"/>
		/// </summary>
		/// <param name="type">Type of the alert (colour)</param>
		/// <returns></returns>
		public static string GetAlertClass(AlertType type)
		{
			switch (type)
			{
				case AlertType.Info:
					return "alert-info";
				case AlertType.Success:
					return "alert-success";
				case AlertType.Warning:
					return "alert-warning";
				case AlertType.Danger:
					return "alert-danger";
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}

		/// <summary>
		/// Mixes colours of an alert
		/// </summary>
		/// <param name="type1">First colour</param>
		/// <param name="type2">Second colour</param>
		/// <returns></returns>
		public static AlertType MixAlertTypes(AlertType type1, AlertType type2)
		{
			return (type1 == AlertType.Danger && type2 == AlertType.Warning ||
					type1 == AlertType.Warning && type2 == AlertType.Danger) ?
						AlertType.Danger :
						(type1 > type2) ? type1 : type2;
		}
	}

	public enum AlertType
	{
		Info = 0,
		Success = 1,
		Warning = 3,
		Danger = 2
	}
}