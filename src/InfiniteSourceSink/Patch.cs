using System;
using System.Collections.Generic;
using HarmonyLib;

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
				StringUtils.AddBuildingStrings(
					InfiniteLiquidSourceConfig.ID,
					InfiniteLiquidSourceConfig.DisplayName,
					InfiniteLiquidSourceConfig.Description,
					InfiniteLiquidSourceConfig.Effect);
				StringUtils.AddBuildingStrings(
					InfiniteLiquidSinkConfig.Id,
					InfiniteLiquidSinkConfig.DisplayName,
					InfiniteLiquidSinkConfig.Description,
					InfiniteLiquidSinkConfig.Effect);
				StringUtils.AddBuildingStrings(
					InfiniteGasSourceConfig.ID,
					InfiniteGasSourceConfig.DisplayName,
					InfiniteGasSourceConfig.Description,
					InfiniteGasSourceConfig.Effect);
				StringUtils.AddBuildingStrings(
					InfiniteGasSinkConfig.Id,
					InfiniteGasSinkConfig.DisplayName,
					InfiniteGasSinkConfig.Description,
					InfiniteGasSinkConfig.Effect);

				Strings.Add($"STRINGS.UI.UISIDESCREENS.INFINITESOURCE.FLOW.TITLE", InfiniteSourceControl.Title);
				Strings.Add($"STRINGS.UI.UISIDESCREENS.INFINITESOURCE.FLOW.TOOLTIP", InfiniteSourceControl.Tooltip);
				Strings.Add($"STRINGS.UI.UISIDESCREENS.INFINITESOURCE.TEMP.TITLE", InfiniteSourceTempControl.Title);
				Strings.Add($"STRINGS.UI.UISIDESCREENS.INFINITESOURCE.TEMP.TOOLTIP", InfiniteSourceTempControl.Tooltip);
			}

			public static void Postfix()
			{
				BuildingUtils.AddBuildingToPlanScreen(InfiniteLiquidSourceConfig.ID, GameStrings.PlanMenuCategory.Plumbing);
				BuildingUtils.AddBuildingToTech(InfiniteLiquidSourceConfig.ID, GameStrings.Technology.Liquids.Plumbing);

				BuildingUtils.AddBuildingToPlanScreen(InfiniteLiquidSinkConfig.Id, GameStrings.PlanMenuCategory.Plumbing);
				BuildingUtils.AddBuildingToTech(InfiniteLiquidSinkConfig.Id, GameStrings.Technology.Liquids.Plumbing);

				BuildingUtils.AddBuildingToPlanScreen(InfiniteGasSourceConfig.ID, GameStrings.PlanMenuCategory.Ventilation);
				BuildingUtils.AddBuildingToTech(InfiniteGasSourceConfig.ID, GameStrings.Technology.Gases.Ventilation);

				BuildingUtils.AddBuildingToPlanScreen(InfiniteGasSinkConfig.Id, GameStrings.PlanMenuCategory.Ventilation);
				BuildingUtils.AddBuildingToTech(InfiniteGasSinkConfig.Id, GameStrings.Technology.Gases.Ventilation);
			}
        }

		[HarmonyPatch(typeof(FilterSideScreen))]
		[HarmonyPatch(nameof(FilterSideScreen.IsValidForTarget))]
		public class FilterSideScreen_IsValidForTarget
		{
			private static bool Prefix(GameObject target, FilterSideScreen __instance, ref bool __result)
			{
				if (target.GetComponent<InfiniteSourceControl>() != null)
				{
					__result = !__instance.isLogicFilter;
					return false;
				}
				return true;
			}
		}
    }

}
