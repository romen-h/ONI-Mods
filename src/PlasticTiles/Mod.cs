using System;

using HarmonyLib;

using KMod;

using RomenH.Common;

namespace RomenH.PlasticTiles
{
	public class Mod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			ModCommon.Init("Plastic Mesh & Airflow Tiles", harmony);

			base.OnLoad(harmony);
		}
	}
}
