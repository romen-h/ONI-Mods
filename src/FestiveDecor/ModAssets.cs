using System.IO;
using System.Reflection;

using RomenH.Common;

using UnityEngine;

namespace RomenH.FestiveDecor
{
	internal static class ModAssets
	{
		internal static Texture2D lutDay;

		internal static Texture2D lutNight;

		internal static void LoadAssets()
		{
			ModDebug.LogThisMethod();

			string modDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			if (FestivalManager.CurrentFestival != Festival.None)
			{
				try
				{
					string lutDayPath = Path.Combine(modDirectory, "textures", $"lut_day_{FestivalManager.FestivalAnimAffix}.png");
					byte[] lutDayBytes = File.ReadAllBytes(lutDayPath);

					lutDay = new Texture2D(2, 2);
					lutDay.LoadImage(lutDayBytes);
				}
				catch
				{
					lutDay = null;
				}

				if (lutDay != null) Debug.Log("FestiveDecor: Loaded LUT for day.");

				try
				{
					string lutNightPath = Path.Combine(modDirectory, "textures", $"lut_night_{FestivalManager.FestivalAnimAffix}.png");
					byte[] lutNightBytes = File.ReadAllBytes(lutNightPath);

					lutNight = new Texture2D(2, 2);
					lutNight.LoadImage(lutNightBytes);
				}
				catch
				{
					lutNight = null;
				}

				if (lutNight != null) Debug.Log("FestiveDecor: Loaded LUT for night.");
			}
		}

		internal static KAnimFile GetAnim(string original)
		{
			return Assets.GetAnim($"{original}_{FestivalManager.FestivalAnimAffix}_kanim");
		}
	}
}
