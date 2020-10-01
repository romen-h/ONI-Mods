using PeterHan.PLib;

using RomenMods.StirlingEngineMod;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RomenMods
{
	public static class Mod
	{
		internal static ModSettings Config;

		public static void OnLoad()
		{
			PUtil.InitLibrary();
			RomenMods.Common.DebugUtils.PrintModInfo();

			Config = PeterHan.PLib.Options.POptions.ReadSettings<ModSettings>();

			if (Config == null)
			{
				Config = new ModSettings();
				PeterHan.PLib.Options.POptions.WriteSettings<ModSettings>(Config);
			}
		}
	}
}
