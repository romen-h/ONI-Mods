using System.Collections.Generic;
using System.IO;
using System.Reflection;

using HarmonyLib;

using KMod;

using PeterHan.PLib.Core;
using PeterHan.PLib.Options;

using RomenH.Common;

namespace RomenH.LUTNotIncluded
{
	public class Mod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			ModCommon.Init("LUT Not Included");
			PUtil.InitLibrary();

			var options = new POptions();
			options.RegisterOptions(this, typeof(ModSettings));

			ModAssets.LoadAssets();

			base.OnLoad(harmony);
		}
	}
}
