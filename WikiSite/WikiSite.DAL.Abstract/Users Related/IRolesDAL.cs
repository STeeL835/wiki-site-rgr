using System;
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
	}
}