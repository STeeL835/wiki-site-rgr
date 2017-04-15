using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WikiSite.PL.ASP.Models
{
	public class SignupModel
	{
		#region VM

		[Required][DataType(DataType.Text)]
		[Display(Name = "Никнейм")]
		[RegularExpression("^[0-9a-zA-ZА-Яа-яёЁ_ ]{3,50}$")]
		public string Nickname { get; set; }

		[DataType(DataType.MultilineText)][MaxLength(1500)]
		[Display(Name = "О себе")]
		public string About { get; set; }

		[EnumDataType(typeof(RoleVM.RolesEnum))]
		[Display(Name = "Роль")]
		public RoleVM.RolesEnum Role { get; set; }


		[Required][DataType(DataType.Text)]
		[RegularExpression("^[0-9a-zA-Z\\-]{4,50}$")]
		[Remote("IsLoginExist", "Users", ErrorMessage = "Такой логин уже существует")]
		[Display(Name = "Логин")]
		public string Login { get; set; }

		[Required][DataType(DataType.Password)]
		[RegularExpression("^[0-9a-zA-Z!@#%$^&*+-]{8,50}$")]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		[Required][DataType(DataType.Password)]
		[System.ComponentModel.DataAnnotations.Compare("Password")]
		[Display(Name = "Пароль еще раз")]
		public string ConfirmPassword { get; set; }

		#endregion

		private Guid _credentialsId = Guid.NewGuid();
		private Guid _userId = Guid.NewGuid();

		public UserVM GetUserVM()
		{
			return new UserVM(_userId, _credentialsId, Nickname, RoleVM.GetRole(Role).Id)
			{
				About = About
			};
		}

		public UserCredentialsVM GetCredentialsVM()
		{
			return new UserCredentialsVM(_credentialsId, Login, UserCredentialsVM.ComputeHashForPassword(Password));
		}
	}

}