using System;
using System.Threading;
using log4net;
using log4net.Config;
using Warehouse.PL.Console;
using WikiSite.DI.Provider;

namespace WikiSite.PL.Console
{
	class Program
	{
		private static readonly UserPLHelper UserPL;
		private static readonly ILog Log = LogManager.GetLogger(typeof (Program));

		static Program()
		{
			XmlConfigurator.Configure();
			try
			{
				BLOs.UsersBLO = Provider.UserBLO;
				BLOs.CredentialsBLO = Provider.CredentialsBLO;

				UserPL = new UserPLHelper(Log);
			}
			catch (Exception e)
			{
				Log.Fatal("Error on initializing layers", e);
				System.Console.WriteLine("Error on initializing database. Program will be closed.");
				System.Console.ReadKey();
				Thread.CurrentThread.Abort();
			}
		}

		static void Main(string[] args)
		{
			var menu = new MenuOrganizer("Выход");

			menu.Add(UserMenu, "Управление пользователями");

			menu.Go();
		}

		static string UserMenu()
		{
			var menu = new MenuOrganizer("В главное меню");

			menu.Add(UserPL.ShowUsers, "Показать пользователей");

			return menu.Go();
		}
	}
}
