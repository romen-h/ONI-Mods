
using HarmonyLib;

using RomenH.Common;

namespace RomenH.Thresholds
{
	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch("Initialize")]
	public static class Db_Initialize_Patch
	{
		public static void Prefix()
		{
			Debug.Log("Threshold Walls: Adding strings...");

			StringUtils.AddBuildingStrings(
				ThresholdWallConfig.ID,
				ThresholdWallConfig.Name,
				ThresholdWallConfig.Desc,
				ThresholdWallConfig.Effect);
			StringUtils.AddBuildingStrings(
				CautionThresholdWallConfig.ID,
				CautionThresholdWallConfig.Name,
				CautionThresholdWallConfig.Desc,
				CautionThresholdWallConfig.Effect);
			StringUtils.AddBuildingStrings(
				MetalThresholdWallConfig.ID,
				MetalThresholdWallConfig.Name,
				MetalThresholdWallConfig.Desc,
				MetalThresholdWallConfig.Effect);
			StringUtils.AddBuildingStrings(
				PlasticThresholdWallConfig.ID,
				PlasticThresholdWallConfig.Name,
				PlasticThresholdWallConfig.Desc,
				PlasticThresholdWallConfig.Effect);

			StringUtils.ExportTranslationTemplates();
		}

		public static void Postfix()
		{
			BuildingUtils.AddBuildingToPlanScreen(ThresholdWallConfig.ID, GameStrings.PlanMenuCategory.Utilities);
			BuildingUtils.AddBuildingToPlanScreen(CautionThresholdWallConfig.ID, GameStrings.PlanMenuCategory.Utilities);
			BuildingUtils.AddBuildingToPlanScreen(MetalThresholdWallConfig.ID, GameStrings.PlanMenuCategory.Utilities);
			BuildingUtils.AddBuildingToPlanScreen(PlasticThresholdWallConfig.ID, GameStrings.PlanMenuCategory.Utilities);
			BuildingUtils.AddBuildingToTech(CautionThresholdWallConfig.ID, GameStrings.Technology.Exosuits.HazardProtection);
			BuildingUtils.AddBuildingToTech(ThresholdWallConfig.ID, GameStrings.Technology.Decor.HomeLuxuries);
			BuildingUtils.AddBuildingToTech(MetalThresholdWallConfig.ID, GameStrings.Technology.SolidMaterial.RefinedRenovations);
			BuildingUtils.AddBuildingToTech(PlasticThresholdWallConfig.ID, GameStrings.Technology.Decor.HomeLuxuries);
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
