using HarmonyLib;

using KMod;

using PeterHan.PLib.Core;
using PeterHan.PLib.Options;

namespace RomenH.TECBlock
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

			base.OnLoad(harmony);
		}
	}
}
