using Harmony;
using PeterHan.PLib.UI;
using RomenMods.Common;

namespace RomenMods.StirlingEngineMod
{
	[HarmonyPatch(typeof(GeneratedBuildings))]
	[HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
	public class GeneratedBuildings_LoadGeneratedBuildings_Patch
	{
		public static void Prefix()
		{
			StringUtils.AddBuildingStrings(StirlingEngineConfig.ID, ModStrings.STIRLINGENGINE.NAME, ModStrings.STIRLINGENGINE.DESC, ModStrings.STIRLINGENGINE.EFFECT);
			StringUtils.AddStatusItemStrings(ModStrings.STIRLINGENGINE_ACTIVE.ID, "BUILDING", ModStrings.STIRLINGENGINE_ACTIVE.NAME, ModStrings.STIRLINGENGINE_ACTIVE.TOOLTIP);
			StringUtils.AddStatusItemStrings(ModStrings.STIRLINGENGINE_NO_HEAT_GRADIENT.ID, "BUILDING", ModStrings.STIRLINGENGINE_NO_HEAT_GRADIENT.NAME, ModStrings.STIRLINGENGINE_NO_HEAT_GRADIENT.TOOLTIP);
			StringUtils.AddStatusItemStrings(ModStrings.STIRLINGENGINE_TOO_HOT.ID, "BUILDING", ModStrings.STIRLINGENGINE_TOO_HOT.NAME, ModStrings.STIRLINGENGINE_TOO_HOT.TOOLTIP);
			StringUtils.AddStatusItemStrings(ModStrings.STIRLINGENGINE_ACTIVE_WATTAGE.ID, "BUILDING", ModStrings.STIRLINGENGINE_ACTIVE_WATTAGE.NAME, ModStrings.STIRLINGENGINE_ACTIVE_WATTAGE.TOOLTIP);
			StringUtils.AddStatusItemStrings(ModStrings.STIRLINGENGINE_HEAT_PUMPED.ID, "BUILDING", ModStrings.STIRLINGENGINE_HEAT_PUMPED.NAME, ModStrings.STIRLINGENGINE_HEAT_PUMPED.TOOLTIP);
			BuildingUtils.AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Power, StirlingEngineConfig.ID);
		}
	}

	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch("Initialize")]
	public static class Db_Initialize_Patch
	{
		public static void Prefix()
		{
			BuildingUtils.AddBuildingToTechnology(GameStrings.Technology.Gases.TemperatureModulation, StirlingEngineConfig.ID);
		}
	}

#if ENABLE_SIDESCREEN
	[HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
	public static class DetailsScreen_OnPrefabInit_Patch
	{
		internal static void Postfix()
		{
			PUIUtils.AddSideScreenContent<StirlingEngineSideScreen>();
		}
	}
#endif
}
