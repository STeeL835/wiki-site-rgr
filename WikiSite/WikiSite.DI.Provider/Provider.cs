using System;
using System.Configuration;
using WikiSite.BLL.Abstract;
using WikiSite.BLL.Default;
using WikiSite.DAL.Abstract;
using WikiSite.DAL.SQL;

namespace WikiSite.DI.Provider
{
    public class Provider
    {
		public static IUsersDAL UsersDAO { get; private set; }
		public static IUserCredentialsDAL CredentialsDAO { get; private set; }
		public static IRolesDAL RolesDAO { get; private set; }

		public static IUsersBLL UsersBLO { get; private set; }
		public static IRolesBLL RolesBLO { get; private set; }

		static Provider()
		{
			string dal;
			try
			{
				dal = ConfigurationManager.AppSettings["DAL"];
			}
			catch (Exception e)
			{
				throw new ApplicationException("Incorrect configuration file. DAL key not found", e);
			}

			string bll;
			try
			{
				bll = ConfigurationManager.AppSettings["BLL"];
			}
			catch (Exception e)
			{
				throw new ApplicationException("Incorrect configuration file. BLL key not found", e);
			}

			SetCurrentDAL(dal);
			SetCurrentBLL(bll);
		}

		private static void SetCurrentDAL(string configValue)
		{
			switch (configValue.ToUpperInvariant())
			{
				case "SQL":
					UsersDAO = new WikiSite.DAL.SQL.UsersDAO();
					CredentialsDAO = new WikiSite.DAL.SQL.UserCredentialsDAO();
					RolesDAO = new WikiSite.DAL.SQL.RolesDAO();
					break;
				default:
					throw new ApplicationException($"Incorrect configuration file. Inconsistent [DAL] key value: {configValue}");
			}
		}

		private static void SetCurrentBLL(string configValue)
		{
			switch (configValue.ToUpperInvariant())
			{
				case "DEFAULT":
					UsersBLO = new UsersBLO(UsersDAO, CredentialsDAO);
					RolesBLO = new RolesBLO(RolesDAO);
					break;
				default:
					throw new ApplicationException($"Incorrect configuration file. Inconsistent [BLL] key value: {configValue}");
			}
		}
	}
}
