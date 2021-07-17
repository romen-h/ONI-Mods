using HarmonyLib;
using UnityEngine;

namespace RomenH.GermicideLamp
{
	[HarmonyPatch(typeof(LightBugOrangeConfig))]
	[HarmonyPatch("CreatePrefab")]
	public static class LightBugOrangeConfig_CreatePrefab_Patch
	{
		public static void Postfix(GameObject __result)
		{
			if (Mod.Settings.EnableUVSunBugs)
			{
				var lamp = __result.AddOrGet<GermicideLamp>();
				lamp.alwaysOn = true;
				lamp.mobileLamp = true;
				lamp.aoeLeft = -2;
				lamp.aoeWidth = 5;
				lamp.aoeBottom = -2;
				lamp.aoeHeight = 5;
				lamp.strength = Mod.Settings.SunBugStrength;
			}
		}
	}

	[HarmonyPatch(typeof(LightBugPurpleConfig))]
	[HarmonyPatch("CreatePrefab")]
	public static class LightBugPurpleConfig_CreatePrefab_Patch
	{
		public static void Postfix(GameObject __result)
		{
			if (Mod.Settings.EnableUVRoyalBugs)
			{
				var lamp = __result.AddOrGet<GermicideLamp>();
				lamp.alwaysOn = true;
				lamp.mobileLamp = true;
				lamp.aoeLeft = -3;
				lamp.aoeWidth = 7;
				lamp.aoeBottom = -3;
				lamp.aoeHeight = 7;
				lamp.strength = Mod.Settings.RoyalBugStrength;
			}
		}
	}
}
