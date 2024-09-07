using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace RomenH.Common
{
	public static class ModCommon
	{
		public static string Name
		{ get; private set; }

		public static Harmony HarmonyInstance
		{ get; private set; }

		public static Logger Log
		{ get; private set; }

		public static string Version
		{ get; private set; }

		public static string Folder
		{ get; private set; }

		public static IDictionary<string, object> Registry
		{ get; private set; }

		public static void Init(string name, Harmony harmony, bool debug = false)
		{
			Name = name;
			HarmonyInstance = harmony;

			Log = new Logger(name);
			if (debug)
			{
				Log.DebugLoggingEnabled = true;
			}

			Assembly asm = Assembly.GetExecutingAssembly();
			Version = asm.GetName().Version.ToString();
			Folder = Path.GetDirectoryName(asm.Location);

			Log.Info($"Initializing {Name} {Version} by Romen...");

			Registry = RomenHRegistry.Init();

			StringUtils.RegisterAllLocStrings();
			StringUtils.ApplyTranslationsPatch(harmony);
		}
	}
}
