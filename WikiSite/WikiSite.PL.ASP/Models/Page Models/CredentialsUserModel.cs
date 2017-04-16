using System.ComponentModel.DataAnnotations;

namespace WikiSite.PL.ASP.Models
{
	public class CredentialsUserModel
	{
		[Required][DataType(DataType.Text)]
		[Display(Name = "Логин")]
		public string Login { get; set; }

		[Required][DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		public UserVM User { get; set; }

		public UserCredentialsVM GetCredentials() => new UserCredentialsVM(Login, Password);
	}
}