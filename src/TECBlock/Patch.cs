using HarmonyLib;
using RomenH.Common;

namespace RomenH.TECBlock
{
	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch("Initialize")]
	public static class Db_Initialize_Patch
	{
		public static void Prefix()
		{
			Debug.Log("TEC Tile: Adding strings...");

			StringUtils.AddBuildingStrings(
				TECTileConfig.ID,
				TECTileConfig.Name,
				TECTileConfig.Desc,
				TECTileConfig.Effect);
		}

		public static void Postfix()
		{
			BuildingUtils.AddBuildingToPlanScreen(TECTileConfig.ID, GameStrings.PlanMenuCategory.Utilities);
			BuildingUtils.AddBuildingToTech(TECTileConfig.ID, GameStrings.Technology.Power.LowResistanceConductors);
		}
	}
}
