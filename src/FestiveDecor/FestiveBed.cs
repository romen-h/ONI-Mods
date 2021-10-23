using HarmonyLib;

namespace RomenH.FestiveDecor
{
	/// <summary>
	/// Basic Bed
	/// </summary>
	[HarmonyPatch(typeof(BedConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class BedConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			Util.ReplaceAnim(__result, "bedlg");
		}
	}

	/// <summary>
	/// Plastic Bed
	/// </summary>
	[HarmonyPatch(typeof(LuxuryBedConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class LuxuryBedConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			Util.ReplaceAnim(__result, "elegantbed");
		}
	}
}
