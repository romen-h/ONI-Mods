using HarmonyLib;

using RomenH.Common;

namespace RomenH.FestiveDecor
{
	/// <summary>
	/// Small Sculpture (1x2)
	/// </summary>
	[HarmonyPatch(typeof(SmallSculptureConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class SmallSculptureConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			Util.ReplaceAnim(__result, "sculpture_1x2");
		}
	}

	/// <summary>
	/// Large Sculpture (1x3)
	/// </summary>
	[HarmonyPatch(typeof(SculptureConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class SculptureConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			Util.ReplaceAnim(__result, "sculpture");
		}
	}

	/// <summary>
	/// Metal Sculpture
	/// </summary>
	[HarmonyPatch(typeof(MetalSculptureConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class MetalSculptureConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			Util.ReplaceAnim(__result, "sculpture_metal");
		}
	}

	/// <summary>
	/// Marble Sculpture
	/// </summary>
	[HarmonyPatch(typeof(MarbleSculptureConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class MarbleSculptureConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			Util.ReplaceAnim(__result, "sculpture_marble");
		}
	}
}
