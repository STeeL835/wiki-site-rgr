using System;
using System.Security.Cryptography;
using System.Text;
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
		private byte[] _passwordHash;

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
		public byte[] PasswordHash
		{
			get { return _passwordHash; }
			set
			{
				if (value == null || value.Length == 0) throw new ArgumentException("Hash mustn't be empty");
				_passwordHash = value;
			}
		}

		public UserCredentialsVM(string login, string password)
		{
			Id = Guid.NewGuid();
			Login = login;
			PasswordHash = ComputeHashForPassword(password);
		}

		public UserCredentialsVM(Guid id, string login, byte[] passwordHash)
		{
			Id = id;
			Login = login;
			PasswordHash = passwordHash;
		}

		public static implicit operator UserCredentialsDTO(UserCredentialsVM vm)
			=> new UserCredentialsDTO { Id = vm.Id, Login = vm.Login, PasswordHash = vm._passwordHash};

		public static explicit operator UserCredentialsVM(UserCredentialsDTO dto) =>
			new UserCredentialsVM(dto.Id, dto.Login, dto.PasswordHash);

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

		/// <summary>
		/// Returns login and password hash
		/// </summary>
		/// <param name="credentialsId">user's credentials id</param>
		/// <returns>Credentials DTO</returns>
		public static UserCredentialsVM GetCredentials(Guid id)
		{
			return (UserCredentialsVM) _bll.GetCredentials(id);
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

		/// <summary>
		/// Converts a string password into hash
		/// </summary>
		/// <param name="password">password string</param>
		/// <returns>bytes array that contains (is) hash</returns>
		public static byte[] ComputeHashForPassword(string password)
		{
			return SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(password));
		}

		#endregion
	}
}