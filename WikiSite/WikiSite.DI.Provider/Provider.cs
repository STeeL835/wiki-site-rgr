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
        public static IArticlesDAL ArticlesDAO { get; private set; } 
        public static IArticleVersionsDAL ArticleVersionsDAO { get; private set; }
        public static IArticleContentsDAL ArticleContentsDAO { get; private set; }

		public static IUsersBLL UsersBLO { get; private set; }
		public static IRolesBLL RolesBLO { get; private set; }
        public static IArticlesBLL ArticlesBLO { get; private set; }

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
                    ArticlesDAO = new WikiSite.DAL.SQL.ArticlesDAO();
                    ArticleVersionsDAO = new WikiSite.DAL.SQL.ArticleVersionsDAO();
                    ArticleContentsDAO = new WikiSite.DAL.SQL.ArticleContentsDAO();
					break;
				default:
					throw new ApplicationException($"Incorrect configuration file. Inconsistent [DAL] key value: {configValue}.");
			}
		}

		private static void SetCurrentBLL(string configValue)
		{
			switch (configValue.ToUpperInvariant())
			{
				case "DEFAULT":
					UsersBLO = new WikiSite.BLL.Default.UsersBLO(UsersDAO, CredentialsDAO);
					RolesBLO = new WikiSite.BLL.Default.RolesBLO(RolesDAO);
                    ArticlesBLO = new WikiSite.BLL.Default.ArticleBLO(ArticlesDAO, ArticleVersionsDAO, ArticleContentsDAO);
					break;
				default:
					throw new ApplicationException($"Incorrect configuration file. Inconsistent [BLL] key value: {configValue}.");
			}
		}
	}
}
