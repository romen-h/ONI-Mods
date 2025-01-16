using System.IO;
using System.Net.Mime;
using System.Reflection;

using HarmonyLib;

using KMod;

using PeterHan.PLib.Core;
using PeterHan.PLib.Options;

using RomenH.Common;
using UnityEngine;

namespace RomenH.FPSLimiter
{
	public class Mod : UserMod2
	{
		internal static string ModFolder
		{ get; private set; }

		public override void OnLoad(Harmony harmony)
		{
			ModCommon.Init("Threshold Walls", harmony);
			PUtil.InitLibrary();

			var options = new POptions();
			options.RegisterOptions(this, typeof(ModSettings));

			ModFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			base.OnLoad(harmony);

			UpdateFPSLimit();
		}

		public static void UpdateFPSLimit()
		{
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = ModSettings.Instance.FPSLimit;
		}
	}
}
