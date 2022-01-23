using System;

using HarmonyLib;

using KMod;

using RomenH.Common;

namespace RomenH.PermeablePlasticTiles
{
	public class Mod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			ModCommon.Init("Permeable Plastic Tiles");

			base.OnLoad(harmony);
		}
	}
}
