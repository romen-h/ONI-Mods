using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using PeterHan.PLib.Options;

namespace RomenH.Thresholds
{
	[JsonObject(MemberSerialization.OptIn)]
	[RestartRequired]
	[ModInfo("https://github.com/romen-h/ONI-Mods")]
	public class ModSettings : SingletonOptions<ModSettings>
	{
		[JsonProperty]
		[Option("Hide Pipes and Wires")]
		public bool HidePipesAndWires
		{ get; set; }

		public ModSettings()
		{
			HidePipesAndWires = false;
		}
	}
}
