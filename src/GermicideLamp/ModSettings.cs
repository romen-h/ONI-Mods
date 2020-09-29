using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomenMods.GermicideLampMod
{
	[JsonObject(MemberSerialization.OptIn)]
	public class ModSettings
	{
		[JsonProperty]
		public bool BigLampGivesSunburn
		{ get; set; }

		[JsonProperty]
		public float BigLampPowerCost
		{ get; set; }

		[JsonProperty]
		public float BigLampGermicidalStrength
		{ get; set; }

		[JsonProperty]
		public bool CeilingLampGivesSunburn
		{ get; set; }

		[JsonProperty]
		public float CeilingLampPowerCost
		{ get; set; }

		[JsonProperty]
		public float CeilingLampGermicidalStrength
		{ get; set; }

		[JsonProperty]
		public int CeilingLampLux
		{ get; set; }

		public ModSettings()
		{
			BigLampGivesSunburn = true;
			BigLampPowerCost = 1000f;
			BigLampGermicidalStrength = 0.6f;

			CeilingLampGivesSunburn = false;
			CeilingLampPowerCost = 50f;
			CeilingLampGermicidalStrength = 0.2f;
			CeilingLampLux = 600;
		}
	}
}
