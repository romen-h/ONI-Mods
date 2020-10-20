using Harmony;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomenMods.FestiveDecorMod
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
			if (FestivalManager.CurrentFestival != Festival.Halloween)
			{
				KAnimFile anim = Assets.GetAnim($"rocket_command_module_{FestivalManager.FestivalAnimAffix}_kanim");
				if (anim != null) __result.AnimFiles = new KAnimFile[1] { anim };
			}
		}
	}
}
