using Newtonsoft.Json;

using PeterHan.PLib.Options;

using TUNING;

namespace InfiniteSourceSink
{
	[JsonObject(MemberSerialization.OptIn)]
	[ModInfo("https://github.com/romen-h/ONI-Mods")]
	[RestartRequired]
	public class ModSettings : SingletonOptions<ModSettings>
	{
		[JsonProperty]
		[Option("Sandbox Only", "Determines whether the infinite sources/sinks are made from neutronium or refined metal.")]
		public bool SandboxOnly
		{ get; set; }

		[JsonProperty]
		[Option("Required Mass (kg)", "Determines the required mass for construction the infinite sources/sinks.")]
		public float[] BuildMassKg
		{ get; set; }

		[JsonProperty]
		[Option("Construction Time (s)", "Determines how many seconds it takes to construct the infinite sources/sinks.")]
		public float BuildTimeSeconds
		{ get; set; }

		public ModSettings()
		{
			SandboxOnly = true; // Use Neutronium
			BuildMassKg = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3; // 200 Kg
			BuildTimeSeconds = BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2; // 30s
		}
	}
}
