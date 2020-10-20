using Harmony;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomenMods.FestiveDecorMod
{
	/// <summary>
	/// Small Sculpture (1x2)
	/// </summary>
	[HarmonyPatch(typeof(SmallSculptureConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class SmallSculptureConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			if (FestivalManager.CurrentFestival != Festival.None)
			{
				KAnimFile anim = Assets.GetAnim($"sculpture_1x2_{FestivalManager.FestivalAnimAffix}_kanim");
				if (anim != null) __result.AnimFiles = new KAnimFile[1] { anim };
			}
		}
	}

	/// <summary>
	/// Large Sculpture (1x3)
	/// </summary>
	[HarmonyPatch(typeof(SculptureConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class SculptureConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			if (FestivalManager.CurrentFestival != Festival.None)
			{
				KAnimFile anim = Assets.GetAnim($"sculpture_{FestivalManager.FestivalAnimAffix}_kanim");
				if (anim != null) __result.AnimFiles = new KAnimFile[1] { anim };
			}
		}
	}

	/// <summary>
	/// Metal Sculpture
	/// </summary>
	[HarmonyPatch(typeof(MetalSculptureConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class MetalSculptureConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			if (FestivalManager.CurrentFestival != Festival.None)
			{
				KAnimFile anim = Assets.GetAnim($"sculpture_metal_{FestivalManager.FestivalAnimAffix}_kanim");
				if (anim != null) __result.AnimFiles = new KAnimFile[1] { anim };
			}
		}
	}

	/// <summary>
	/// Marble Sculpture
	/// </summary>
	[HarmonyPatch(typeof(MarbleSculptureConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class MarbleSculptureConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			if (FestivalManager.CurrentFestival != Festival.None)
			{
				KAnimFile anim = Assets.GetAnim($"sculpture_marble_{FestivalManager.FestivalAnimAffix}_kanim");
				if (anim != null) __result.AnimFiles = new KAnimFile[1] { anim };
			}
		}
	}
}
