using System.Collections.Generic;

using HarmonyLib;

using PeterHan.PLib.UI;

using RomenH.Common;

namespace RomenH.PlasticDoor
{
	[HarmonyPatch(typeof(Db), "Initialize")]
	public static class Db_Initialize_Patch
	{
		public static void Postfix()
		{
			BuildingUtils.AddBuildingToPlanScreen(PlasticCurtainConfig.ID, PlasticCurtainConfig.PlanMenu, subcategory: GameStrings.PlanMenuSubcategory.Base.Doors);
			BuildingUtils.AddBuildingToTech(PlasticCurtainConfig.ID, PlasticCurtainConfig.Tech);
		}
	}

	[HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
	public static class DetailsScreen_OnPrefabInit_Patch
	{
		internal static void Postfix()
		{
			PUIUtils.AddSideScreenContent<SimpleCurtainSidescreen>();
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

	[HarmonyPatch(typeof(Database.BuildingStatusItems), "CreateStatusItems")]
	public static class Database_BuildingStatusItems_CreateStatusItems_Patch
	{
		public static void Postfix()
		{
			ModAssets.MakeStatusItem();
		}
	}

	[HarmonyPatch(typeof(ElementLoader), "Load")]
	public static class Patch_ElementLoader_Load
	{
		public static void Postfix()
		{
			List<SimHashes> elementsToTag = new List<SimHashes>
			{
				SimHashes.Polypropylene,
				SimHashes.Isoresin,
				SimHashes.SuperInsulator,
				SimHashes.SolidViscoGel
			};

			foreach (SimHashes hash in elementsToTag)
			{
				Element element = ElementLoader.FindElementByHash(hash);
				element.oreTags = element.oreTags.Append(ModAssets.plasticTag);
			}
		}
	}
}
