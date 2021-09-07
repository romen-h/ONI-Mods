using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace RomenH.TECBlock
{
	[JsonObject(MemberSerialization.OptIn)]
	[ModInfo("https://github.com/romen-h/ONI-Mods")]
	[RestartRequired]
	public class ModSettings : SingletonOptions<ModSettings>
	{
		[JsonProperty]
		[Option("Power Consumption (W)", "Determines the power consumed by the building, in watts.\nDefault = 480 W", Format = "F0")]
		public float Wattage
		{ get; set; }

		[JsonProperty]
		[Option("Efficiency (%)", "Determines the percentage of the wattage that is used to calculate how much heat should be moved.\nDefault = 25 %", Format = "F2")]
		public float Efficiency
		{ get; set; }

		[JsonProperty]
		[Option("Max Temp. Difference (K)", "Determines the maximum temperature difference that the TEC can sustain.\nDefault = 70 K", Format = "F0")]
		public float MaxTemperatureDifference
		{ get; set; }

		[JsonProperty]
		[Option("kDTU/W Coefficient", "Determines how much thermal energy is equivalent to one Watt.\nDefault = 0.1 kDTU/W")]
		public float KiloDTUPerWatt
		{ get; set; }

		public ModSettings()
		{
			Wattage = 480f;
			Efficiency = 0.25f;
			MaxTemperatureDifference = 70f;
			KiloDTUPerWatt = 0.1f;
		}
	}
}
