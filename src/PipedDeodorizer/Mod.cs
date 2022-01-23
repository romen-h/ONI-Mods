using System.IO;
using System.Reflection;

using HarmonyLib;

using KMod;

using PeterHan.PLib.Core;

using RomenH.Common;

namespace RomenH.PipedDeodorizer
{
	public class Mod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			ModCommon.Init("Piped Deodorizer");
			PUtil.InitLibrary();

			base.OnLoad(harmony);
		}
	}
}
