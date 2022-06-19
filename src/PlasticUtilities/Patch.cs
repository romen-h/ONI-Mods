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
		public static void Prefix()
		{
			Debug.Log("New Lights: Adding strings...");

			StringUtils.AddBuildingStrings(
				PlasticGasConduitConfig.ID,
				PlasticGasConduitConfig.Name,
				PlasticGasConduitConfig.Desc,
				PlasticGasConduitConfig.Effect
			);

			StringUtils.AddBuildingStrings(
				PlasticGasVentConfig.ID,
				PlasticGasVentConfig.Name,
				PlasticGasVentConfig.Desc,
				PlasticGasVentConfig.Effect
			);

			StringUtils.AddBuildingStrings(
				PlasticLiquidConduitConfig.ID,
				PlasticLiquidConduitConfig.Name,
				PlasticLiquidConduitConfig.Desc,
				PlasticLiquidConduitConfig.Effect
			);

			StringUtils.AddBuildingStrings(
				PlasticLiquidVentConfig.ID,
				PlasticLiquidVentConfig.Name,
				PlasticLiquidVentConfig.Desc,
				PlasticLiquidVentConfig.Effect
			);

			StringUtils.AddBuildingStrings(
				PlasticBubbleWallConfig.ID,
				PlasticBubbleWallConfig.Name,
				PlasticBubbleWallConfig.Desc,
				PlasticBubbleWallConfig.Effect
			);

			StringUtils.AddBuildingStrings(
				PlasticCheckerWallConfig.ID,
				PlasticCheckerWallConfig.Name,
				PlasticCheckerWallConfig.Desc,
				PlasticCheckerWallConfig.Effect
			);

			StringUtils.ExportTranslationTemplates();
		}

		public static void Postfix()
		{
			Debug.Log("Plastic Utilities: Adding buildings to game...");

			BuildingUtils.AddBuildingToPlanScreen(PlasticGasConduitConfig.ID, GameStrings.PlanMenuCategory.Ventilation, GasConduitConfig.ID);
			BuildingUtils.AddBuildingToTech(PlasticGasConduitConfig.ID, GameStrings.Technology.Decor.HomeLuxuries);

			BuildingUtils.AddBuildingToPlanScreen(PlasticGasVentConfig.ID, GameStrings.PlanMenuCategory.Ventilation, GasVentConfig.ID);
			BuildingUtils.AddBuildingToTech(PlasticGasVentConfig.ID, GameStrings.Technology.Decor.HomeLuxuries);

			BuildingUtils.AddBuildingToPlanScreen(PlasticLiquidConduitConfig.ID, GameStrings.PlanMenuCategory.Plumbing, LiquidConduitConfig.ID);
			BuildingUtils.AddBuildingToTech(PlasticLiquidConduitConfig.ID, GameStrings.Technology.Decor.HomeLuxuries);

			BuildingUtils.AddBuildingToPlanScreen(PlasticLiquidVentConfig.ID, GameStrings.PlanMenuCategory.Plumbing, LiquidVentConfig.ID);
			BuildingUtils.AddBuildingToTech(PlasticLiquidVentConfig.ID, GameStrings.Technology.Decor.HomeLuxuries);

			BuildingUtils.AddBuildingToPlanScreen(PlasticBubbleWallConfig.ID, GameStrings.PlanMenuCategory.Utilities, ExteriorWallConfig.ID);
			BuildingUtils.AddBuildingToTech(PlasticBubbleWallConfig.ID, GameStrings.Technology.Decor.HomeLuxuries);

			BuildingUtils.AddBuildingToPlanScreen(PlasticCheckerWallConfig.ID, GameStrings.PlanMenuCategory.Utilities, ExteriorWallConfig.ID);
			BuildingUtils.AddBuildingToTech(PlasticCheckerWallConfig.ID, GameStrings.Technology.Decor.HomeLuxuries);
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
