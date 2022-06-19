using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using RomenH.Common;

namespace RomenH.Thresholds
{
	public class Mod : UserMod2
	{
		internal static string ModFolder
		{ get; private set; }

		public override void OnLoad(Harmony harmony)
		{
			ModCommon.Init("Threshold Walls");
			PUtil.InitLibrary();

			var options = new POptions();
			options.RegisterOptions(this, typeof(ModSettings));

			ModFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			base.OnLoad(harmony);
		}

		public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
		{
			if (Type.GetType("DrywallHidesPipes.DrywallPatch, DrywallHidesPipes", false, false) != null)
			{
				ModSettings.Instance.HidePipesAndWires = true;
			}

			base.OnAllModsLoaded(harmony, mods);
		}
	}
}
