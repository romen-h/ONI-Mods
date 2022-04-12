using HarmonyLib;

using PeterHan.PLib.UI;

using RomenH.Common;

using UnityEngine;

namespace InfiniteSourceSink
{
	public class InfiniteSourceSinkPatches
	{
		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch(nameof(Db.Initialize))]
		public class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				Debug.Log("Infinite Gases and Liquids: Adding strings...");

				StringUtils.AddBuildingStrings(
					InfiniteLiquidSourceConfig.ID,
					InfiniteLiquidSourceConfig.DisplayName,
					InfiniteLiquidSourceConfig.Description,
					InfiniteLiquidSourceConfig.Effect);
				StringUtils.AddBuildingStrings(
					InfiniteLiquidSinkConfig.ID,
					InfiniteLiquidSinkConfig.DisplayName,
					InfiniteLiquidSinkConfig.Description,
					InfiniteLiquidSinkConfig.Effect);
				StringUtils.AddBuildingStrings(
					InfiniteGasSourceConfig.ID,
					InfiniteGasSourceConfig.DisplayName,
					InfiniteGasSourceConfig.Description,
					InfiniteGasSourceConfig.Effect);
				StringUtils.AddBuildingStrings(
					InfiniteGasSinkConfig.ID,
					InfiniteGasSinkConfig.DisplayName,
					InfiniteGasSinkConfig.Description,
					InfiniteGasSinkConfig.Effect);

				Strings.Add($"STRINGS.UI.UISIDESCREENS.INFINITESOURCE.FLOW.TITLE", InfiniteSourceFlowControl.Title);
				Strings.Add($"STRINGS.UI.UISIDESCREENS.INFINITESOURCE.FLOW.TOOLTIP", InfiniteSourceFlowControl.Tooltip);
				Strings.Add($"STRINGS.UI.UISIDESCREENS.INFINITESOURCE.TEMP.TITLE", InfiniteSourceTempControl.Title);
				Strings.Add($"STRINGS.UI.UISIDESCREENS.INFINITESOURCE.TEMP.TOOLTIP", InfiniteSourceTempControl.Tooltip);

				StringUtils.ExportTranslationTemplates();
			}

			public static void Postfix()
			{
				BuildingUtils.AddBuildingToPlanScreen(InfiniteLiquidSourceConfig.ID, GameStrings.PlanMenuCategory.Plumbing);
				BuildingUtils.AddBuildingToTech(InfiniteLiquidSourceConfig.ID, GameStrings.Technology.Liquids.Plumbing);

				BuildingUtils.AddBuildingToPlanScreen(InfiniteLiquidSinkConfig.ID, GameStrings.PlanMenuCategory.Plumbing);
				BuildingUtils.AddBuildingToTech(InfiniteLiquidSinkConfig.ID, GameStrings.Technology.Liquids.Plumbing);

				BuildingUtils.AddBuildingToPlanScreen(InfiniteGasSourceConfig.ID, GameStrings.PlanMenuCategory.Ventilation);
				BuildingUtils.AddBuildingToTech(InfiniteGasSourceConfig.ID, GameStrings.Technology.Gases.Ventilation);

				BuildingUtils.AddBuildingToPlanScreen(InfiniteGasSinkConfig.ID, GameStrings.PlanMenuCategory.Ventilation);
				BuildingUtils.AddBuildingToTech(InfiniteGasSinkConfig.ID, GameStrings.Technology.Gases.Ventilation);
			}
		}

		[HarmonyPatch(typeof(FilterSideScreen))]
		[HarmonyPatch(nameof(FilterSideScreen.IsValidForTarget))]
		public class FilterSideScreen_IsValidForTarget
		{
			private static bool Prefix(GameObject target, FilterSideScreen __instance, ref bool __result)
			{
				if (target.GetComponent<InfiniteSourceFlowControl>() != null)
				{
					__result = !__instance.isLogicFilter;
					return false;
				}
				return true;
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

		[HarmonyPatch(typeof(SideScreenContent), nameof(SideScreenContent.GetSideScreenSortOrder))]
		public class SideScreenContent_GetSideScreenSortOrder_Patch
		{
			public static void Postfix(SideScreenContent __instance, ref int __result)
			{
				if (__instance is FilterSideScreen)
				{
					__result = 2;
				}
				else if (__instance is IntSliderSideScreen)
				{
					__result = 1;
				}
			}
		}
	}

}
