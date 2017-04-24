using System;
using System.ComponentModel.DataAnnotations;

namespace WikiSite.PL.ASP.Models
{
	public class AuthVM : UserCredentialsVM
	{
		[Display(Name = "Не запоминать меня")]
		public bool DontRememberMe { get; set; } = false;

		/// <summary>
		/// Only for model binder
		/// </summary>
		public AuthVM()
		{
			
		}

		public AuthVM(string login, string password) : base(login, password)
		{
		}

		public AuthVM(Guid id, string login, string password) : base(id, login, password)
		{
		}
	}
}