using System;
using System.Collections.Generic;

using HarmonyLib;

using PeterHan.PLib.UI;

using RomenH.Common;
using RomenH.ScheduleSensor;

namespace RomenH.LogicScheduleSensor
{
	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch("Initialize")]
	public static class Db_Initialize_Patch
	{
		public static void Postfix()
		{
			BuildingUtils.AddBuildingToPlanScreen(LogicScheduleSensorConfig.ID, GameStrings.PlanMenuCategory.Automation, subcategory: GameStrings.PlanMenuSubcategory.Automation.Sensors);
			BuildingUtils.AddBuildingToTech(LogicScheduleSensorConfig.ID, GameStrings.Technology.Computers.GenericSensors);

			var scheduleGroups = Db.Get().ScheduleGroups.allGroups;
			if (scheduleGroups != null)
			{
				foreach (var scheduleGroup in scheduleGroups)
				{
					ScheduleGroupInfo.BlockColors[scheduleGroup.Id] = scheduleGroup.uiColor;
				}
			}
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
}
