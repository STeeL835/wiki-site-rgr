using System;
using System.Collections.Generic;
using System.Linq;
using WikiSite.BLL.Abstract;
using WikiSite.DAL.Abstract;
using WikiSite.Entities;

namespace WikiSite.BLL.Default
{
	public class RolesBLO : IRolesBLL
	{
		private readonly IRolesDAL _rolesDal;

		public RolesBLO(IRolesDAL rolesDal)
		{
			if (rolesDal == null) throw new ArgumentNullException(nameof(rolesDal), "Roles DAL instance is null");
			_rolesDal = rolesDal;
		}

		/// <summary>
		/// Returns a role with certain id
		/// </summary>
		/// <param name="id">Guid of a role</param>
		/// <returns>DTO of a role</returns>
		public RoleDTO GetRole(Guid id)
		{
			if (id == Guid.Empty) throw new ArgumentNullException(nameof(id), "Id can't be empty");
			return _rolesDal.GetRole(id);
		}

		/// <summary>
		/// Returns a role by it's name
		/// </summary>
		/// <param name="name">name of a role</param>
		/// <returns>Role DTO</returns>
		public RoleDTO GetRole(string name)
		{
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name), "Name can't be empty");
			return _rolesDal.GetRole(name);
		}

		/// <summary>
		/// Returns all the roles that exist in db
		/// </summary>
		/// <returns>Roles sequence</returns>
		public IEnumerable<RoleDTO> GetRoles()
		{
			return _rolesDal.GetRoles().ToArray();
		}
	}
}