using Harmony;
using UnityStandardAssets.ImageEffects;

namespace RomenMods.FestiveDecorMod
{
	[HarmonyPatch(typeof(CameraController))]
	[HarmonyPatch("OnPrefabInit")]
	public static class CameraController_OnPrefabInit_Patch
	{
		public static void Postfix(CameraController __instance)
		{
			if (Mod.Settings.EnableColorCorrection)
			{
				var cc = __instance.overlayCamera.GetComponent<ColorCorrectionLookup>();

				if (ModAssets.lutDay != null)
				{
					__instance.dayColourCube = ModAssets.lutDay;
					cc.Convert(__instance.dayColourCube, "");
				}

				if (ModAssets.lutNight != null)
				{
					__instance.nightColourCube = ModAssets.lutNight;
					cc.Convert2(__instance.nightColourCube, "");
				}
			}
		}
	}
}
