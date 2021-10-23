using System.Collections.Generic;

using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;

using RomenH.Common;

namespace RomenH.LUTNotIncluded
{
	public class Mod : UserMod2
	{
		internal static readonly ModSettings DefaultSettings = new ModSettings();
		internal static ModSettings Settings;

		internal POptions Options
		{ get; private set; }

		internal static IDictionary<string,object> Registry
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

			Registry = RomenHRegistry.Init();

			base.OnLoad(harmony);
		}
	}
}
