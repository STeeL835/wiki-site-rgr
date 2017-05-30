using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Foolproof;

namespace WikiSite.PL.ASP.Models
{
	public class UserEditModel
	{
		private string _nickname;
		private Guid _roleId;

		#region VM

		public int ShortId { get; set; }

		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "Никнейм")]
		[RegularExpression("^[0-9a-zA-ZА-Яа-яёЁ_ ]{3,50}$",
			ErrorMessage = "Никнейм может состоять только из латинских и русских букв, цифр, пробела и знака подчеркивания, " +
						   "а также не может быть короче 3 и длиннее 50 символов")]
		public string Nickname
		{
			get { return _nickname; }
			set
			{
				if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Nickname mustn't be empty");
				_nickname = value;
			}
		}

		[DataType(DataType.MultilineText)]
		[MaxLength(1500, ErrorMessage = "Максимальное количество символов - 1500")]
		[Display(Name = "О себе")]
		public string About { get; set; }

		public SelectList Roles { get; set; }

		[Required][Display(Name = "Роль")]
		public Guid RoleId
		{
			get { return _roleId; }
			set
			{
				if (value == Guid.Empty) throw new ArgumentException("Empty id for role");
				_roleId = value;
			}
		}

		[Display(Name = "Изменить пароль")]
		public bool ChangePassword { get; set; }

		[RequiredIf("ChangePassword", true, ErrorMessage = "Это поле необходимо, если вы хотите изменить пароль")]
		[DataType(DataType.Password)]
		[Display(Name = "Текущий пароль")]
		public string OldPassword { get; set; }

		[RequiredIf("ChangePassword", true, ErrorMessage = "Это поле необходимо, если вы хотите изменить пароль")] // new
		[DataType(DataType.Password)]
		[RegularExpression("^[0-9a-zA-Z!@#%$^&*+-]{8,50}$",
			ErrorMessage = "Пароль может содержать латинские буквы, цифры, символы !@#%$^&*+-, " +
						   "а также не может быть короче 8 и длиннее 50 символов")]
		[Display(Name = "Новый пароль")] // new 
		public string NewPassword { get; set; } // attributes are inherited even on overrided elements

		[RequiredIf("ChangePassword", true, ErrorMessage = "Это поле необходимо, если вы хотите изменить пароль")] // new
		[DataType(DataType.Password)]
		[System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
		[Display(Name = "Новый пароль еще раз")] // new 
		public string ConfirmNewPassword { get; set; }

		#endregion

		public UserEditModel()
		{
			Roles = new SelectList(RoleVM.GetRoles(),"Id","Name");
		}

		public UserEditModel(UserVM user) : this()
		{
			About = user.About;
			Nickname = user.Nickname;
			RoleId = user.RoleId;
			ShortId = user.ShortId;
		}

		/// <summary>
		/// Returns new and updated VM.
		/// Needs the original because id part gets lost from form,
		/// because it's never sent there
		/// </summary>
		/// <param name="user">current VM</param>
		/// <returns>New and updated VM.</returns>		
		public UserVM GetUserVM(UserVM user)
		{
			return new UserVM(user.Id, user.CredentialsId, Nickname, RoleId)
			{
				About = About
			};
		}

		/// <summary>
		/// Returns new and updated VM.
		/// Needs the original because some values get lost from form,
		/// because they're never sent there
		/// </summary>
		/// <param name="credId">current credentials id</param>
		/// <param name="login">current login</param>
		/// <returns>New and updated VM.</returns>
		public UserCredentialsVM GetCredentialsVM(Guid credId, string login, string email)
		{
			return new UserCredentialsVM(credId, login, email, NewPassword);
		}

	}
}