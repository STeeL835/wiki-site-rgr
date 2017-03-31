using System;

namespace WikiSite.PL.Console
{
	public static class PromptHelper
	{
		public static Guid PromptGuid(string hint, string errorHint = null)
		{
			Guid result;
			bool success;
			do
			{
				System.Console.Write(hint);
				success = Guid.TryParse(System.Console.ReadLine(), out result);
				if (!success && errorHint != null) System.Console.WriteLine(errorHint + Environment.NewLine);
			} while (!success);
			return result;
		}

		public static DateTime PromptDateTime(string hint, Predicate<DateTime> additionalCondition, string errorHint = null)
		{
			DateTime result;
			bool success;
			do
			{
				System.Console.Write(hint);
				success = DateTime.TryParse(System.Console.ReadLine(), out result);
				if ((!success || additionalCondition(result)) && errorHint != null)
					System.Console.WriteLine(errorHint + Environment.NewLine);
			} while (!success || additionalCondition(result));
			return result;
		}

		public static string PromptString(string hint, string errorHint = null)
		{
			string result;
			bool success;
			do
			{
				System.Console.Write(hint);
				result = System.Console.ReadLine();
				success = !string.IsNullOrWhiteSpace(result);
				if (!success && errorHint != null)
					System.Console.WriteLine(errorHint + Environment.NewLine);
			} while (!success);
			return result.Trim();
		}

		public static int PromptInt(string hint, Predicate<int> additionalCondition, string errorHint = null)
		{
			int result;
			bool success;
			do
			{
				System.Console.Write(hint);
				success = int.TryParse(System.Console.ReadLine(), out result);
				if ((!success || additionalCondition(result)) && errorHint != null)
					System.Console.WriteLine(errorHint + Environment.NewLine);
			} while (!success && additionalCondition(result));
			return result;
		}
	}
}