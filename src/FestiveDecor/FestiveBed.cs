using Harmony;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomenMods.FestiveDecorMod
{
	/// <summary>
	/// Basic Bed
	/// </summary>
	[HarmonyPatch(typeof(BedConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class BedConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			if (FestivalManager.CurrentFestival != Festival.None)
			{
				KAnimFile anim = Assets.GetAnim($"bedlg_{FestivalManager.FestivalAnimAffix}_kanim");
				if (anim != null) __result.AnimFiles = new KAnimFile[1] { anim };
			}
		}
	}

	/// <summary>
	/// Plastic Bed
	/// </summary>
	[HarmonyPatch(typeof(LuxuryBedConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class LuxuryBedConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			if (FestivalManager.CurrentFestival != Festival.None)
			{
				KAnimFile anim = Assets.GetAnim($"elegantbed_{FestivalManager.FestivalAnimAffix}_kanim");
				if (anim != null) __result.AnimFiles = new KAnimFile[1] { anim };
			}
		}
	}
}
