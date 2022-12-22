using HarmonyLib;

using Klei.AI;

using RomenH.Common;

namespace RomenH.GermicideLamp
{
	[HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
	public static class Db_Initialize_Patch
	{
		public static void Postfix()
		{
			BuildingUtils.AddBuildingToPlanScreen(GermicideLampConfig.ID, GameStrings.PlanMenuCategory.Medicine);
			BuildingUtils.AddBuildingToPlanScreen(CeilingGermicideLampConfig.ID, GameStrings.PlanMenuCategory.Medicine);

			BuildingUtils.AddBuildingToTech(GermicideLampConfig.ID, GameStrings.Technology.Medicine.MicroTargetedMedicine);
			BuildingUtils.AddBuildingToTech(CeilingGermicideLampConfig.ID, GameStrings.Technology.Medicine.PathogenDiagnostics);
		}
	}

	[HarmonyPatch(typeof(Localization), "Initialize")]
	public class Localization_Initialize_Patch
	{
		public static void Postfix()
		{
			StringUtils.LoadTranslations();
		}
	}
}
