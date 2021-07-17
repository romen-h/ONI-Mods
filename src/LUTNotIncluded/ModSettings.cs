using Newtonsoft.Json;

using PeterHan.PLib;
using PeterHan.PLib.Options;

namespace RomenH.LUTNotIncluded
{
	[RestartRequired]
	[JsonObject(MemberSerialization.OptIn)]
	public class ModSettings
	{
		[JsonProperty]
		[Option("Enable Color Correction", "Determines whether custom color correction will be enabled.")]
		public bool EnableColorCorrection
		{ get; set; }

		[JsonProperty]
		[Option("Always Day", "Forces the daytime color correction to be used all the time.")]
		public bool AlwaysDay
		{ get; set; }

		public ModSettings()
		{
			EnableColorCorrection = true;
		}
	}
}
