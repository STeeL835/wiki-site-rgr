using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Altairis.ValidationToolkit;

namespace WikiSite.PL.ASP.Models
{
	public class UserEditModel
	{
		#region VM

		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "Никнейм")]
		[RegularExpression("^[0-9a-zA-ZА-Яа-яёЁ_ ]{3,50}$")]
		public string Nickname { get; set; }

		[DataType(DataType.MultilineText)]
		[MaxLength(1500)]
		[Display(Name = "О себе")]
		public string About { get; set; }

		[EnumDataType(typeof(RoleVM.RolesEnum))]
		[Display(Name = "Роль")]
		public RoleVM.RolesEnum Role { get; set; }


		[Display(Name = "Изменить пароль")]
		public bool ChangePassword { get; set; }

		[RequiredWhen("ChangePassword", true)]
		[DataType(DataType.Password)]
		[RegularExpression("^[0-9a-zA-Z!@#%$^&*+-]{8,50}$")]
		[Display(Name = "Старый пароль")]
		public string OldPassword { get; set; }

		[RequiredWhen("ChangePassword", true)]
		[DataType(DataType.Password)]
		[RegularExpression("^[0-9a-zA-Z!@#%$^&*+-]{8,50}$")]
		[Display(Name = "Новый пароль")]
		public string Password { get; set; }

		[RequiredWhen("ChangePassword", true)]
		[DataType(DataType.Password)]
		[System.ComponentModel.DataAnnotations.Compare("Password")]
		[Display(Name = "Новый пароль еще раз")]
		public string ConfirmPassword { get; set; }

		#endregion

		public UserEditModel()
		{
			// please, model binder, here's your parameterless constructor
		}

		public UserEditModel(UserVM user)
		{
			About = user.About;
			Nickname = user.Nickname;
			Role = RoleVM.GetRoleEnum(user.RoleId);
		}

		public UserVM GetUserVM(UserVM user)
		{
			return new UserVM(user.Id, user.CredentialsId, Nickname, RoleVM.GetRole(Role).Id)
			{
				About = About
			};
		}

		public UserCredentialsVM GetCredentialsVM(UserCredentialsVM credentials)
		{
			return new UserCredentialsVM(credentials.Id, credentials.Login, UserCredentialsVM.ComputeHashForPassword(Password));
		}
	}
}