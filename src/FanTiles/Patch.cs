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
				GasFanConfig.Id,
				GasFanConfig.DisplayName,
				GasFanConfig.Description,
				GasFanConfig.Effect);

			StringUtils.AddBuildingStrings(
				HighPressureGasFan.Id,
				HighPressureGasFan.DisplayName,
				HighPressureGasFan.Description,
				HighPressureGasFan.Effect);

			StringUtils.AddBuildingStrings(
				CompressorGasFanConfig.Id,
				CompressorGasFanConfig.DisplayName,
				CompressorGasFanConfig.Description,
				CompressorGasFanConfig.Effect);

			StringUtils.AddBuildingStrings(
				CompressorLiquidFanConfig.Id,
				CompressorLiquidFanConfig.DisplayName,
				CompressorLiquidFanConfig.Description,
				CompressorLiquidFanConfig.Effect);

			StringUtils.AddBuildingStrings(
				LiquidFanConfig.Id,
				LiquidFanConfig.DisplayName,
				LiquidFanConfig.Description,
				LiquidFanConfig.Effect);
		}

		private static void Postfix()
		{
			BuildingUtils.AddBuildingToPlanScreen(GasFanConfig.Id, GameStrings.PlanMenuCategory.Ventilation);
			BuildingUtils.AddBuildingToPlanScreen(HighPressureGasFan.Id, GameStrings.PlanMenuCategory.Ventilation);
			BuildingUtils.AddBuildingToPlanScreen(CompressorGasFanConfig.Id, GameStrings.PlanMenuCategory.Ventilation);
			BuildingUtils.AddBuildingToPlanScreen(LiquidFanConfig.Id, GameStrings.PlanMenuCategory.Plumbing);
			BuildingUtils.AddBuildingToPlanScreen(CompressorLiquidFanConfig.Id, GameStrings.PlanMenuCategory.Plumbing);

			BuildingUtils.AddBuildingToTech(GasFanConfig.Id, GameStrings.Technology.Gases.Ventilation);
			BuildingUtils.AddBuildingToTech(HighPressureGasFan.Id, GameStrings.Technology.Gases.ImprovedVentilation);
			BuildingUtils.AddBuildingToTech(CompressorGasFanConfig.Id, GameStrings.Technology.Gases.HVAC);
			BuildingUtils.AddBuildingToTech(LiquidFanConfig.Id, GameStrings.Technology.Liquids.Plumbing);
			BuildingUtils.AddBuildingToTech(CompressorLiquidFanConfig.Id, GameStrings.Technology.Liquids.LiquidTuning);
		}
	}
}
