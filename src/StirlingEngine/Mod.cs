using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;

namespace RomenH.StirlingEngine
{
	public class Mod : UserMod2
	{
		internal static readonly ModSettings DefaultSettings = new ModSettings();

		internal POptions Options
		{ get; private set; }

		public override void OnLoad(Harmony harmony)
		{
			PUtil.InitLibrary();

			Options = new POptions();
			Options.RegisterOptions(this, typeof(ModSettings));

			ModStrings.STRINGS.BUILDINGS.STIRLINGENGINE.DESC = $"Draws up to {StirlingEngine.WattsToHeat(ModSettings.Instance.MaxWattOutput):F0} DTU/s of heat from the cell below the floor and converts it to power. The amount of heat drawn is based on the ratio of building temperature vs temperature below the floor tile.";

			base.OnLoad(harmony);
		}
	}
}
