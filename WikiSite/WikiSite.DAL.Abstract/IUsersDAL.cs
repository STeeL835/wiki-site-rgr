using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiSite.Entities;

namespace WikiSite.DAL.Abstract
{
    public interface IUsersDAL
    {
	    void AddUser(UserDTO user);
	    void UpdateUser(UserDTO user);
	    void RemoveUser(Guid userId);

	    IEnumerable<UserDTO> GetUsers();
	    IEnumerable<UserDTO> GetUsers(Guid roleId);
	    UserDTO GetUser(Guid userId);
	    UserDTO GetUser(int userShortId);

	    IEnumerable<UserDTO> SearchUsers(string searchInput);
    }
}
