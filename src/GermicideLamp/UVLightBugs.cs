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
			if (ModSettings.Instance.EnableUVSunBugs)
			{
				ExtentsHelpers.CenteredUVExtents(ModSettings.Instance.SunBugRange, 1, 1, out int left, out int width, out int bottom, out int height);

				var lamp = __result.AddOrGet<GermicideLamp>();
				lamp.alwaysOn = true;
				lamp.isMobile = true;
				lamp.aoeLeft = left; // -2
				lamp.aoeWidth = width; // 5
				lamp.aoeBottom = bottom; // -2
				lamp.aoeHeight = height; // 5
				lamp.basePower = ModSettings.Instance.SunBugStrength;
			}
		}
	}

	[HarmonyPatch(typeof(LightBugPurpleConfig))]
	[HarmonyPatch("CreatePrefab")]
	public static class LightBugPurpleConfig_CreatePrefab_Patch
	{
		public static void Postfix(GameObject __result)
		{
			if (ModSettings.Instance.EnableUVRoyalBugs)
			{
				ExtentsHelpers.CenteredUVExtents(ModSettings.Instance.RoyalBugRange, 1, 1, out int left, out int width, out int bottom, out int height);

				var lamp = __result.AddOrGet<GermicideLamp>();
				lamp.alwaysOn = true;
				lamp.isMobile = true;
				lamp.aoeLeft = left; // -3
				lamp.aoeWidth = width; // 7
				lamp.aoeBottom = bottom; // -3
				lamp.aoeHeight = height; // 7
				lamp.basePower = ModSettings.Instance.RoyalBugStrength;
			}
		}
	}
}
