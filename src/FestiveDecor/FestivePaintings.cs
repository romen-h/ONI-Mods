using HarmonyLib;

using RomenH.Common;

namespace RomenH.FestiveDecor
{
	/// <summary>
	/// Painting (2x2)
	/// </summary>
	[HarmonyPatch(typeof(CanvasConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class CanvasConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			Util.ReplaceAnim(__result, "painting");
		}
	}

	/// <summary>
	/// Painting (2x3)
	/// </summary>
	[HarmonyPatch(typeof(CanvasTallConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class CanvasTallConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			Util.ReplaceAnim(__result, "painting_tall");
		}
	}

	/// <summary>
	/// Painting (3x2)
	/// </summary>
	[HarmonyPatch(typeof(CanvasWideConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class CanvasWideConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			Util.ReplaceAnim(__result, "painting_wide");
		}
	}
}
