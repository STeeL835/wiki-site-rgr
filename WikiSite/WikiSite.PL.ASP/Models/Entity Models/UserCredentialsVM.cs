using System;
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
		//private byte[] _passwordHash;
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
		public string Login
		{
			get { return _login; }
			set
			{
				if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Login mustn't be empty");
				_login = value;
			}
		}
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

		public UserCredentialsVM(string login, string password) : this(Guid.NewGuid(), login, password)
		{ }

		public UserCredentialsVM(Guid id, string login, string password)
		{
			Id = id;
			Login = login;
			Password = password;
		}

		public static implicit operator UserCredentialsInDTO(UserCredentialsVM vm)
			=> new UserCredentialsInDTO { Id = vm.Id, Login = vm.Login, Password = vm._password};

		#endregion

		#region Static

		private static IUsersBLL _bll;

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

		public static string GetLogin(Guid userId)
		{
			return _bll.GetLogin(userId);
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


		#endregion
	}
}