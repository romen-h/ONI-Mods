using HarmonyLib;

using RomenH.Common;

namespace RomenH.Fans
{
	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch(nameof(Db.Initialize))]
	public class Db_Initialize_Patch
	{
		private static void Postfix()
		{
			BuildingUtils.AddBuildingToPlanScreen(FanConfig.ID, GameStrings.PlanMenuCategory.Ventilation, subcategory: GameStrings.PlanMenuSubcategory.Ventilation.Pumps);
			BuildingUtils.AddBuildingToPlanScreen(HighPressureFanConfig.ID, GameStrings.PlanMenuCategory.Ventilation, subcategory: GameStrings.PlanMenuSubcategory.Ventilation.Pumps);
			BuildingUtils.AddBuildingToPlanScreen(CompressorFanConfig.ID, GameStrings.PlanMenuCategory.Ventilation, subcategory: GameStrings.PlanMenuSubcategory.Ventilation.Pumps);
			BuildingUtils.AddBuildingToPlanScreen(LiquidTurbineConfig.ID, GameStrings.PlanMenuCategory.Plumbing, subcategory: GameStrings.PlanMenuSubcategory.Plumbing.Pumps);
			BuildingUtils.AddBuildingToPlanScreen(CompressorTurbineConfig.ID, GameStrings.PlanMenuCategory.Plumbing, subcategory: GameStrings.PlanMenuSubcategory.Plumbing.Pumps);

			BuildingUtils.AddBuildingToTech(FanConfig.ID, GameStrings.Technology.Gases.Ventilation);
			BuildingUtils.AddBuildingToTech(HighPressureFanConfig.ID, GameStrings.Technology.Gases.ImprovedVentilation);
			BuildingUtils.AddBuildingToTech(CompressorFanConfig.ID, GameStrings.Technology.Gases.HVAC);
			BuildingUtils.AddBuildingToTech(LiquidTurbineConfig.ID, GameStrings.Technology.Liquids.Plumbing);
			BuildingUtils.AddBuildingToTech(CompressorTurbineConfig.ID, GameStrings.Technology.Liquids.LiquidTuning);
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
