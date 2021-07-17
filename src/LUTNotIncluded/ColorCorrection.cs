using System.IO;
using HarmonyLib;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace RomenH.LUTNotIncluded
{
	[HarmonyPatch(typeof(CameraController))]
	[HarmonyPatch("OnPrefabInit")]
	public static class CameraController_OnPrefabInit_Patch
	{
		public static void Postfix(CameraController __instance)
		{
			ModAssets.defaultDayLUT = __instance.dayColourCube;
			ModAssets.defaultNightLUT = __instance.nightColourCube;
#if false
			try
			{
				byte[] pngBytes = ModAssets.defaultDayLUT.EncodeToPNG();
				string path = Path.Combine(ModAssets.TextureDirectory, "Defaults", "default_day_lut.png");
				File.WriteAllBytes(path, pngBytes);
			}
			catch
			{ }

			try
			{
				byte[] pngBytes = ModAssets.defaultNightLUT.EncodeToPNG();
				string path = Path.Combine(ModAssets.TextureDirectory, "Defaults", "default_night_lut.png");
				File.WriteAllBytes(path, pngBytes);
			}
			catch
			{ }
#endif

			if (Mod.Settings.EnableColorCorrection)
			{
				bool changes = false;

				if (ModAssets.dayLUT != null)
				{
					__instance.dayColourCube = ModAssets.dayLUT;
					changes = true;
				}

				if (Mod.Settings.AlwaysDay)
				{
					if (ModAssets.dayLUT != null)
						__instance.nightColourCube = ModAssets.dayLUT;
					else
						__instance.nightColourCube = __instance.dayColourCube;
					changes = true;
				}
				else 
				{
					if (ModAssets.nightLUT != null)
					{
						__instance.nightColourCube = ModAssets.nightLUT;
						changes = true;
					}
				}

				if (changes)
				{
					var cc = __instance.overlayCamera.GetComponent<ColorCorrectionLookup>();
					cc.Convert(__instance.dayColourCube, "");
					cc.Convert2(__instance.nightColourCube, "");
				}
			}
		}
	}
}
