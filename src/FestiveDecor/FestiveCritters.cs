using HarmonyLib;
using UnityEngine;

namespace RomenH.FestiveDecor
{
	/// <summary>
	/// Slickster
	/// </summary>
	[HarmonyPatch(typeof(OilFloaterConfig))]
	[HarmonyPatch("CreatePrefab")]
	public static class BaseOilFloaterConfig_CreatePrefab_Patch
	{
		public static void Postfix(GameObject __result)
		{
			Util.ReplaceAnim(__result, "oilfloater");
		}
	}
}
