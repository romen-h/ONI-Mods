using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomenMods.StirlingEngineMod
{
	[JsonObject(MemberSerialization.OptIn)]
	public class ModSettings
	{
		[JsonProperty]
		public float MinimumTemperatureDifference
		{ get; set; }

		[JsonProperty]
		public float MaxWattOutput
		{ get; set; }

		[JsonProperty]
		public float DTUPerWatt
		{ get; set; }

		[JsonProperty]
		public float WasteHeatRatio
		{ get; set; }

		public ModSettings()
		{
			MinimumTemperatureDifference = 10f;
			MaxWattOutput = 100f;
			DTUPerWatt = 100f;
			WasteHeatRatio = 0.1f;
		}
	}
}
