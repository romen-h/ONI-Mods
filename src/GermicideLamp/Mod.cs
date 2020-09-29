using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using PeterHan.PLib;

using RomenMods.GermicideLampMod;

namespace RomenMods
{
	public static class Mod
	{
		internal static ModSettings Settings;

		public static void OnLoad()
		{
			PUtil.InitLibrary();
			RomenMods.Common.DebugUtils.PrintModInfo();

			Settings = PeterHan.PLib.Options.POptions.ReadSettings<ModSettings>();

			if (Settings == null)
			{
				Settings = new ModSettings();
				PeterHan.PLib.Options.POptions.WriteSettings<ModSettings>(Settings);
			}
		}
	}
}