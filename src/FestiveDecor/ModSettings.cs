using Newtonsoft.Json;

using PeterHan.PLib;
using PeterHan.PLib.Options;

namespace RomenMods.FestiveDecorMod
{
	[RestartRequired]
	[JsonObject(MemberSerialization.OptIn)]
	public class ModSettings
	{
		#region Date

		[JsonProperty]
		[PeterHan.PLib.Option("Use Custom Date", "Allows the festival for a particular day of the year to be permanently enabled.", category: "Date")]
		public bool UseOverrideDate
		{ get; set; }

		[JsonProperty]
		[PeterHan.PLib.Option("Custom Month", "", category: "Date")]
		[PeterHan.PLib.Limit(1,12)]
		public int OverrideMonth
		{ get; set; }

		[JsonProperty]
		[PeterHan.PLib.Option("Custom Day", "", category: "Date")]
		[PeterHan.PLib.Limit(1,31)]
		public int OverrideDayOfMonth
		{ get; set; }

		#endregion

		#region General

		[JsonProperty]
		[PeterHan.PLib.Option("Enable Color Correction", "Determines whether custom colour correction will be enabled during festivals.", category: "General")]
		public bool EnableColorCorrection
		{ get; set; }

		[JsonProperty]
		[PeterHan.PLib.Option("Enable Printing Pod Overlay", "Determines whether the printing pod building will have a festive overlay.", category: "General")]
		public bool EnableHQOverlay
		{ get; set; }

		[JsonProperty]
		[PeterHan.PLib.Option("Enable Helmets", "Determines whether the atmo suit helmets will be replaced during festivals.", category: "General")]
		public bool EnableCustomHelmets
		{ get; set; }

		#endregion

		#region Halloween

		[JsonProperty]
		[PeterHan.PLib.Option("Enable Halloween", "", category: "Halloween")]
		public bool EnableHalloween
		{ get; set; }

		[JsonProperty]
		[PeterHan.PLib.Option("Enable Spiders", "Determines whether spider-related content will be visible during Halloween.", category: "Halloween")]
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
