using System;
using System.Collections.Generic;
using WikiSite.Entities;

namespace WikiSite.BLL.Abstract
{
	public interface IRolesBLL
	{
		/// <summary>
		/// Returns a role with certain id
		/// </summary>
		/// <param name="id">Guid of a role</param>
		/// <returns>DTO of a role</returns>
		RoleDTO GetRole(Guid id);

		/// <summary>
		/// Returns a role by it's name
		/// </summary>
		/// <param name="name">name of a role</param>
		/// <returns>Role DTO</returns>
		RoleDTO GetRole(string name);

		/// <summary>
		/// Returns all the roles that exist in db
		/// </summary>
		/// <returns>Roles sequence</returns>
		IEnumerable<RoleDTO> GetRoles();
	}
}