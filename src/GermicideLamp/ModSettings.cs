using Newtonsoft.Json;
using UnityEngine;

using Option = PeterHan.PLib.Options.OptionAttribute;
using Limit = PeterHan.PLib.Options.LimitAttribute;

namespace RomenH.GermicideLamp
{
	[PeterHan.PLib.Options.RestartRequired]
	[JsonObject(MemberSerialization.OptIn)]
	[PeterHan.PLib.Options.ModInfo("Germicidal UV Lamps", "https://github.com/romen-h/ONI-Mods")]
	public class ModSettings
	{
		#region General

		internal Color LightColor
		{ get; private set; }

		[JsonProperty]
		[Option("Kill Food Poisoning", "Determines whether UVC light kills food poisoning germs.", category: "General")]
		public bool UVCKillsFoodPoisoning
		{ get; set; }

		[JsonProperty]
		[Option("Kill Slimelung", "Determines whether UVC light kills slimelung germs.", category: "General")]
		public bool UVCKillsSlimelung
		{ get; set; }

		[JsonProperty]
		[Option("Kill Zombie Spores", "Determines whether UVC light kills zombie spores.", category: "General")]
		public bool UVCKillsZombieSpores
		{ get; set; }

#if SPACED_OUT
		[JsonProperty]
		[DlcOnlyOption("Kill Radiation Contaminents", "Determines whether UVC light kills radiation contaminents.", category: "General")]
		public bool UVCKillsRadiation
		{ get; set; }
#endif

		#endregion

		#region Big Lamp

		[JsonProperty]
		[Option("Gives Dupes Sunburn", "Determines whether dupes that go near the lamp will receive sunburn.", category: "Germicidal UV Lamp (Big)")]
		public bool BigLampGivesSunburn
		{ get; set; }

		[JsonProperty]
		[Option("Power Consumption", "The amount of power consumed by the lamp in Watts.", category: "Germicidal UV Lamp (Big)", Format = "F0")]
		public float BigLampPowerCost
		{ get; set; }

		[JsonProperty]
		[Option("Strength", "The percentage of germs killed per tick. (1.0 = 100%)", category: "Germicidal UV Lamp (Big)", Format = "F2")]
		[Limit(0f, 1f)]
		public float BigLampGermicidalStrength
		{ get; set; }

		[JsonProperty]
		[Option("Heat Output", "The amount of heat produced by the lamp in kDTU/s.", category: "Germicidal UV Lamp (Big)", Format = "F3")]
		public float BigLampHeat
		{ get; set; }

#endregion

#region Ceiling Light

		[JsonProperty]
		[Option("Gives Dupes Sunburn", "Determines whether dupes that go near the lamp will receive sunburn.", category: "Germicidal Ceiling Light")]
		public bool CeilingLampGivesSunburn
		{ get; set; }

		[JsonProperty]
		[Option("Power Consumption", "The amount of power consumed by the light in Watts.", category: "Germicidal Ceiling Light", Format = "F0")]
		public float CeilingLampPowerCost
		{ get; set; }

		[JsonProperty]
		[Option("Strength", "The percentage of germs killed per tick. (1.0 = 100%)", category: "Germicidal Ceiling Light", Format = "F2")]
		[Limit(0f, 1f)]
		public float CeilingLampGermicidalStrength
		{ get; set; }

		[JsonProperty]
		[Option("Lux", "The power of the light in lux.", category: "Germicidal Ceiling Light")]
		public int CeilingLampLux
		{ get; set; }

		[JsonProperty]
		[Option("Heat Output", "The amount of heat produced by the light in kDTU/s.", category: "Germicidal Ceiling Light", Format = "F3")]
		public float CeilingLampHeat
		{ get; set; }

#endregion

#region ShineBugs

		[JsonProperty]
		[Option("Enable UV Sun Bugs", "Determines whether Sun Bugs will emit UVC light.", "Shine Bugs")]
		public bool EnableUVSunBugs
		{ get; set; }

		[JsonProperty]
		[Option("Sun Bug Strength", "The percentage of germs killed per tick. (1.0 = 100%)", category: "Shine Bugs", Format = "F2")]
		[Limit(0f, 1f)]
		public float SunBugStrength
		{ get; set; }

		[JsonProperty]
		[Option("Enable UV Royal Bugs", "Determines whether Royal Bugs will emit UVC light.", "Shine Bugs")]
		public bool EnableUVRoyalBugs
		{ get; set; }

		[JsonProperty]
		[Option("Royal Bug Strength", "The percentage of germs killed per tick. (1.0 = 100%)", category: "Shine Bugs", Format = "F2")]
		[Limit(0f, 1f)]
		public float RoyalBugStrength
		{ get; set; }

#endregion

		public ModSettings()
		{
			LightColor = new Color(0, 2f, 2f);

			UVCKillsFoodPoisoning = true;
			UVCKillsSlimelung = true;
			UVCKillsZombieSpores = false;
			UVCKillsRadiation = false;

			BigLampGivesSunburn = true;
			BigLampPowerCost = 1000f;
			BigLampGermicidalStrength = 0.6f;
			BigLampHeat = 1f;

			CeilingLampGivesSunburn = false;
			CeilingLampPowerCost = 50f;
			CeilingLampGermicidalStrength = 0.2f;
			CeilingLampLux = 600;
			CeilingLampHeat = 0.1f;

			EnableUVSunBugs = true;
			SunBugStrength = 0.1f;
			EnableUVRoyalBugs = true;
			RoyalBugStrength = 0.5f;
		}
	}
}
