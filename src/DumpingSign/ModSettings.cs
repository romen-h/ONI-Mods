
using System.Collections.Generic;

using Newtonsoft.Json;
using PeterHan.PLib.Options;
using RomenH.Common;

namespace RomenH.DumpingSign
{
	[JsonObject(MemberSerialization.OptIn)]
	[ModInfo("Dumping Sign", "https://github.com/romen-h/ONI-Mods")]
	public class ModSettings : SingletonOptions<ModSettings>, IOptions
	{
		[JsonProperty]
		[Option("Cancel Sweep Chores", "Determines whether the sign will auto-cancel any sweep chores that would cause an infinite delivery loop.")]
		public bool CancelSweepChores
		{ get; set; }

		[JsonProperty]
		[Option("Debug Logging", "Determines whether this mod will print tons of extra log lines.")]
		public bool DebugLogging
		{ get; set; }

		public ModSettings()
		{
			CancelSweepChores = true;
			DebugLogging = false;
		}

		public IEnumerable<IOptionsEntry> CreateOptions()
		{
			return null;
		}

		public void OnOptionsChanged()
		{
			ModCommon.Log.DebugLoggingEnabled = DebugLogging;
		}
	}
}
