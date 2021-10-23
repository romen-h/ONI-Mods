using HarmonyLib;

using RomenH.Common;

using UnityEngine;

namespace RomenH.FestiveDecor
{
	/// <summary>
	/// Ceiling Light
	/// </summary>
	[HarmonyPatch(typeof(CeilingLightConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class CeilingLightConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			Util.ReplaceAnim(__result, "ceilinglight");
		}
	}

	/// <summary>
	/// Ceiling Light Tweaks
	/// </summary>
	[HarmonyPatch(typeof(CeilingLightConfig))]
	[HarmonyPatch("DoPostConfigureComplete")]
	public static class CeilingLightConfig_DoPostConfigureComplete_Patch
	{
		public static void Postfix(GameObject __0)
		{
			if (FestivalManager.CurrentFestival == Festival.Halloween)
			{
				// Set to green colour and offset to look good
				var light = __0.GetComponent<Light2D>();
				light.Color = new Color(0.71f * 2f, 0.90f * 2f, 0.36f * 2f);
				light.Offset = new Vector2(0.05f, 0f);
			}
		}
	}

	/// <summary>
	/// Floor Lamp
	/// </summary>
	[HarmonyPatch(typeof(FloorLampConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class FloorLampConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			Util.ReplaceAnim(__result, "floorlamp");
		}
	}
}
