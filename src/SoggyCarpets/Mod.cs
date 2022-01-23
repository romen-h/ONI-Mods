using System.IO;
using System.Reflection;

using HarmonyLib;

using KMod;

using RomenH.Common;

namespace SoggyCarpets
{
	public class Mod : UserMod2
	{
		internal static string ModFolder
		{ get; private set; }

		public override void OnLoad(Harmony harmony)
		{
			ModCommon.Init("Soggy Carpet");

			base.OnLoad(harmony);
		}
	}
}
