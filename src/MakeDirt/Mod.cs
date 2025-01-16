using System.IO;
using System.Reflection;

using HarmonyLib;

using KMod;

using RomenH.Common;

namespace RomenH.MakeDirt
{
	public class Mod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			ModCommon.Init("Make Dirt", harmony);

			base.OnLoad(harmony);
		}
	}
}
