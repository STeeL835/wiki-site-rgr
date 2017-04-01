using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Warehouse.PL.Console;
using WikiSite.BLL.Abstract;
using WikiSite.PL.Console.Models;

namespace WikiSite.PL.Console
{
	internal class UserPLHelper
	{
		protected IUsersBLL BLL;
		protected ILog Log;

		public UserPLHelper(ILog log)
		{
			BLL = BLOs.UsersBLO;
			if (log == null) throw new ArgumentNullException(nameof(log), "log is null");
			Log = log;
		}

		public string ShowUsers()
		{
			System.Console.WriteLine("Загрузка..{0}", Environment.NewLine);
			var items = GetEnumeratedUsers();
			DisplayAll(items);
			System.Console.WriteLine(new string('=', System.Console.BufferWidth));
			System.Console.WriteLine("\tВсего {0} пользователей {1}", items.Count, Environment.NewLine);

			var menu = new MenuOrganizer("Назад") {ClearOnShowingEnabled = false};
			menu.Add(() => "Nope", "Подробности о пользователе");
			menu.Go();

			return null;
		}

		public void DisplayAll(Dictionary<int, UserVM> users)
		{
			if (users.Count == 0) return;

			var counterWidth = users.Count.ToString().Length;
			var nameWidth = users.Max(dto => dto.Value.Nickname.Length);
			var smallIdWidth = users.Max(dto => dto.Value.SmallId.ToString().Length);

			System.Console.WriteLine("(Номер | Короткий Id | Никнейм){0}", Environment.NewLine);

			foreach (var user in users)
			{
				System.Console.Write(
					$"{user.Key.ToString().PadLeft(counterWidth)}" +
					$" | {user.Value.SmallId.ToString().PadRight(smallIdWidth)}" +
					$" | {user.Value.Nickname.PadRight(nameWidth)} | "); // table output
				System.Console.WriteLine();
			}
		}

		public Dictionary<int, UserVM> GetEnumeratedUsers()
		{
			int counter = 1;
			return BLL.GetUsers().ToDictionary(user => counter++, dto => (UserVM)dto);
		}
	}
}
