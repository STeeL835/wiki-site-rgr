using System;

namespace WikiSite.Entities
{
	public class CredentialsDTO
	{
		public Guid Id { get; set; }
		public string Login { get; set; }
		public string PasswordHash { get; set; }
	}
}