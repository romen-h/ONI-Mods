using HarmonyLib;

using RomenH.Common;

namespace RomenH.StirlingEngine
{
	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch("Initialize")]
	public class Db_Initialize_Patch
	{
		public static void Postfix()
		{
			BuildingUtils.AddBuildingToPlanScreen(StirlingEngineConfig.ID, GameStrings.PlanMenuCategory.Power);
			BuildingUtils.AddBuildingToTech(StirlingEngineConfig.ID, GameStrings.Technology.Gases.TemperatureModulation);
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
