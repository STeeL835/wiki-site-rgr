using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Foolproof;
using WikiSite.BLL.Abstract;
using WikiSite.DI.Provider;
using WikiSite.Entities;

namespace WikiSite.PL.ASP.Models
{
	public class UserCredentialsVM
	{
		#region Instance

		private Guid _id;
		private string _login;
		private string _email;
		private string _password;

		public Guid Id
		{
			get { return _id; }
			set
			{
				if (value == Guid.Empty) throw new ArgumentException("Empty id");
				_id = value;
			}
		}

		[RequiredIfEmpty("Email")][Display(Name = "Логин")]
		[DataType(DataType.Text)]
		public string Login
		{
			get { return _login; }
			set
			{
				if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Login mustn't be empty");
				_login = value;
			}
		}

		[RequiredIfEmpty("Login")][Display(Name = "E-mail")]
		[DataType(DataType.EmailAddress)]
		public string Email
		{
			get { return _email; }
			set
			{
				if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Email mustn't be empty");
				_email = value;
			}
		}
		[Required(ErrorMessage = "Пароль не должен быть короче 8 символов")][Display(Name = "Пароль")]
		[DataType(DataType.Password)]
		public string Password
		{
			get { return _password; }
			set
			{
				if (string.IsNullOrWhiteSpace(value) || value.Length < 8)
					throw new ArgumentException("value didn't match the conditions");
				_password = value;
			}
		}

		/// <summary>
		/// Only for model binder
		/// </summary>
		public UserCredentialsVM()
		{
			_id = Guid.NewGuid();
		}

		public UserCredentialsVM(string login, string email, string password) : this(Guid.NewGuid(), login, email, password)
		{ }

		public UserCredentialsVM(Guid id, string login, string email, string password)
		{
			Id = id;
			Login = login;
			Email = email;
			Password = password;
		}

		public static implicit operator UserCredentialsDTO(UserCredentialsVM vm)
			=> new UserCredentialsDTO { Id = vm.Id, Login = vm.Login, Email = vm.Email, Password = vm._password};

		#endregion

		#region Static

		private static IUsersBLL _bll;
		private static MD5 _md5 = MD5.Create();

		static UserCredentialsVM()
		{
			_bll = Provider.UsersBLO;
		}

		/// <summary>
		/// Checks whether these credentials exist in db
		/// </summary>
		/// <param name="credentials">model for credentials</param>
		/// <returns>Whether these credentials exist in db</returns>
		public static bool AreCredentialsExist(UserCredentialsVM credentials)
		{
			return _bll.GetUser(credentials) != null;
		}

		/// <summary>
		/// Checks for login in db. Returns whether login is exist or not
		/// </summary>
		/// <param name="login">login string</param>
		/// <returns>Whether login is exist or not</returns>
		public static bool IsLoginExist(string login)
		{
			return _bll.IsLoginExist(login);
		}

		/// <summary>
		/// Checks for email in db. Returns whether email is exist or not
		/// </summary>
		/// <param name="email">email string</param>
		/// <returns>Whether email is email or not</returns>
		public static bool IsEmailExist(string email)
		{
			return _bll.IsEmailExist(email);
		}

		public static string GetLogin(Guid userId)
		{
			return _bll.GetLogin(userId);
		}

		public static string GetEmail(Guid userId)
		{
			return _bll.GetEmail(userId);
		}

		public static bool IsPasswordMatch(UserCredentialsVM vm)
		{
			return _bll.IsPasswordMatch(vm);
		}

		/// <summary>
		/// Updates credentials of a user
		/// </summary>
		/// <param name="credentials"></param>
		/// <returns></returns>
		public static bool UpdateCredentials(UserCredentialsVM credentials)
		{
			return _bll.UpdateUserCredentials(credentials);
		}

		public static string GetEmailHash(Guid userId)
		{
			var data = _md5.ComputeHash(Encoding.UTF8.GetBytes(GetEmail(userId))); // getting hash
			StringBuilder sBuilder = new StringBuilder();
			foreach (byte b in data) //converting byte array to hex string
			{
				sBuilder.Append(b.ToString("x2"));
			}
			return sBuilder.ToString();
		}

		public static string GetEmailHash(string userId)
		{
			return GetEmailHash(Guid.Parse(userId));
		}

		#endregion
	}
}