using HarmonyLib;
using RomenH.Common;

namespace RomenH.PipedDeodorizer
{
	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch("Initialize")]
	public static class Db_Initialize_Patch
	{
		public static void Prefix()
		{
			Debug.Log("Piped Deodorizer: Adding strings...");

			StringUtils.AddBuildingStrings(
				PipedDeodorizerConfig.ID,
				ModStrings.STRINGS.BUILDINGS.PIPEDDEODORIZER.NAME,
				ModStrings.STRINGS.BUILDINGS.PIPEDDEODORIZER.DESC,
				ModStrings.STRINGS.BUILDINGS.PIPEDDEODORIZER.EFFECT);
		}

		public static void Postfix()
		{
			BuildingUtils.AddBuildingToPlanScreen(PipedDeodorizerConfig.ID, GameStrings.PlanMenuCategory.Oxygen);
			BuildingUtils.AddBuildingToTech(PipedDeodorizerConfig.ID, GameStrings.Technology.Gases.HVAC);
		}
	}
}
