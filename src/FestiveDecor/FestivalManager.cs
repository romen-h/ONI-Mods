namespace RomenH.FestiveDecor
{
	/// <summary>
	/// Enumerates the festivals supported by this mod.
	/// </summary>
	public enum Festival : int
	{
		None,
		SpringCandy,
		Halloween,
		WinterHolidays
	}

	/// <summary>
	/// A static class to access the current festival.
	/// </summary>
	public static class FestivalManager
	{
		/// <summary>
		/// Gets the currently active festival.
		/// </summary>
		public static Festival CurrentFestival
		{ get; private set; }

		/// <summary>
		/// Gets the affix string used for the currently active festival.
		/// </summary>
		public static string FestivalAnimAffix
		{ get; private set; }

		internal static Festival GetFestivalForDate(System.DateTime date)
		{
			// Easter March 22 - April 25
			if (FestivalEnabled(Festival.SpringCandy) && ((date.Month == 3 && date.Day >= 22) || (date.Month == 4 && date.Day <= 25)))
			{
				return Festival.SpringCandy;
			}
			// Halloween in Oct & Nov 1st
			else if (FestivalEnabled(Festival.Halloween) && (date.Month == 10 || (date.Month == 11 && date.Day <= 1)))
			{
				return Festival.Halloween;
			}
			// Winter Holidays in Dec
			else if (FestivalEnabled(Festival.WinterHolidays) && date.Month == 12)
			{
				return Festival.WinterHolidays;
			}
			else
			{
				return Festival.None;
			}
		}

		/// <summary>
		/// Sets the current festival according to the given DateTime.
		/// </summary>
		internal static void SetFestival(Festival festival)
		{
			CurrentFestival = festival;
			FestivalAnimAffix = GetAnimAffix(CurrentFestival);

			Debug.Log($"FestiveDecor: Current Festival = {CurrentFestival}");
		}

		private static bool FestivalEnabled(Festival f)
		{
			switch (f)
			{
				case Festival.None:
					return true;
				case Festival.Halloween:
					return ModSettings.Instance.EnableHalloween;
				default:
					return false;
			}
		}

		private static string GetAnimAffix(Festival f)
		{
			switch (f)
			{
				case Festival.SpringCandy:
					return "spring";

				case Festival.Halloween:
					return "halloween";

				case Festival.WinterHolidays:
					return "winter";

				default:
					return null;
			}
		}
	}
}
