using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;

using RomenH.Common;

namespace RomenH.PlasticUtilities
{
	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch("Initialize")]
	public static class Db_Initialize_Patch
	{
		public static void Postfix()
		{
			Debug.Log("Plastic Utilities: Adding buildings to game...");

			BuildingUtils.AddBuildingToPlanScreen(PlasticBubbleWallConfig.ID, GameStrings.PlanMenuCategory.Base, subcategory: GameStrings.PlanMenuSubcategory.Base.Tiles);
			BuildingUtils.AddBuildingToTech(PlasticBubbleWallConfig.ID, GameStrings.Technology.Decor.HomeLuxuries);

			BuildingUtils.AddBuildingToPlanScreen(PlasticCheckerWallConfig.ID, GameStrings.PlanMenuCategory.Base, subcategory: GameStrings.PlanMenuSubcategory.Base.Tiles);
			BuildingUtils.AddBuildingToTech(PlasticCheckerWallConfig.ID, GameStrings.Technology.Decor.HomeLuxuries);

			BuildingUtils.AddBuildingToPlanScreen(PlasticLiquidConduitConfig.ID, GameStrings.PlanMenuCategory.Plumbing, subcategory: GameStrings.PlanMenuSubcategory.Plumbing.Pipes);
			BuildingUtils.AddBuildingToTech(PlasticLiquidConduitConfig.ID, GameStrings.Technology.Decor.HomeLuxuries);

			BuildingUtils.AddBuildingToPlanScreen(PlasticLiquidVentConfig.ID, GameStrings.PlanMenuCategory.Plumbing, subcategory: GameStrings.PlanMenuSubcategory.Plumbing.Pipes);
			BuildingUtils.AddBuildingToTech(PlasticLiquidVentConfig.ID, GameStrings.Technology.Decor.HomeLuxuries);

			BuildingUtils.AddBuildingToPlanScreen(PlasticGasConduitConfig.ID, GameStrings.PlanMenuCategory.Ventilation, subcategory: GameStrings.PlanMenuSubcategory.Ventilation.Pipes);
			BuildingUtils.AddBuildingToTech(PlasticGasConduitConfig.ID, GameStrings.Technology.Decor.HomeLuxuries);

			BuildingUtils.AddBuildingToPlanScreen(PlasticGasVentConfig.ID, GameStrings.PlanMenuCategory.Ventilation, subcategory: GameStrings.PlanMenuSubcategory.Ventilation.Pipes);
			BuildingUtils.AddBuildingToTech(PlasticGasVentConfig.ID, GameStrings.Technology.Decor.HomeLuxuries);
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
