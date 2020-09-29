using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RomenMods.Common
{
	/// <summary>
	/// A class containing static utility methods for debugging ONI mods.
	/// </summary>
	public static class DebugUtils
	{
		/// <summary>
		/// Prints the name and version of the assembly that calls this method.
		/// </summary>
		public static void PrintModInfo()
		{
			AssemblyName ass = Assembly.GetCallingAssembly().GetName();
			Debug.Log($"Mod Info: {ass.Name} {ass.Version}");
		}
	}
}
