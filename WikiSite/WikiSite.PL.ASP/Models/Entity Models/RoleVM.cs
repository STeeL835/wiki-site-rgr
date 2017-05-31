using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using WikiSite.BLL.Abstract;
using WikiSite.DI.Provider;
using WikiSite.Entities;

namespace WikiSite.PL.ASP.Models
{
	public class RoleVM
	{
		#region Instance

		public Guid Id { get; private set; }
		public string Name { get; private set; }

		public static explicit operator RoleVM(RoleDTO dto) => new RoleVM {Name = dto.Name, Id = dto.Id};

		#endregion

		#region Static

		private static IRolesBLL _bll;

		static RoleVM()
		{
			_bll = Provider.RolesBLO;
		}

		public static RoleVM GetRole(Guid id)
		{
			return (RoleVM) _bll.GetRole(id);
		}

		public static RoleVM GetRole(string name)
		{
			return (RoleVM) _bll.GetRole(name);
		}

		public static RoleVM[] GetRoles()
		{
			return _bll.GetRoles().Select(dto => (RoleVM) dto).ToArray();
		}

		public static bool UserIsInRole(string role, Guid userId)
		{
			var user = UserVM.GetUser(userId);
			var rolevm = GetRole(user.RoleId);
			return role == rolevm.Name.ToLowerInvariant();
		}

		#endregion
	}
}