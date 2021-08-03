using Newtonsoft.Json;

using PeterHan.PLib.Options;

namespace RomenH.StirlingEngine
{
	[JsonObject(MemberSerialization.OptIn)]
	[ModInfo("https://github.com/romen-h/ONI-Mods")]
	[RestartRequired]
	public class ModSettings : SingletonOptions<ModSettings>
	{
		[JsonProperty]
		[Option("Minimum Temperature Difference", "Determines the minimum temperature difference between building and source tile that allows the building to operate.")]
		[Limit(1,100)]
		public float MinimumTemperatureDifference
		{ get; set; }

		[JsonProperty]
		[Option("Power Scale", "Determines the max power output and amount of heat the engine will pump.")]
		[Limit(10,1000)]
		public float MaxWattOutput
		{ get; set; }

		[JsonProperty]
		[Option("Waste Heat Ratio", "Determines the amount of heat that is taken into the building instead of generating power.")]
		[Limit(0,1)]
		public float WasteHeatRatio
		{ get; set; }

		[JsonProperty]
		public float DTUPerWatt
		{ get; set; }

		public ModSettings()
		{
			MinimumTemperatureDifference = 10f;
			MaxWattOutput = 100f;
			WasteHeatRatio = 0.1f;
			DTUPerWatt = 100f;
		}
	}
}
