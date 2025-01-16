using System.Collections.Generic;

using Newtonsoft.Json;

using PeterHan.PLib.Options;

using RomenH.Common;

namespace RomenH.FPSLimiter
{
	[JsonObject(MemberSerialization.OptIn)]
	[ModInfo("FPS Limiter", "https://github.com/romen-h/ONI-Mods")]
	[RestartRequired]
	public class ModSettings : SingletonOptions<ModSettings>
	{
		[JsonProperty]
		[Option("FPS Limit")]
		[Limit(1, 1000)]
		public int FPSLimit
		{ get; set; }

		public ModSettings()
		{
			FPSLimit = 120;
		}
	}
}
