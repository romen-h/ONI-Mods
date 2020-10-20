using Harmony;
using UnityEngine;

namespace RomenMods.FestiveDecorMod
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
			if (FestivalManager.CurrentFestival != Festival.None)
			{
				KAnimFile anim = Assets.GetAnim($"ceilinglight_{FestivalManager.FestivalAnimAffix}_kanim");
				if (anim != null) __result.AnimFiles = new KAnimFile[1] { anim };
			}
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
			if (FestivalManager.CurrentFestival != Festival.None)
			{
				KAnimFile anim = Assets.GetAnim($"floorlamp_{FestivalManager.FestivalAnimAffix}_kanim");
				if (anim != null) __result.AnimFiles = new KAnimFile[1] { anim };
			}
		}
	}
}
