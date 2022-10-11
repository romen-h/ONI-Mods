using Newtonsoft.Json;

using PeterHan.PLib.Options;

namespace RomenH.LUTNotIncluded
{
	public enum ForceLUTOptions
	{
		None,
		AlwaysDay,
		AlwaysNight
	}

	[JsonObject(MemberSerialization.OptIn)]
	public class ModSettings : SingletonOptions<ModSettings>
	{
		[JsonProperty]
		[Option("Enable Color Correction", "Determines whether custom color correction will be enabled.")]
		public bool EnableColorCorrection
		{ get; set; }

		[JsonProperty]
		[Option("Always Day/Night", "Determines which color correction will be used all the time.")]
		public ForceLUTOptions ForceLUT
		{ get; set; }

		public ModSettings()
		{
			EnableColorCorrection = true;
			ForceLUT = ForceLUTOptions.None;
		}
	}
}
