using System.IO;
using System.Reflection;

using HarmonyLib;

using KMod;

using PeterHan.PLib.Core;

using RomenH.Common;

namespace RomenH.Thresholds
{
	public class Mod : UserMod2
	{
		internal static string ModFolder
		{ get; private set; }

		public override void OnLoad(Harmony harmony)
		{
			ModCommon.Init("Threshold Walls");
			PUtil.InitLibrary();

			ModFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			base.OnLoad(harmony);
		}
	}
}
