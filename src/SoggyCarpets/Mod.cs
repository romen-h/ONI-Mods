using System.IO;
using System.Reflection;

using HarmonyLib;

using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using RomenH.Common;
using RomenH.SoggyCarpets;

namespace RomenH.SoggyCarpets
{
	public class Mod : UserMod2
	{
		internal static string ModFolder
		{ get; private set; }

		public override void OnLoad(Harmony harmony)
		{
			ModCommon.Init("Soggy Carpets", harmony, false);

			PUtil.InitLibrary();

			var options = new POptions();
			options.RegisterOptions(this, typeof(ModSettings));

			base.OnLoad(harmony);
		}
	}
}
