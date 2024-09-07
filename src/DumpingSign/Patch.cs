using HarmonyLib;
using RomenH.Common;

namespace RomenH.DumpingSign
{
	[HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
	public class Db_Initialize_Patch
	{
		public static void Postfix()
		{
			SignCategories.InitCategories();
			BuildingUtils.AddBuildingToPlanScreen(DumpingSignConfig.ID, GameStrings.PlanMenuCategory.Base, subcategory: "storage");
		}
	}
}
