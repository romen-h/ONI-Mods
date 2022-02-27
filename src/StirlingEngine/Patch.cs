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
				StirlingEngineConfig.Name,
				StirlingEngineConfig.Desc,
				StirlingEngineConfig.Effect
			);

			StringUtils.AddStatusItemStrings(
				StirlingEngine.ActiveStatusItem.ID,
				"BUILDING",
				StirlingEngine.ActiveStatusItem.Name,
				StirlingEngine.ActiveStatusItem.Tooltip);

			StringUtils.AddStatusItemStrings(
				StirlingEngine.NoHeatGradientStatusItem.ID,
				"BUILDING",
				StirlingEngine.NoHeatGradientStatusItem.Name,
				StirlingEngine.NoHeatGradientStatusItem.Tooltip);

			StringUtils.AddStatusItemStrings(
				StirlingEngine.TooHotStatusItem.ID,
				"BUILDING",
				StirlingEngine.TooHotStatusItem.Name,
				StirlingEngine.TooHotStatusItem.Tooltip);

			StringUtils.AddStatusItemStrings(
				StirlingEngine.ActiveWattageStatusItem.ID,
				"BUILDING",
				StirlingEngine.ActiveWattageStatusItem.Name,
				StirlingEngine.ActiveWattageStatusItem.Tooltip);

			StringUtils.AddStatusItemStrings(
				StirlingEngine.HeatPumpedStatusItem.ID,
				"BUILDING",
				StirlingEngine.HeatPumpedStatusItem.Name,
				StirlingEngine.HeatPumpedStatusItem.Tooltip);

			StringUtils.ExportTranslationTemplates();
		}

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
