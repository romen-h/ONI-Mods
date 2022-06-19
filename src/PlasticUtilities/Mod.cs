using System;

using HarmonyLib;

using KMod;

using RomenH.Common;

namespace RomenH.PlasticUtilities
{
	public class Mod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			ModCommon.Init("Plastic Utilities");

			base.OnLoad(harmony);
		}
	}
}
