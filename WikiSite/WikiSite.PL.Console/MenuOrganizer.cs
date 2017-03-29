using System;

namespace Warehouse.PL.Console
{
	public class MenuOrganizer
	{
		private const int D1 = 49;
		private const int Num1 = 97;
		private const int Max = 9;

		private readonly string[] _titles;
		private readonly Func<string>[] _actions;
		private bool _menuActive;
		private string _lastResult;

		public int ActionsAmount { get; private set; }
		public bool ClearOnChoosingEnabled { get; set; } = true;
		public bool ExitOnPerform { get; set; } = false;
		public string ExitTitle { get; set; }

		/// <summary>
		/// Provides menu functionality to console with reacting on 
		/// numeric key pressing (NumPad too)
		/// </summary>
		/// <param name="exitTitle">Name of action for returning back from menu (cancelling menu loop)</param>
		public MenuOrganizer(string exitTitle = "Exit")
		{
			_titles = new string[Max];
			_actions = new Func<string>[Max];
			ExitTitle = exitTitle;
		}

		/// <summary>
		/// Provides menu functionality to console with reacting on 
		/// numeric key pressing (NumPad too)
		/// </summary>
		/// <param name="clearScreen">Defines whether menu should clear the console (remove menu)
		/// before switching to menu option (true by default)</param>
		/// <param name="exitTitle">Name of action for returning back from menu (cancelling menu loop)</param>
		public MenuOrganizer(bool clearScreen, string exitTitle = "Exit") : this(exitTitle)
		{
			ClearOnChoosingEnabled = clearScreen;
		}

		private void PrintMenu()
		{
			for (int i = 0; i < ActionsAmount; i++)
			{
				System.Console.WriteLine($"{i + 1} {_titles[i]}");
			}
			System.Console.WriteLine($"0 {ExitTitle}");
		}

		private string CatchKeysPerform()
		{
			bool passedMenu = false;
			string result = null;
			do
			{
				int pressed = (int) System.Console.ReadKey(true).Key;

				if (pressed >= D1 && pressed < D1 + ActionsAmount)
				{
					result = RunAction(pressed - D1);
					passedMenu = true;
				}
				else if (pressed >= Num1 && pressed < Num1 + ActionsAmount)
				{
					result = RunAction(pressed - Num1);
					passedMenu = true;
				}

				else if (pressed == D1 - 1 || pressed == Num1 - 1)
				{
					_menuActive = false;
					passedMenu = true;
				}
			} while (!passedMenu);
			return result;
		}

		private string RunAction(int actionNumber)
		{
			if (ClearOnChoosingEnabled)
				System.Console.Clear();

			System.Console.WriteLine($"{_titles[actionNumber]}");
			System.Console.WriteLine();
			return _lastResult = _actions[actionNumber].Invoke();
		}

		private string ShowMenu()
		{
			string result;
			do
			{
				System.Console.Clear();
				if (_lastResult != null)
				{
					System.Console.WriteLine(_lastResult);
					System.Console.WriteLine();
					_lastResult = null;
				}
				PrintMenu();
				result = CatchKeysPerform();
			} while (_menuActive && !ExitOnPerform);
			return result;
		}

		/// <summary>
		/// Adds a menu option to menu. If amount of options > 9, it won't be added.
		/// Action method must return result string / null if it not needed.
		/// </summary>
		/// <param name="action">Action that will be done. Compatible with menu's Go() method.</param>
		/// <param name="title">Name of option without indexation, it's done by organizer.</param>
		/// <returns>Index number of option / -1 if not added.</returns>
		public int Add(Func<string> action, string title)
		{
			if (ActionsAmount == Max) return -1;

			_titles[ActionsAmount] = title;
			_actions[ActionsAmount] = action;
			ActionsAmount++;

			return ActionsAmount - 1;
		}

		/// <summary>
		/// Starts a menu loop
		/// </summary>
		/// <returns>null</returns>
		public string Go()
		{
			_menuActive = true;
			return ShowMenu();
		}
	}
}