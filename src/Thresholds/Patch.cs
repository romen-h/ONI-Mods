
using Epic.OnlineServices.Ecom;
using HarmonyLib;

using RomenH.Common;

namespace RomenH.Thresholds
{
	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch("Initialize")]
	public static class Db_Initialize_Patch
	{
		public static void Postfix()
		{
			BuildingUtils.AddBuildingToPlanScreen(ThresholdWallConfig.ID, GameStrings.PlanMenuCategory.Base, subcategory: GameStrings.PlanMenuSubcategory.Base.Doors);
			BuildingUtils.AddBuildingToPlanScreen(CautionThresholdWallConfig.ID, GameStrings.PlanMenuCategory.Base, subcategory: GameStrings.PlanMenuSubcategory.Base.Doors);
			BuildingUtils.AddBuildingToPlanScreen(MetalThresholdWallConfig.ID, GameStrings.PlanMenuCategory.Base, subcategory: GameStrings.PlanMenuSubcategory.Base.Doors);
			BuildingUtils.AddBuildingToPlanScreen(PlasticThresholdWallConfig.ID, GameStrings.PlanMenuCategory.Base, subcategory: GameStrings.PlanMenuSubcategory.Base.Doors);
			BuildingUtils.AddBuildingToPlanScreen(LogicThresholdWallConfig.ID, GameStrings.PlanMenuCategory.Base, subcategory: GameStrings.PlanMenuSubcategory.Base.Doors);
			BuildingUtils.AddBuildingToTech(CautionThresholdWallConfig.ID, GameStrings.Technology.Exosuits.HazardProtection);
			BuildingUtils.AddBuildingToTech(ThresholdWallConfig.ID, GameStrings.Technology.Decor.HomeLuxuries);
			BuildingUtils.AddBuildingToTech(MetalThresholdWallConfig.ID, GameStrings.Technology.SolidMaterial.RefinedRenovations);
			BuildingUtils.AddBuildingToTech(PlasticThresholdWallConfig.ID, GameStrings.Technology.Decor.HomeLuxuries);
			BuildingUtils.AddBuildingToTech(LogicThresholdWallConfig.ID, GameStrings.Technology.Computers.Multiplexing);
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

	[HarmonyPatch(typeof(OvercrowdingMonitor), "IsConfined")]
	public class OvercrowdingMonitor_IsConfimed_Patch
	{
		public static void Postfix(ref bool __result, OvercrowdingMonitor.Instance smi)
		{
			if (smi.cavity == null)
			{
				int cell = Grid.PosToCell(smi.gameObject);
				if (!Grid.IsSolidCell(cell) && !Grid.HasDoor[cell])
				{
					__result = false;
				}
			}
		}
	}
}
