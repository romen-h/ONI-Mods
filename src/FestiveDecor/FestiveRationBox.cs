using HarmonyLib;

namespace RomenH.FestiveDecor
{
	/// <summary>
	/// Ration Box
	/// </summary>
	[HarmonyPatch(typeof(RationBoxConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class RationBoxConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			Util.ReplaceAnim(__result, "rationbox");
		}
	}
}
