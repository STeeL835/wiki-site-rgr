using System;

namespace WikiSite.Entities
{
	public class UserCredentialsDBDTO
	{
		public Guid Id { get; set; }
		public string Login { get; set; }
		public string Email { get; set; }
		public byte[] PasswordHash { get; set; }
	}
}