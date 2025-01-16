using System.IO;
using System.Reflection;

using HarmonyLib;

using KMod;

using RomenH.Common;

namespace RomenH.LogicScheduleSensor
{
	public class Mod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			ModCommon.Init("Schedule Sensor", harmony, false);

			base.OnLoad(harmony);
		}
	}
}
