using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;

using RomenH.Common;

namespace RomenH.PlasticTiles
{
	[HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
	public static class Db_Initialize_Patch
	{
		public static void Postfix()
		{
			Debug.Log("Permeable Plastic Tiles: Adding buildings to game...");

			BuildingUtils.AddBuildingToPlanScreen(PlasticMeshTileConfig.ID, GameStrings.PlanMenuCategory.Base, subcategory: GameStrings.PlanMenuSubcategory.Base.Tiles);
			BuildingUtils.AddBuildingToTech(PlasticMeshTileConfig.ID, GameStrings.Technology.Decor.HomeLuxuries);

			BuildingUtils.AddBuildingToPlanScreen(PlasticMembraneTileConfig.ID, GameStrings.PlanMenuCategory.Base, subcategory: GameStrings.PlanMenuSubcategory.Base.Tiles);
			BuildingUtils.AddBuildingToTech(PlasticMembraneTileConfig.ID, GameStrings.Technology.Decor.HomeLuxuries);
		}
	}

	[HarmonyPatch(typeof(Localization), nameof(Localization.Initialize))]
	public class Localization_Initialize_Patch
	{
		public static void Postfix()
		{
			StringUtils.LoadTranslations();
		}
	}
}
