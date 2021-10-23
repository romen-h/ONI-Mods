using HarmonyLib;

using RomenH.Common;

namespace RomenH.FestiveDecor
{
	/// <summary>
	/// Command Module
	/// </summary>
	[HarmonyPatch(typeof(CommandModuleConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class CommandModuleConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			Util.ReplaceAnim(__result, "rocket_command_module");
		}
	}
}
