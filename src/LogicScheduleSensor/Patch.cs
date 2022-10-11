using System;
using System.Collections.Generic;

using HarmonyLib;

using PeterHan.PLib.UI;

using RomenH.Common;

namespace RomenH.LogicScheduleSensor
{
	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch("Initialize")]
	public static class Db_Initialize_Patch
	{
		public static void Postfix()
		{
			BuildingUtils.AddBuildingToPlanScreen(LogicScheduleSensorConfig.ID, GameStrings.PlanMenuCategory.Automation);
			BuildingUtils.AddBuildingToTech(LogicScheduleSensorConfig.ID, GameStrings.Technology.Computers.GenericSensors);
		}
	}

	[HarmonyPatch(typeof(DetailsScreen))]
	[HarmonyPatch("OnPrefabInit")]
	public static class DetailsScreen_OnPrefabInit_Patch
	{
		internal static void Postfix()
		{
			PUIUtils.AddSideScreenContent<LogicScheduleSensorSideScreen>();
		}
	}

	[HarmonyPatch(typeof(ScheduleScreenEntry))]
	[HarmonyPatch("OnNameChanged")]
	public static class ScheduleScreenEntry_OnNameChanged_Patch
	{
		public static event Action<Schedule> ScheduleNameChanged;

		public static void Postfix(ScheduleScreenEntry __instance)
		{
			ScheduleNameChanged?.Invoke(__instance.schedule);
		}
	}

	[HarmonyPatch(typeof(ScheduleScreen))]
	[HarmonyPatch("OnPrefabInit")]
	[HarmonyPriority(Priority.Low)]
	public static class ScheduleScreen_OnPrefabInit_Patch
	{
		public static Dictionary<string, ColorStyleSetting> blockColors;

		public static void Postfix(ScheduleScreen __instance)
		{
			var fi = AccessTools.Field(typeof(ScheduleScreen), "paintStyles");
			blockColors = (Dictionary<string, ColorStyleSetting>)fi.GetValue(__instance);
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
