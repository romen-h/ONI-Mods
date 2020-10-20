using Harmony;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomenMods.FestiveDecorMod
{
	/// <summary>
	/// Painting (2x2)
	/// </summary>
	[HarmonyPatch(typeof(CanvasConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class CanvasConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			if (FestivalManager.CurrentFestival != Festival.None)
			{
				KAnimFile anim = Assets.GetAnim($"painting_{FestivalManager.FestivalAnimAffix}_kanim");
				if (anim != null) __result.AnimFiles = new KAnimFile[1] { anim };
			}
		}
	}

	/// <summary>
	/// Painting (2x3)
	/// </summary>
	[HarmonyPatch(typeof(CanvasTallConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class CanvasTallConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			if (FestivalManager.CurrentFestival != Festival.None)
			{
				KAnimFile anim = Assets.GetAnim($"painting_tall_{FestivalManager.FestivalAnimAffix}_kanim");
				if (anim != null) __result.AnimFiles = new KAnimFile[1] { anim };
			}
		}
	}

	/// <summary>
	/// Painting (3x2)
	/// </summary>
	[HarmonyPatch(typeof(CanvasWideConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class CanvasWideConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			if (FestivalManager.CurrentFestival != Festival.None)
			{
				KAnimFile anim = Assets.GetAnim($"painting_wide_{FestivalManager.FestivalAnimAffix}_kanim");
				if (anim != null) __result.AnimFiles = new KAnimFile[1] { anim };
			}
		}
	}
}
