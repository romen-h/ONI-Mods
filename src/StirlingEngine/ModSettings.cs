using Newtonsoft.Json;

using PeterHan.PLib.Options;

namespace RomenH.StirlingEngine
{
	[RestartRequired]
	[JsonObject(MemberSerialization.OptIn)]
	[ModInfo("Stirling Engine", "https://github.com/romen-h/ONI-Mods")]
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
