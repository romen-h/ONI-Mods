using HarmonyLib;

using PeterHan.PLib.Core;

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
			if (ModSettings.Instance.EnableColorCorrection)
			{
				// Get this mod's LUT textures (might not exist)
				Texture2D dayTexture = ModAssets.dayLUT;
				Texture2D nightTexture = ModAssets.nightLUT;

				// Load overrides from mod interop Registry
				if (ModCommon.Registry != null)
				{
					if (ModCommon.Registry.ContainsKey(RegistryKeys.LUTNotIncluded_DayLUT))
					{
						var dayObj = ModCommon.Registry[RegistryKeys.LUTNotIncluded_DayLUT];
						if (dayObj is Texture2D tex)
						{
							Debug.Log("LUT Not Included: Found override texture for day LUT.");
							nightTexture = tex;
						}
					}

					if (ModCommon.Registry.ContainsKey(RegistryKeys.LUTNotIncluded_NightLUT))
					{
						var nightObj = ModCommon.Registry[RegistryKeys.LUTNotIncluded_NightLUT];
						if (nightObj is Texture2D tex)
						{
							Debug.Log("LUT Not Included: Found override texture for night LUT.");
							nightTexture = tex;
						}
					}
				}

				// Set textures to defaults if still null
				if (dayTexture == null) dayTexture = __instance.dayColourCube;
				if (nightTexture == null) nightTexture = __instance.nightColourCube;

				// Update LUT textures based on setting
				if (ModSettings.Instance.ForceLUT == ForceLUTOptions.AlwaysDay)
				{
					__instance.dayColourCube = dayTexture;
					__instance.nightColourCube = dayTexture;
				}
				else if (ModSettings.Instance.ForceLUT == ForceLUTOptions.AlwaysNight)
				{
					__instance.dayColourCube = nightTexture;
					__instance.nightColourCube = nightTexture;
				}
				else
				{
					__instance.dayColourCube = dayTexture;
					__instance.nightColourCube = nightTexture;
				}

				// Apply new LUTs
				var cc = __instance.overlayCamera.GetComponent<ColorCorrectionLookup>();
				if (cc != null)
				{
					cc.Convert(__instance.dayColourCube, "");
					cc.Convert2(__instance.nightColourCube, "");
				}
			}
		}
	}
}
