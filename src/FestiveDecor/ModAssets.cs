using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using UnityEngine;

namespace RomenMods.FestiveDecorMod
{
	internal static class ModAssets
	{
		internal static Texture2D lutDay;

		internal static Texture2D lutNight;

		internal static void LoadAssets()
		{
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
			}
		}
	}
}
