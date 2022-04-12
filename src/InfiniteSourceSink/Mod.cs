using System.IO;
using System.Reflection;

using HarmonyLib;

using KMod;

using PeterHan.PLib.Core;
using PeterHan.PLib.Options;

using RomenH.Common;

namespace InfiniteSourceSink
{
	public class Mod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			ModCommon.Init("Infinite Gases & Liquids");
			PUtil.InitLibrary();

			var options = new POptions();
			options.RegisterOptions(this, typeof(ModSettings));

			base.OnLoad(harmony);

			Debug.Log($"Loaded Mod: ID={this.mod.staticID}, Version={this.mod.packagedModInfo.version}");
		}
	}
}
