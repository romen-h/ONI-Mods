using System.IO;
using System.Reflection;

using UnityEngine;

namespace RomenH.LUTNotIncluded
{
	internal static class ModAssets
	{
		internal static readonly string TextureDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Textures");

		internal static Texture2D defaultDayLUT;
		internal static Texture2D dayLUT;

		internal static Texture2D defaultNightLUT;
		internal static Texture2D nightLUT;

		internal static void LoadAssets()
		{
			try
			{
				string lutDayPath = Path.Combine(TextureDirectory, $"lut_day.png");
				byte[] lutDayBytes = File.ReadAllBytes(lutDayPath);

				dayLUT = new Texture2D(2, 2);
				dayLUT.LoadImage(lutDayBytes);
			}
			catch
			{
				dayLUT = null;
			}

			if (dayLUT != null) Debug.Log("LUTNotIncluded: Loaded LUT for day.");

			try
			{
				string lutNightPath = Path.Combine(TextureDirectory, "Textures", $"lut_night.png");
				byte[] lutNightBytes = File.ReadAllBytes(lutNightPath);

				nightLUT = new Texture2D(2, 2);
				nightLUT.LoadImage(lutNightBytes);
			}
			catch
			{
				nightLUT = null;
			}

			if (nightLUT != null) Debug.Log("LUTNotIncluded: Loaded LUT for night.");
		}
	}
}
