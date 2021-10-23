using HarmonyLib;

using UnityEngine;

namespace RomenH.FestiveDecor
{
	/// <summary>
	/// Earth
	/// </summary>
	[HarmonyPatch(typeof(BackgroundEarthConfig))]
	[HarmonyPatch("CreatePrefab")]
	public static class BackgroundEarthConfig_CreatePrefab_Patch
	{
		public static void Postfix(GameObject __result)
		{
			Util.ReplaceAnim(__result, "earth");
		}
	}
}
