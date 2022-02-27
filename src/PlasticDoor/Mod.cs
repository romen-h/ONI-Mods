using System.IO;
using System.Reflection;

using HarmonyLib;

using KMod;

using RomenH.Common;

namespace Curtain
{
	public class Mod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			ModCommon.Init("Plastic Door");

			base.OnLoad(harmony);
		}
	}
}
