using Harmony;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace RomenMods.FestiveDecorMod
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
			if (FestivalManager.CurrentFestival != Festival.None)
			{
				var anim = Assets.GetAnim($"earth_{FestivalManager.FestivalAnimAffix}_kanim");
				if (anim != null)
				{
					var ac = __result.GetComponent<KBatchedAnimController>();
					ac.AnimFiles = new KAnimFile[1] { anim };
				}
			}
		}
	}
}
