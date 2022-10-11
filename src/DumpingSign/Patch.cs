
using HarmonyLib;

using RomenH.Common;

namespace RomenH.DumpingSign
{
	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch(nameof(Db.Initialize))]
	public class Db_Initialize_Patch
	{
		public static void Postfix()
		{
			BuildingUtils.AddBuildingToPlanScreen(DumpingSignConfig.ID, GameStrings.PlanMenuCategory.Base);
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
