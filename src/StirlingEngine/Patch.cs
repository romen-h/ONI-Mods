using HarmonyLib;
using RomenH.Common;

namespace RomenH.StirlingEngine
{
	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch("Initialize")]
	public class Db_Initialize_Patch
	{
		public static void Prefix()
		{
			Debug.Log("Stirling Engine: Adding strings...");

			StringUtils.AddBuildingStrings(
				StirlingEngineConfig.ID,
				ModStrings.STRINGS.BUILDINGS.STIRLINGENGINE.NAME,
				ModStrings.STRINGS.BUILDINGS.STIRLINGENGINE.DESC,
				ModStrings.STRINGS.BUILDINGS.STIRLINGENGINE.EFFECT);

			StringUtils.AddStatusItemStrings(
				ModStrings.STRINGS.BUILDINGS.STIRLINGENGINE_ACTIVE.ID,
				"BUILDING",
				ModStrings.STRINGS.BUILDINGS.STIRLINGENGINE_ACTIVE.NAME,
				ModStrings.STRINGS.BUILDINGS.STIRLINGENGINE_ACTIVE.TOOLTIP);

			StringUtils.AddStatusItemStrings(
				ModStrings.STRINGS.BUILDINGS.STIRLINGENGINE_NO_HEAT_GRADIENT.ID,
				"BUILDING",
				ModStrings.STRINGS.BUILDINGS.STIRLINGENGINE_NO_HEAT_GRADIENT.NAME,
				ModStrings.STRINGS.BUILDINGS.STIRLINGENGINE_NO_HEAT_GRADIENT.TOOLTIP);

			StringUtils.AddStatusItemStrings(
				ModStrings.STRINGS.BUILDINGS.STIRLINGENGINE_TOO_HOT.ID,
				"BUILDING",
				ModStrings.STRINGS.BUILDINGS.STIRLINGENGINE_TOO_HOT.NAME,
				ModStrings.STRINGS.BUILDINGS.STIRLINGENGINE_TOO_HOT.TOOLTIP);

			StringUtils.AddStatusItemStrings(
				ModStrings.STRINGS.BUILDINGS.STIRLINGENGINE_ACTIVE_WATTAGE.ID,
				"BUILDING",
				ModStrings.STRINGS.BUILDINGS.STIRLINGENGINE_ACTIVE_WATTAGE.NAME,
				ModStrings.STRINGS.BUILDINGS.STIRLINGENGINE_ACTIVE_WATTAGE.TOOLTIP);

			StringUtils.AddStatusItemStrings(
				ModStrings.STRINGS.BUILDINGS.STIRLINGENGINE_HEAT_PUMPED.ID,
				"BUILDING",
				ModStrings.STRINGS.BUILDINGS.STIRLINGENGINE_HEAT_PUMPED.NAME,
				ModStrings.STRINGS.BUILDINGS.STIRLINGENGINE_HEAT_PUMPED.TOOLTIP);

		}

		public static void Postfix()
		{
			BuildingUtils.AddBuildingToPlanScreen(StirlingEngineConfig.ID, GameStrings.PlanMenuCategory.Power);
			BuildingUtils.AddBuildingToTech(StirlingEngineConfig.ID, GameStrings.Technology.Gases.TemperatureModulation);
		}
	}
}
