using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WikiSite.PL.ASP.Models
{
	public class AuthVM : UserCredentialsVM
	{
		private string _loginEmail;

		[Display(Name = "Не запоминать меня")]
		public bool DontRememberMe { get; set; } = false;

		[Required][Display(Name = "Логин или E-mail")]
		[Remote("CheckLogin", "Auth", AreaReference.UseRoot,
			ErrorMessage = "Проверьте Email, похоже, он введен неправильно, или используйте логин для входа")]
		public string LoginEmail
		{
			get { return _loginEmail; }
			set
			{
				_loginEmail = value;
				if (value.Contains("@"))
					Email = value;
				else
					Login = value;
			}
		}

		/// <summary>
		/// Only for model binder
		/// </summary>
		public AuthVM()
		{

		}

		public AuthVM(string login, string email, string password) : base(login, email, password)
		{
		}

		public AuthVM(Guid id, string login, string email, string password) : base(id, login, email, password)
		{
		}
	}
}