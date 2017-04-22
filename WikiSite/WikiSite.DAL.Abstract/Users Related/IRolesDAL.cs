using System;
using System.Collections.Generic;
using WikiSite.Entities;

namespace WikiSite.DAL.Abstract
{
	public interface IRolesDAL
	{
		/// <summary>
		/// Returns a role with certain id
		/// </summary>
		/// <param name="id">Guid of a role</param>
		/// <returns>DTO of a role</returns>
		RoleDTO GetRole(Guid id);

		/// <summary>
		/// Returns all the roles that exist in db
		/// </summary>
		/// <returns>Roles sequence</returns>
		IEnumerable<RoleDTO> GetRoles();
	}
}