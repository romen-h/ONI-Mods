using System.IO;
using System.Reflection;

using HarmonyLib;

using KMod;

using RomenH.Common;

namespace RomenH.LogicScheduleSensor
{
	public class Mod : UserMod2
	{
		internal static string ModFolder
		{ get; private set; }

		public override void OnLoad(Harmony harmony)
		{
			ModCommon.Init("Schedule Sensor");

			base.OnLoad(harmony);
		}
	}
}
