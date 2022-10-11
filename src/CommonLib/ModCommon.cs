using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RomenH.Common
{
	public static class ModCommon
	{
		public static string Name
		{ get; private set; }

		public static string Version
		{ get; private set; }

		public static string Folder
		{ get; private set; }

		public static IDictionary<string, object> Registry
		{ get; private set; }

		public static void Init(string name)
		{
			Name = name;

			Assembly asm = Assembly.GetExecutingAssembly();
			Version = asm.GetName().Version.ToString();
			Folder = Path.GetDirectoryName(asm.Location);

			Debug.Log($"Initializing Mod: {Name} ({Version}) by Romen");

			Registry = RomenHRegistry.Init();

			StringUtils.RegisterAllLocStrings();
		}
	}
}
