using System.Collections.Generic;

using HarmonyLib;

using KMod;

using PeterHan.PLib.Core;
using PeterHan.PLib.Options;

using RomenH.Common;

namespace RomenH.FestiveDecor
{
	public class Mod : UserMod2
	{
		public static IDictionary<string, object> Registry
		{ get; private set; }

		public override void OnLoad(Harmony harmony)
		{
			PUtil.InitLibrary(true);

			POptions popt = new POptions();

			popt.RegisterOptions(this, typeof(ModSettings));

			System.DateTime date = System.DateTime.Now;
			if (ModSettings.Instance.UseOverrideDate)
			{
				try
				{
					date = new System.DateTime(date.Year, ModSettings.Instance.OverrideMonth, ModSettings.Instance.OverrideDayOfMonth);
				}
				catch
				{
					Debug.Log("FestiveDecor: Override date in settings file is invalid. Using current date.");
					date = System.DateTime.Now;
				}
			}

			Registry = RomenHRegistry.Init();

			Festival festival = FestivalManager.GetFestivalForDate(date);
			FestivalManager.SetFestival(festival);

			ModAssets.LoadAssets();

			base.OnLoad(harmony);
		}

		public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
		{
			foreach (var mod in mods)
			{
				if (mod.staticID == ModModules.LUTModuleID && ModSettings.Instance.EnableColorCorrection)
				{
					ModModules.EnableLUTModule(mod);
				}
			}
		}
	}
}
