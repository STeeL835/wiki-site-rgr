using System;
using System.Security.Cryptography;

namespace WikiSite.Entities
{
	public class UserCredentialsOutDTO
	{
		public Guid Id { get; set; }
		public string Login { get; set; }
		public byte[] PasswordHash { get; set; }
	}
}