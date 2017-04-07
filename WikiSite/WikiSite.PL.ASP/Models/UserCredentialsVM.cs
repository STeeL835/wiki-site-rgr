using System;
using System.Security.Cryptography;
using System.Text;
using WikiSite.Entities;

namespace WikiSite.PL.ASP.Models
{
	public class UserCredentialsVM
	{
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
				if (_passwordHash == null || _passwordHash.Length == 0) throw new ArgumentException("Hash mustn't be empty");
				_passwordHash = value;
			}
		}

		public UserCredentialsVM(string login, string password)
		{
			Id = Guid.NewGuid();
			Login = login;
			PasswordHash = SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(password));
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
	}
}