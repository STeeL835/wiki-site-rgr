using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiSite.Entities
{
    public class UserDTO
    {
		public Guid Id { get; set; }
		public int SmallId { get; set; }
		public Guid CredentialsId { get; set; }
		public string Nickname { get; set; }
		public string About { get; set; }
		public Guid RoleId { get; set; }
    }
}
