using JetBrains.Annotations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomenMods.FestiveDecorMod
{
	/// <summary>
	/// Enumerates the festivals supported by this mod.
	/// </summary>
	public enum Festival : int
	{
		None = 0,
		Easter = 1,
		Halloween = 2,
		WinterHolidays = 3
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

		/// <summary>
		/// Sets the current festival according to the given DateTime.
		/// </summary>
		internal static void SetFestival(System.DateTime date)
		{
			PeterHan.PLib.PSharedData.PutData("FestiveDecor.Constants.iSpring", (int)Festival.Easter);
			PeterHan.PLib.PSharedData.PutData("FestiveDecor.Constants.iHalloween", (int)Festival.Halloween);
			PeterHan.PLib.PSharedData.PutData("FestiveDecor.Constants.iWinterHolidays", (int)Festival.WinterHolidays);

			// Easter March 22 - April 25
			if (FestivalEnabled(Festival.Easter) && ((date.Month == 3 && date.Day >= 22) || (date.Month == 4 && date.Day <= 25)))
			{
				CurrentFestival = Festival.Easter;
			}
			// Halloween in Oct & Nov 1st
			else if (FestivalEnabled(Festival.Halloween) && (date.Month == 10 || (date.Month == 11 && date.Day <= 1)))
			{
				CurrentFestival = Festival.Halloween;
			}
			// Winter Holidays in Dec
			else if (FestivalEnabled(Festival.WinterHolidays) && date.Month == 12)
			{
				CurrentFestival = Festival.WinterHolidays;
			}
			else
			{
				CurrentFestival = Festival.None;
			}

			PeterHan.PLib.PSharedData.PutData("FestiveDecor.iCurrentFestival", (int)CurrentFestival);

			FestivalAnimAffix = GetAnimAffix(CurrentFestival);
		}

		private static bool FestivalEnabled(Festival f)
		{
			switch (f)
			{
				case Festival.None:
					return true;
				case Festival.Halloween:
					return Mod.Settings.EnableHalloween;
				default:
					return false;
			}
		}

		private static string GetAnimAffix(Festival f)
		{
			switch (f)
			{
				case Festival.Easter:
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
