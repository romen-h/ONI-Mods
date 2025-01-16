using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using PeterHan.PLib.Options;
using RomenH.Common;

namespace RomenH.SoggyCarpets
{
	[JsonObject(MemberSerialization.OptIn)]
	[ModInfo("Dumping Sign", "https://github.com/romen-h/ONI-Mods")]
	public class ModSettings : SingletonOptions<ModSettings>, IOptions
	{
		[JsonProperty]
		[Option("Storage Capacity", "Determines the storage capacity of the carpets in kg.")]
		public float CarpetStorageCapacity
		{ get; set; }

		[JsonProperty]
		[Option("Drip Rate", "Determines how fast liquid drips out of the carpet in kg/s.")]
		public float CarpetDripRate
		{ get; set; }

		[JsonProperty]
		[Option("Output Pressure", "Determines the mass limit for the cell below the carpet to prevent dripping. (kg)")]
		public float CarpetOutputPressure
		{ get; set; }

		[JsonProperty]
		[Option("Debug Logging", "Determines whether this mod will print tons of extra log lines.")]
		public bool DebugLogging
		{ get; set; }

		public ModSettings()
		{
			CarpetStorageCapacity = 500f;
			CarpetDripRate = 0.5f;
			CarpetOutputPressure = 500f;
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
