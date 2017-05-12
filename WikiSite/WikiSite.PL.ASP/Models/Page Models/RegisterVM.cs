using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WikiSite.PL.ASP.Models
{
	public class RegisterVM : UserVM
	{
		#region VM

		/* Nickname */

		/* About */
	
		public SelectList Roles { get; set; }


		[Required][DataType(DataType.Text)]
		[RegularExpression("^[0-9a-zA-Z\\-]{4,50}$", 
			ErrorMessage = "Логин может состоять только из латинских букв, цифр и знака тире, " +
			               "а также не может быть короче 4 и длиннее 50 символов")]
		[Remote("IsLoginExist", "Auth", AreaReference.UseRoot, ErrorMessage = "Такой логин уже занят")]
		[Display(Name = "Логин")]
		public string Login { get; set; }

		[Required][Display(Name = "E-mail")]
		[DataType(DataType.EmailAddress, ErrorMessage = "Проверьте правильность введенного email")]
		[Remote("IsEmailExist", "Auth", AreaReference.UseRoot, ErrorMessage = "Такой e-mail уже привязан")]
		public string Email { get; set; }

		[Required][DataType(DataType.Password)]
		[RegularExpression("^[0-9a-zA-Z!@#%$^&*+-]{8,50}$", 
			ErrorMessage = "Пароль может содержать латинские буквы, цифры, символы !@#%$^&*+-, " +
			               "а также не может быть короче 8 и длиннее 50 символов")]
		[Display(Name = "Пароль")]
		public virtual string Password { get; set; }

		[Required][DataType(DataType.Password)]
		[System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Пароли не совпадают")]
		[Display(Name = "Пароль еще раз")]
		public virtual string ConfirmPassword { get; set; }

		#endregion

		/* User's and credentials ID are generated in a base constructor */

		public RegisterVM() : base()
		{
			Roles = new SelectList(RoleVM.GetRoles(),"Id","Name");
			RoleId = RoleVM.GetRole("User").Id;
		}

		public UserCredentialsVM GetCredentialsVM()
		{
			return new UserCredentialsVM(CredentialsId, Login, Email, Password);
		}
	}
}
