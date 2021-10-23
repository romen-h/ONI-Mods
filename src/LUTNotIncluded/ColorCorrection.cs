using System.Collections.Generic;
using System.IO;
using HarmonyLib;

using RomenH.Common;

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
#if DUMP_TEXTURES
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

			Texture2D dayTexture = ModAssets.dayLUT;
			Texture2D nightTexture = ModAssets.nightLUT;

			if (Mod.Settings.EnableColorCorrection)
			{
				if (Mod.Registry != null)
				{
					if (Mod.Registry.ContainsKey(RegistryKeys.LUTNotIncluded_DayLUT))
					{
						var dayObj = Mod.Registry[RegistryKeys.LUTNotIncluded_DayLUT];
						if (dayObj is Texture2D tex)
						{
							Debug.Log("LUT Not Included: Found override texture for day LUT.");
							nightTexture = tex;
						}
					}

					if (Mod.Registry.ContainsKey(RegistryKeys.LUTNotIncluded_NightLUT))
					{
						var nightObj = Mod.Registry[RegistryKeys.LUTNotIncluded_NightLUT];
						if (nightObj is Texture2D tex)
						{
							Debug.Log("LUT Not Included: Found override texture for night LUT.");
							nightTexture = tex;
						}
					}
				}

				bool changes = false;

				if (dayTexture != null)
				{
					__instance.dayColourCube = ModAssets.dayLUT;
					changes = true;
				}

				if (Mod.Settings.AlwaysDay)
				{
					if (dayTexture != null)
						__instance.nightColourCube = dayTexture;
					else
						__instance.nightColourCube = __instance.dayColourCube;
					changes = true;
				}
				else 
				{
					if (nightTexture != null)
					{
						__instance.nightColourCube = nightTexture;
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
