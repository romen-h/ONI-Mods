using Harmony;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace RomenMods.FestiveDecorMod
{
	/// <summary>
	/// Ladder
	/// </summary>
	[HarmonyPatch(typeof(LadderConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class LadderConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			if (FestivalManager.CurrentFestival == Festival.Halloween)
			{
				KAnimFile anim = Assets.GetAnim($"ladder_halloween_kanim");
				if (anim != null) __result.AnimFiles = new KAnimFile[1] { anim };
			}
			else if (FestivalManager.CurrentFestival != Festival.None)
			{
				KAnimFile anim = Assets.GetAnim($"ladder_{FestivalManager.FestivalAnimAffix}_kanim");
				if (anim != null) __result.AnimFiles = new KAnimFile[1] { anim };
			}
		}
	}

#if true
	/// <summary>
	/// Ladder
	/// </summary>
	[HarmonyPatch(typeof(LadderConfig))]
	[HarmonyPatch("DoPostConfigureComplete")]
	public static class LadderConfig_DoPostConfigureComplete_Patch
	{
		public static void Postfix(GameObject __0)
		{
			if (FestivalManager.CurrentFestival == Festival.Halloween)
			{
				SymbolOverrideControllerUtil.AddToPrefab(__0);
				var glow = __0.AddComponent<GlowInTheDark>();
				glow.noGlowAnim = Assets.GetAnim("ladder_halloween_kanim");
				glow.glowAnim = Assets.GetAnim("ladder_halloween_glow_kanim");
			}
		}
	}
#endif

	/// <summary>
	/// Plastic Ladder
	/// </summary>
	[HarmonyPatch(typeof(LadderFastConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class LadderFastConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			if (FestivalManager.CurrentFestival != Festival.None)
			{
				KAnimFile anim = Assets.GetAnim($"ladder_plastic_{FestivalManager.FestivalAnimAffix}_kanim");
				if (anim != null) __result.AnimFiles = new KAnimFile[1] { anim };
			}
		}
	}
}
