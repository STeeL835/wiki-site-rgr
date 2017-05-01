using System;

namespace WikiSite.Entities
{
	public class UserCredentialsInDTO
	{
		public Guid Id { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
	}
}