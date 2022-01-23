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

			StringUtils.ExportTranslationTemplates();
		}

		public static void Postfix()
		{
			BuildingUtils.AddBuildingToPlanScreen(PipedDeodorizerConfig.ID, GameStrings.PlanMenuCategory.Oxygen);
			BuildingUtils.AddBuildingToTech(PipedDeodorizerConfig.ID, GameStrings.Technology.Gases.HVAC);
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
