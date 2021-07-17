using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;

namespace RomenH.LUTNotIncluded
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

			ModAssets.LoadAssets();

			base.OnLoad(harmony);
		}
	}
}
