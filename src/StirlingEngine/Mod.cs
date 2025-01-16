using System.IO;
using System.Reflection;

using HarmonyLib;

using KMod;

using PeterHan.PLib.Core;
using PeterHan.PLib.Options;

using RomenH.Common;
using RomenH.CommonLib;

namespace RomenH.StirlingEngine
{
	public class Mod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			ModCommon.Init("Stirling Engine", harmony);
			PUtil.InitLibrary();

			//AudioUtil.LoadSound("PistonLoop", Path.Combine(ModCommon.Folder, "piston.wav"), true);

			var options = new POptions();
			options.RegisterOptions(this, typeof(ModSettings));

			base.OnLoad(harmony);
		}
	}
}
