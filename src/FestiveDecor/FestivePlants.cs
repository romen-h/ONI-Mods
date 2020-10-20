using Harmony;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace RomenMods.FestiveDecorMod
{
	/// <summary>
	/// Mealwood
	/// </summary>
	[HarmonyPatch(typeof(BasicSingleHarvestPlantConfig))]
	[HarmonyPatch("CreatePrefab")]
	public static class BasicSingleHarvestPlantConfig_CreatePrefab_Patch
	{
		public static void Postfix(GameObject __result)
		{
			if (FestivalManager.CurrentFestival != Festival.None)
			{
				var anim = Assets.GetAnim($"meallice_{FestivalManager.FestivalAnimAffix}_kanim");
				if (anim != null)
				{
					var ac = __result.GetComponent<KBatchedAnimController>();
					ac.AnimFiles = new KAnimFile[1] { anim };
				}
			}
		}
	}

	/// <summary>
	/// Meal Lice
	/// </summary>
	[HarmonyPatch(typeof(BasicPlantFoodConfig))]
	[HarmonyPatch("CreatePrefab")]
	public static class BasicPlantFoodConfig_CreatePrefab_Patch
	{
		public static void Postfix(GameObject __result)
		{
			if (FestivalManager.CurrentFestival != Festival.None)
			{
				var anim = Assets.GetAnim($"meallice_grain_{FestivalManager.FestivalAnimAffix}_kanim");
				if (anim != null)
				{
					var ac = __result.GetComponent<KBatchedAnimController>();
					ac.AnimFiles = new KAnimFile[1] { anim };
				}
			}
		}
	}
}
