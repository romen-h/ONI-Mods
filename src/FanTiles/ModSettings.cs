
using Newtonsoft.Json;

using PeterHan.PLib.Options;

namespace RomenH.Fans
{
	[PeterHan.PLib.Options.RestartRequired]
	[JsonObject(MemberSerialization.OptIn)]
	[PeterHan.PLib.Options.ModInfo("Fan Tiles", "https://github.com/romen-h/ONI-Mods")]
	public class ModSettings : SingletonOptions<ModSettings>
	{
		// Fan

		[JsonProperty]
		[Option("Flow Rate (kg/s)", "The amount of gas moved per second.", category: "Fan", Format = "F3")]
		public float FanFlowRate
		{ get; set; }

		[JsonProperty]
		[Option("Pressure Limit (kg)", "The max pressure that the fan can pump into.", category: "Fan", Format = "F3")]
		public float FanPressureLimit
		{ get; set; }

		[JsonProperty]
		[Option("Power Consumption (W)", "The power consumed by the fan.", category: "Fan", Format = "F0")]
		public float FanWattage
		{ get; set; }

		// High Pressure Fan

		[JsonProperty]
		[Option("Flow Rate (kg/s)", "The amount of gas moved per second.", category: "High Pressure Fan", Format = "F3")]
		public float HighPressureFanFlowRate
		{ get; set; }

		[JsonProperty]
		[Option("Pressure Limit (kg)", "The max pressure that the fan can pump into.", category: "High Pressure Fan", Format = "F3")]
		public float HighPressureFanPressureLimit
		{ get; set; }

		[JsonProperty]
		[Option("Power Consumption (W)", "The power consumed by the fan.", category: "High Pressure Fan", Format = "F0")]
		public float HighPressureFanWattage
		{ get; set; }

		// Compressor Fan

		[JsonProperty]
		[Option("Flow Rate (kg/s)", "The amount of gas moved per second.", category: "Compressor Fan", Format = "F3")]
		public float CompressorFanFlowRate
		{ get; set; }

		[JsonProperty]
		[Option("Power Consumption (W)", "The power consumed by the fan.", category: "Compressor Fan", Format = "F0")]
		public float CompressorFanWattage
		{ get; set; }

		// Turbine

		[JsonProperty]
		[Option("Flow Rate (kg/s)", "The amount of liquid moved per second.", category: "Turbine", Format = "F3")]
		public float TurbineFlowRate
		{ get; set; }

		[JsonProperty]
		[Option("Pressure Limit (kg)", "The max pressure that the turbine can pump into.", category: "Turbine", Format = "F3")]
		public float TurbinePressureLimit
		{ get; set; }

		[JsonProperty]
		[Option("Power Consumption (W)", "The power consumed by the turbine.", category: "Turbine", Format = "F0")]
		public float TurbineWattage
		{ get; set; }

		// Compressor Turbine

		[JsonProperty]
		[Option("Flow Rate (kg/s)", "The amount of liquid moved per second.", category: "Compressor Turbine", Format = "F3")]
		public float CompressorTurbineFlowRate
		{ get; set; }

		[JsonProperty]
		[Option("Power Consumption (W)", "The power consumed by the turbine.", category: "Compressor Turbine", Format = "F0")]
		public float CompressorTurbineWattage
		{ get; set; }

		public ModSettings()
		{
			FanFlowRate = 0.5f;
			FanPressureLimit = 2f;
			FanWattage = 60f;

			HighPressureFanFlowRate = 1f;
			HighPressureFanPressureLimit = 20f;
			HighPressureFanWattage = 120f;

			CompressorFanFlowRate = 4f;
			CompressorFanWattage = 480f;

			TurbineFlowRate = 5f;
			TurbinePressureLimit = 1000f;
			TurbineWattage = 60f;

			CompressorTurbineFlowRate = 40f;
			CompressorTurbineWattage = 480f;
		}
	}
}
