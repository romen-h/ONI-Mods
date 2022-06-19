using System;
using System.Collections.Generic;
using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using RomenH.Common;

namespace RomenH.PlasticUtilities
{
	public class Mod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			ModCommon.Init("Plastic Utilities");
			PUtil.InitLibrary();

			var options = new POptions();
			options.RegisterOptions(this, typeof(ModSettings));

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
