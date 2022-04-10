using System.IO;
using System.Reflection;

using HarmonyLib;

using KMod;

using PeterHan.PLib.Core;
using PeterHan.PLib.Options;

using RomenH.Common;

namespace RomenH.SharingIsCaring
{
	public class Mod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			ModCommon.Init("Sharing is Caring");
			PUtil.InitLibrary();

			var options = new POptions();
			options.RegisterOptions(this, typeof(ModSettings));

			base.OnLoad(harmony);
		}
	}
}
