using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using PeterHan.PLib.Options;

namespace RomenH.SharingIsCaring
{
	[JsonObject(MemberSerialization.OptIn)]
	[ModInfo("https://github.com/romen-h/ONI-Mods")]
	[RestartRequired]
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
