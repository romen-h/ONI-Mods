using Harmony;
using RomenH.Common;

namespace RomenH.PipedDeodorizer
{
	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch("Initialize")]
	public static class Db_Initialize_Patch
	{
		public static void Prefix()
		{
			StringUtils.AddBuildingStrings(
				PipedDeodorizerConfig.ID,
				ModStrings.STRINGS.BUILDINGS.PIPEDDEODORIZER.NAME,
				ModStrings.STRINGS.BUILDINGS.PIPEDDEODORIZER.DESC,
				ModStrings.STRINGS.BUILDINGS.PIPEDDEODORIZER.EFFECT);

#if VANILLA
			BuildingUtils.AddBuildingToPlanScreen(PipedDeodorizerConfig.ID, GameStrings.PlanMenuCategory.Oxygen);
#endif
		}

		public static void Postfix()
		{
#if SPACED_OUT
			BuildingUtils.AddBuildingToPlanScreen(PipedDeodorizerConfig.ID, GameStrings.PlanMenuCategory.Oxygen);
#endif

			BuildingUtils.AddBuildingToTech(PipedDeodorizerConfig.ID, GameStrings.Technology.Gases.HVAC);
		}
	}
}
