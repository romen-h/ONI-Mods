using PeterHan.PLib;

using RomenMods.GermicideLampMod;

using UnityEngine;

namespace RomenMods
{
	public static class Mod
	{
		internal static readonly ModSettings DefaultSettings = new ModSettings();
		internal static ModSettings Settings;

		public static void OnLoad()
		{
			PUtil.InitLibrary();

			Settings = PeterHan.PLib.Options.POptions.ReadSettings<ModSettings>();
			if (Settings == null)
			{
				Settings = new ModSettings();

				PeterHan.PLib.Options.POptions.WriteSettings(Settings);
			}

			PeterHan.PLib.Options.POptions.RegisterOptions(typeof(ModSettings));
		}
	}
}
