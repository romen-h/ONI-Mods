using System.IO;
using System.Reflection;
using FMOD.Studio;
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

			FMODUnity.RuntimeManager.StudioSystem.getMemoryUsage(out MEMORY_USAGE usage);
			ModCommon.Log.Info($"FMOD Memory Usage: ex={usage.exclusive}, in={usage.inclusive}, sd={usage.sampledata}");

			base.OnLoad(harmony);
		}
	}
}
