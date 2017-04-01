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
		public static IUsersDAL UserDAO { get; private set; }
		public static IUserCredentialsDAL CredentialsDAO { get; private set; }

		public static IUsersBLL UserBLO { get; private set; }
		public static IUserCredentialsBLL CredentialsBLO { get; private set; }

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
					UserDAO = new WikiSite.DAL.SQL.UsersDAO();
					CredentialsDAO = new WikiSite.DAL.SQL.UserCredentialsDAO();
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
					UserBLO = new UsersBLO(UserDAO);
					CredentialsBLO = new UserCredentialsBLO(CredentialsDAO);
					break;
				default:
					throw new ApplicationException($"Incorrect configuration file. Inconsistent [BLL] key value: {configValue}");
			}
		}
	}
}
