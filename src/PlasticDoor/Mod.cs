using System.IO;
using System.Reflection;

using HarmonyLib;

using KMod;

using RomenH.Common;

namespace RomenH.PlasticDoor
{
	public class Mod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			ModCommon.Init("Plastic Door", harmony);

			base.OnLoad(harmony);
		}
	}
}
