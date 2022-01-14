using HarmonyLib;
using PeterHan.PLib.UI;
using RomenH.Common;
using System.Collections.Generic;

namespace Curtain
{
    class HarmonyPatches
    {
        [HarmonyPatch(typeof(Db))]
        [HarmonyPatch("Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
			{
				Debug.Log("Plastic Door: Adding strings...");

				StringUtils.AddBuildingStrings(
                    PlasticCurtainConfig.ID,
                    STRINGS.BUILDINGS.PREFABS.AC_PLASTICCURTAIN.NAME,
                    STRINGS.BUILDINGS.PREFABS.AC_PLASTICCURTAIN.DESC,
                    STRINGS.BUILDINGS.PREFABS.AC_PLASTICCURTAIN.EFFECT);

                StringUtils.AddStatusItemStrings(
                    STRINGS.BUILDING.STATUSITEMS.CHANGECURTAINCONTROLSTATE.ID,
                    "BUILDING",
                    STRINGS.BUILDING.STATUSITEMS.CHANGECURTAINCONTROLSTATE.NAME,
                    STRINGS.BUILDING.STATUSITEMS.CHANGECURTAINCONTROLSTATE.TOOLTIP);
			}

            public static void Postfix()
            {
                BuildingUtils.AddBuildingToPlanScreen(PlasticCurtainConfig.ID, PlasticCurtainConfig.PlanMenu);
                BuildingUtils.AddBuildingToTech(PlasticCurtainConfig.ID, PlasticCurtainConfig.Tech);
            }
        }

		[HarmonyPatch(typeof(DetailsScreen))]
		[HarmonyPatch("OnPrefabInit")]
		public static class DetailsScreen_OnPrefabInit_Patch
		{
			internal static void Postfix()
			{
				PUIUtils.AddSideScreenContent<SimpleCurtainSidescreen>();
			}
		}

#if false
        [HarmonyPatch(typeof(Localization), "Initialize")]
        class StringLocalisationPatch
        {
            public static void Postfix()
            {
                Loc.Translate(typeof(STRINGS));
            }
        }

        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        public static class DetailsScreen_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                Log.Info("Cloning side screen...");
                AddClonedSideScreen<CurtainSideScreen>("Curtain Side Screen", "Door Toggle Side Screen", typeof(DoorToggleSideScreen));
            }
        }
#endif

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
}
