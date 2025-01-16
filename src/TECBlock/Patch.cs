using HarmonyLib;

using RomenH.Common;

namespace RomenH.TECBlock
{
	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch("Initialize")]
	public static class Db_Initialize_Patch
	{
		public static void Postfix()
		{
			BuildingUtils.AddBuildingToPlanScreen(TECTileConfig.ID, GameStrings.PlanMenuCategory.Utilities, subcategory: GameStrings.PlanMenuSubcategory.Utilities.Temperature);
			BuildingUtils.AddBuildingToTech(TECTileConfig.ID, GameStrings.Technology.Power.LowResistanceConductors);

			BuildingUtils.AddBuildingToPlanScreen(TEGTileConfig.ID, GameStrings.PlanMenuCategory.Power, subcategory: GameStrings.PlanMenuSubcategory.Power.Generators);
			BuildingUtils.AddBuildingToTech(TEGTileConfig.ID, GameStrings.Technology.Power.LowResistanceConductors);
		}
	}

	[HarmonyPatch(typeof(Localization), "Initialize")]
	public class Localization_Initialize_Patch
	{
		public static void Postfix()
		{
			StringUtils.LoadTranslations();
		}
	}
}
