using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;

namespace RomenH.StirlingEngine
{
	public class Mod : UserMod2
	{
		internal static readonly ModSettings DefaultSettings = new ModSettings();
		internal static ModSettings Settings;

		internal POptions Options
		{ get; private set; }

		public override void OnLoad(Harmony harmony)
		{
			PUtil.InitLibrary();

			Options = new POptions();

			Settings = POptions.ReadSettings<ModSettings>();
			if (Settings == null)
			{
				Settings = new ModSettings();
				POptions.WriteSettings(Settings);
			}
			Options.RegisterOptions(this, typeof(ModSettings));

			ModStrings.STRINGS.BUILDINGS.STIRLINGENGINE.DESC = $"Draws up to {StirlingEngine.WattsToHeat(Mod.Settings.MaxWattOutput):F0} DTU/s of heat from the cell below the floor and converts it to power. The amount of heat drawn is based on the ratio of building temperature vs temperature below the floor tile.";

			base.OnLoad(harmony);
		}
	}
}
