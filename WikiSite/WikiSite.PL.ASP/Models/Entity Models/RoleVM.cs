using System;
using System.Configuration;
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

		public static RoleVM GetRole(RolesEnum value)
		{
			switch (value)
			{
				case RolesEnum.User:
					return GetRole(Guid.Parse(ConfigurationManager.AppSettings["UserRoleId"]));
				case RolesEnum.Moderator:
					return GetRole(Guid.Parse(ConfigurationManager.AppSettings["ModeratorRoleId"]));
				case RolesEnum.Admin:
					return GetRole(Guid.Parse(ConfigurationManager.AppSettings["AdminRoleId"]));
				default:
					throw new ArgumentOutOfRangeException(nameof(value), value, null);
			}
		}

		public static RolesEnum GetRoleEnum(Guid id)
		{
			if (id == Guid.Parse(ConfigurationManager.AppSettings["UserRoleId"]))
				return RolesEnum.User;
			if (id == Guid.Parse(ConfigurationManager.AppSettings["ModeratorRoleId"]))
				return RolesEnum.Moderator;
			if (id == Guid.Parse(ConfigurationManager.AppSettings["AdminRoleId"]))
				return RolesEnum.Admin;
			
				throw new ArgumentOutOfRangeException(nameof(id), id, null);
		}

		#endregion

		public enum RolesEnum
		{
			//[Display(Name = "Обычный пользователь")]  // Parser doesn't use unicode, but latin char pages
			User,
			//[Display(Name = "Модератор")]
			Moderator,
			//[Display(Name = "Администратор")]
			Admin,
		}
	}
}