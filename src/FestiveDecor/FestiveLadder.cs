using HarmonyLib;

using RomenH.Common;

using UnityEngine;

namespace RomenH.FestiveDecor
{
	/// <summary>
	/// Ladder
	/// </summary>
	[HarmonyPatch(typeof(LadderConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class LadderConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			Util.ReplaceAnim(__result, "ladder");
		}
	}

	/// <summary>
	/// Ladder
	/// </summary>
	[HarmonyPatch(typeof(LadderConfig))]
	[HarmonyPatch("ConfigureBuildingTemplate")]
	public static class LadderConfig_ConfigureBuildingTemplate_Patch
	{
		public static void Postfix(GameObject go)
		{
			if (FestivalManager.CurrentFestival == Festival.Halloween)
			{
				var glow = go.AddComponent<GlowInTheDark>();
				glow.noGlowAnim = Assets.GetAnim("ladder_halloween_kanim");
				glow.glowAnim = Assets.GetAnim("ladder_halloween_glow_kanim");
			}
		}
	}

	/// <summary>
	/// Plastic Ladder
	/// </summary>
	[HarmonyPatch(typeof(LadderFastConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class LadderFastConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			Util.ReplaceAnim(__result, "ladder_plastic");
		}
	}
}
