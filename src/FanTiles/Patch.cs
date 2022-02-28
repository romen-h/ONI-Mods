using HarmonyLib;

using RomenH.Common;

namespace Fans
{
	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch(nameof(Db.Initialize))]
	public class Db_Initialize_Patch
	{
		private static void Prefix()
		{
			Debug.Log("FanTiles: Adding strings...");

			StringUtils.AddBuildingStrings(
				FanConfig.ID,
				FanConfig.Name,
				FanConfig.Desc,
				FanConfig.Effect);

			StringUtils.AddBuildingStrings(
				HighPressureFanConfig.ID,
				HighPressureFanConfig.Name,
				HighPressureFanConfig.Desc,
				HighPressureFanConfig.Effect);

			StringUtils.AddBuildingStrings(
				CompressorFanConfig.ID,
				CompressorFanConfig.Name,
				CompressorFanConfig.Desc,
				CompressorFanConfig.Effect);

			StringUtils.AddBuildingStrings(
				LiquidTurbineConfig.ID,
				LiquidTurbineConfig.Name,
				LiquidTurbineConfig.Desc,
				LiquidTurbineConfig.Effect);

			StringUtils.AddBuildingStrings(
				CompressorTurbineConfig.ID,
				CompressorTurbineConfig.Name,
				CompressorTurbineConfig.Desc,
				CompressorTurbineConfig.Effect);

			StringUtils.ExportTranslationTemplates();
		}

		private static void Postfix()
		{
			BuildingUtils.AddBuildingToPlanScreen(FanConfig.ID, GameStrings.PlanMenuCategory.Ventilation);
			BuildingUtils.AddBuildingToPlanScreen(HighPressureFanConfig.ID, GameStrings.PlanMenuCategory.Ventilation);
			BuildingUtils.AddBuildingToPlanScreen(CompressorFanConfig.ID, GameStrings.PlanMenuCategory.Ventilation);
			BuildingUtils.AddBuildingToPlanScreen(LiquidTurbineConfig.ID, GameStrings.PlanMenuCategory.Plumbing);
			BuildingUtils.AddBuildingToPlanScreen(CompressorTurbineConfig.ID, GameStrings.PlanMenuCategory.Plumbing);

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
