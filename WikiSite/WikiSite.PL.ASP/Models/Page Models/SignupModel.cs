using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WikiSite.PL.ASP.Models
{
	public class SignupModel : UserVM
	{
		#region VM

		/* Nickname */

		/* About */
	
		public SelectList Roles { get; set; }


		[Required][DataType(DataType.Text)]
		[RegularExpression("^[0-9a-zA-Z\\-]{4,50}$")]
		[Remote("IsLoginExist", "Users", ErrorMessage = "Такой логин уже существует")]
		[Display(Name = "Логин")]
		public string Login { get; set; }

		[Required][DataType(DataType.Password)]
		[RegularExpression("^[0-9a-zA-Z!@#%$^&*+-]{8,50}$")]
		[Display(Name = "Пароль")]
		public virtual string Password { get; set; }

		[Required][DataType(DataType.Password)]
		[System.ComponentModel.DataAnnotations.Compare("Password")]
		[Display(Name = "Пароль еще раз")]
		public virtual string ConfirmPassword { get; set; }

		#endregion

		/* User's and credentials ID are generated in a base constructor */

		public SignupModel() : base()
		{
			Roles = new SelectList(RoleVM.GetRoles(),"Id","Name");
		}

		public UserCredentialsVM GetCredentialsVM()
		{
			return new UserCredentialsVM(CredentialsId, Login, UserCredentialsVM.ComputeHashForPassword(Password));
		}
	}
}