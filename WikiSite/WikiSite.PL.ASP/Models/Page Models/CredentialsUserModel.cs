using System.ComponentModel.DataAnnotations;

namespace WikiSite.PL.ASP.Models
{
	// Not inherited because User can be null
	public class CredentialsUserModel
	{
		private string _email;
		private string _login;

		[Required][DataType(DataType.Text)]
		[Display(Name = "Логин")]
		public string Login {
			get { return _login; }
			set
			{
				_login = value;
				if (value.Contains("@"))
					_email = value;
			} }

		[Required][DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		public UserVM User { get; set; }

		public UserCredentialsVM GetCredentials() => new UserCredentialsVM(_login, _email, Password);
	}
}