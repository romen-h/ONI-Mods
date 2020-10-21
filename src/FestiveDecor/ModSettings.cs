using Newtonsoft.Json;

namespace RomenMods.FestiveDecorMod
{
	[JsonObject(MemberSerialization.OptIn)]
	public class ModSettings
	{
		[JsonProperty]
		public bool UseOverrideDate
		{ get; set; }

		[JsonProperty]
		public int OverrideMonth
		{ get; set; }

		[JsonProperty]
		public int OverrideDayOfMonth
		{ get; set; }

		[JsonProperty]
		public bool EnableHalloween
		{ get; set; }

		[JsonProperty]
		public bool EnableColorCorrection
		{ get; set; }

		[JsonProperty]
		public bool EnableHQOverlay
		{ get; set; }

		[JsonProperty]
		public bool EnableCustomHelmets
		{ get; set; }

		[JsonProperty]
		public bool EnableSpiders
		{ get; set; }

		public ModSettings()
		{
			UseOverrideDate = false;
			OverrideMonth = 1;
			OverrideDayOfMonth = 1;

			EnableHalloween = true;

			EnableColorCorrection = true;
			EnableCustomHelmets = true;
			EnableHQOverlay = true;
			EnableSpiders = true;
		}
	}
}
