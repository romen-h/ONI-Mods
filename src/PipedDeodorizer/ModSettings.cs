using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using PeterHan.PLib.Options;

namespace RomenH.PipedDeodorizer
{
	[JsonObject(MemberSerialization.OptIn)]
	[ModInfo("https://github.com/romen-h/ONI-Mods")]
	[RestartRequired]
	public class ModSettings : SingletonOptions<ModSettings>
	{
		[JsonProperty]
		[Option("Power Consumption (W)", "Determines the power consumed by the building, in watts.\nDefault = 60 W", Format = "F0")]
		[Limit(1,1000)]
		public float Wattage
		{ get; set; }

		[JsonProperty]
		[Option("Heat Generated (kDTU/s)", "Determines the amount of heat generated while the building is working.\nDefault = 1 kDTU/s", Format = "F3")]
		public float HeatGenerated
		{ get; set; }

		[JsonProperty]
		[Option("Intake Rate (kg/s)", "Determines the rate that the deodorizer consumes Polluted Oxygen.\nDefault = 1 kg/s", Format = "F3")]
		[Limit(0.001, 1)]
		public float IntakeRate
		{ get; set; }

		[JsonProperty]
		[Option("Classic Deodorizer", "Toggles whether the deodorizer requires filtration medium and will produce Clay like a normal deodorizer.")]
		public bool ClassicDeodorizer
		{ get; set; }

		public ModSettings()
		{
			Wattage = 60f;
			HeatGenerated = 1f;
			IntakeRate = 1f;
			ClassicDeodorizer = false;
		}
	}
}
