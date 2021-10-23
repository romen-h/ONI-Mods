using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;

using RomenH.Common;

namespace RomenH.Thresholds
{
	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch("Initialize")]
	public static class Db_Initialize_Patch
	{
		public static void Prefix()
		{
			StringUtils.AddBuildingStrings(
				ThresholdWallConfig.ID,
				ThresholdWallConfig.Name,
				ThresholdWallConfig.Desc,
				ThresholdWallConfig.Effect);
			StringUtils.AddBuildingStrings(
				CautionThresholdWallConfig.ID,
				CautionThresholdWallConfig.Name,
				CautionThresholdWallConfig.Desc,
				CautionThresholdWallConfig.Effect);
			StringUtils.AddBuildingStrings(
				MetalThresholdWallConfig.ID,
				MetalThresholdWallConfig.Name,
				MetalThresholdWallConfig.Desc,
				MetalThresholdWallConfig.Effect);
		}

		public static void Postfix()
		{
			BuildingUtils.AddBuildingToPlanScreen(ThresholdWallConfig.ID, GameStrings.PlanMenuCategory.Base);
			BuildingUtils.AddBuildingToPlanScreen(CautionThresholdWallConfig.ID, GameStrings.PlanMenuCategory.Base);
			BuildingUtils.AddBuildingToPlanScreen(MetalThresholdWallConfig.ID, GameStrings.PlanMenuCategory.Base);
			BuildingUtils.AddBuildingToTech(CautionThresholdWallConfig.ID, GameStrings.Technology.Decor.InteriorDecor);
			BuildingUtils.AddBuildingToTech(ThresholdWallConfig.ID, GameStrings.Technology.Decor.HomeLuxuries);
			BuildingUtils.AddBuildingToTech(MetalThresholdWallConfig.ID, GameStrings.Technology.SolidMaterial.RefinedRenovations);
		}
	}

#if false
	[HarmonyPatch]
	public static class RoomProber_IsWall_Patch
	{
		public static MethodBase TargetMethod()
		{
			return typeof(RoomProber).GetNestedType("CavityFloodFiller", BindingFlags.NonPublic).GetMethod("IsWall", BindingFlags.Static | BindingFlags.NonPublic);
		}

		public static bool Prefix(ref bool __result, int cell)
		{
			if (Threshold.ThresholdCells.Contains(cell))
			{
				__result = true;
				return false;
			}

			return true;
		}
	}
#endif
}
