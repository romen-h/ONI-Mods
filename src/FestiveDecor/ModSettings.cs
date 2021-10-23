using Newtonsoft.Json;

using PeterHan.PLib.Options;

namespace RomenH.FestiveDecor
{
	[RestartRequired]
	[JsonObject(MemberSerialization.OptIn)]
	[ModInfo("https://github.com/romen-h/ONI-Mods")]
	public class ModSettings : SingletonOptions<ModSettings>
	{
		#region Date

		[JsonProperty]
		[Option("Use Custom Date", "Allows the festival for a particular day of the year to be permanently enabled.", category: "Date")]
		public bool UseOverrideDate
		{ get; set; }

		[JsonProperty]
		[Option("Custom Month", "", category: "Date")]
		[Limit(1, 12)]
		public int OverrideMonth
		{ get; set; }

		[JsonProperty]
		[Option("Custom Day", "", category: "Date")]
		[Limit(1, 31)]
		public int OverrideDayOfMonth
		{ get; set; }

		#endregion

		#region General

		[JsonProperty]
		[Option("Enable Color Correction", "Determines whether custom colour correction will be enabled during festivals.", category: "General")]
		public bool EnableColorCorrection
		{ get; set; }

		[JsonProperty]
		[Option("Enable Printing Pod Overlay", "Determines whether the printing pod building will have a festive overlay.", category: "General")]
		public bool EnableHQOverlay
		{ get; set; }

		[JsonProperty]
		[Option("Enable Helmets", "Determines whether the atmo suit helmets will be replaced during festivals.", category: "General")]
		public bool EnableCustomHelmets
		{ get; set; }

		#endregion

		#region Halloween

		[JsonProperty]
		[Option("Enable Halloween", "", category: "Halloween")]
		public bool EnableHalloween
		{ get; set; }

		[JsonProperty]
		[Option("Enable Spiders", "Determines whether spider-related content will be visible during Halloween.", category: "Halloween")]
		public bool EnableSpiders
		{ get; set; }

		#endregion

		public ModSettings()
		{
			UseOverrideDate = false;
			OverrideMonth = 1;
			OverrideDayOfMonth = 1;

			EnableColorCorrection = true;
			EnableCustomHelmets = true;
			EnableHQOverlay = true;

			EnableHalloween = true;
			EnableSpiders = true;
		}
	}
}
