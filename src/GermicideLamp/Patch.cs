using Harmony;
using RomenMods.Common;

namespace RomenMods.GermicideLampMod
{
	[HarmonyPatch(typeof(GeneratedBuildings))]
	[HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
	public class GeneratedBuildings_LoadGeneratedBuildings_Patch
	{
		public static void Prefix()
		{
			StringUtils.AddBuildingStrings(GermicideLampConfig.ID, ModStrings.GERMICIDELAMP.NAME, ModStrings.GERMICIDELAMP.DESC, ModStrings.GERMICIDELAMP.EFFECT);
			StringUtils.AddBuildingStrings(CeilingGermicideLampConfig.ID, ModStrings.CEILINGGERMICIDELAMP.NAME, ModStrings.CEILINGGERMICIDELAMP.DESC, ModStrings.CEILINGGERMICIDELAMP.EFFECT);

			BuildingUtils.AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Utilities, GermicideLampConfig.ID);
			BuildingUtils.AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Utilities, CeilingGermicideLampConfig.ID);
		}
	}

	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch("Initialize")]
	public static class Db_Initialize_Patch
	{
		public static void Prefix()
		{
			BuildingUtils.AddBuildingToTechnology(GameStrings.Technology.Medicine.PathogenDiagnostics, GermicideLampConfig.ID);
			BuildingUtils.AddBuildingToTechnology(GameStrings.Technology.Medicine.PathogenDiagnostics, CeilingGermicideLampConfig.ID);
		}
	}
}
