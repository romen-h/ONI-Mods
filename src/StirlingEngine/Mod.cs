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
			ModCommon.Init("Stirling Engine");
			PUtil.InitLibrary();

			//AudioUtil.LoadSound("PistonLoop", Path.Combine(ModCommon.Folder, "piston.wav"), true);

			var options = new POptions();
			options.RegisterOptions(this, typeof(ModSettings));

			StirlingEngineConfig.Desc = $"Draws up to {StirlingEngine.WattsToHeat(ModSettings.Instance.MaxWattOutput):F0} DTU/s of heat from the cell below the floor and converts it to power. The amount of heat drawn is based on the ratio of building temperature vs temperature below the floor tile.";

			base.OnLoad(harmony);
		}
	}
}
