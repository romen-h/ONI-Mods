using PeterHan.PLib;

using RomenMods.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomenMods.FestiveDecorMod
{
	public static class Mod
	{
		public static void OnLoad()
		{
			PUtil.InitLibrary();
			DebugUtils.PrintModInfo();

			FestivalManager.SetFestival(System.DateTime.Now);

			ModAssets.LoadAssets();
		}
	}
}
