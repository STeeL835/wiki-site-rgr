using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WikiSite.PL.ASP.Models
{
	public class SignupModel : UserVM
	{
		#region VM

		private RoleVM.RolesEnum _role;

		/* Nickname */

		/* About */

		[EnumDataType(typeof(RoleVM.RolesEnum))]
		[Display(Name = "Роль")]
		public RoleVM.RolesEnum Role
		{
			get { return _role; }
			set
			{
				_role = value;
				RoleId = RoleVM.GetRole(value).Id;
			}
		}


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

		public UserCredentialsVM GetCredentialsVM()
		{
			return new UserCredentialsVM(CredentialsId, Login, UserCredentialsVM.ComputeHashForPassword(Password));
		}
	}
}