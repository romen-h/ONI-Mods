using HarmonyLib;

using RomenH.Common;

namespace RomenH.GermicideLamp
{
	[HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
	public static class Db_Initialize_Patch
	{
		public static void Postfix()
		{
			BuildingUtils.AddBuildingToPlanScreen(GermicideLampConfig.ID, GameStrings.PlanMenuCategory.Medicine, subcategory: GameStrings.PlanMenuSubcategory.Medicine.Hygiene);
			BuildingUtils.AddBuildingToPlanScreen(CeilingGermicideLampConfig.ID, GameStrings.PlanMenuCategory.Medicine, subcategory: GameStrings.PlanMenuSubcategory.Medicine.Hygiene);

			BuildingUtils.AddBuildingToTech(GermicideLampConfig.ID, GameStrings.Technology.Medicine.MicroTargetedMedicine);
			BuildingUtils.AddBuildingToTech(CeilingGermicideLampConfig.ID, GameStrings.Technology.Medicine.PathogenDiagnostics);
		}
	}
}
