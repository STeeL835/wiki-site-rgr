using System;
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
			if (rolesDal == null) throw new ArgumentNullException(nameof(rolesDal), "DAL instance is null");
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
	}
}