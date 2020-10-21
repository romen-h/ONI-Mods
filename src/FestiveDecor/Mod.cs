using PeterHan.PLib;
using RomenMods.Common;

namespace RomenMods.FestiveDecorMod
{
	public static class Mod
	{
		public static ModSettings Settings;

		public static void OnLoad()
		{
			PUtil.InitLibrary();
			DebugUtils.PrintModInfo();

			Settings = PeterHan.PLib.Options.POptions.ReadSettings<ModSettings>();
			if (Settings == null)
			{
				Settings = new ModSettings();
				
				PeterHan.PLib.Options.POptions.WriteSettings<ModSettings>(Settings);
			}

			System.DateTime date = System.DateTime.Now;
			if (Settings.UseOverrideDate)
			{
				try
				{
					date = new System.DateTime(date.Year, Settings.OverrideMonth, Settings.OverrideDayOfMonth);
				}
				catch
				{
					Debug.Log("FestiveDecor: Override date in settings file is invalid. Using current date.");
					date = System.DateTime.Now;
				}
			}

			FestivalManager.SetFestival(date);

			ModAssets.LoadAssets();
		}
	}
}
