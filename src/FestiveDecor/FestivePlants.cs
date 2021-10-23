using HarmonyLib;

using RomenH.Common;

using UnityEngine;

namespace RomenH.FestiveDecor
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
			Util.ReplaceAnim(__result, "meallice");
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
			Util.ReplaceAnim(__result, "meallice_grain");
		}
	}
}
