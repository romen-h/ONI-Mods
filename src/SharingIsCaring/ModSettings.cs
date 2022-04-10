
using Newtonsoft.Json;

using PeterHan.PLib.Options;

namespace RomenH.SharingIsCaring
{
	[JsonObject(MemberSerialization.OptIn)]
	[ModInfo("https://github.com/romen-h/ONI-Mods")]
	public class ModSettings : SingletonOptions<ModSettings>
	{
		[JsonProperty]
		[Option("Share Beds")]
		public bool ShareBeds
		{ get; set; }

		[JsonProperty]
		[Option("Share Tables")]
		public bool ShareTables
		{ get; set; }

		public ModSettings()
		{
			ShareBeds = true;
			ShareTables = true;
		}
	}
}
