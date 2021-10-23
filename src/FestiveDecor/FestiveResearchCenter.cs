using HarmonyLib;

using RomenH.Common;

namespace RomenH.FestiveDecor
{
	/// <summary>
	/// Research Station
	/// </summary>
	[HarmonyPatch(typeof(ResearchCenterConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class ResearchCenterConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			Util.ReplaceAnim(__result, "research_center");
		}
	}
}
