
using RomenH.Common;

namespace RomenH.FestiveDecor
{
	public static class ModModules
	{
		public const string LUTModuleID = "LUTNotIncluded";

		public static bool LUTEnabled
		{ get; private set; }

		internal static void EnableLUTModule(KMod.Mod mod)
		{
			if (mod == null) return;

			if (Mod.Registry != null)
			{
				if (ModAssets.lutNight != null)
				{
					Mod.Registry[RegistryKeys.LUTNotIncluded_NightLUT] = ModAssets.lutNight;
					LUTEnabled = true;
				}
				else
				{
					Debug.Log("Festive Decor: Failed to apply night LUT. (No asset)");
				}
			}
			else
			{
				Debug.Log("Festive Decor: Failed to enable LUT Module. (No registry)");
			}
		}
	}
}
