using Newtonsoft.Json;

using PeterHan.PLib.Options;

using UnityEngine;

namespace RomenH.GermicideLamp
{
	[PeterHan.PLib.Options.RestartRequired]
	[JsonObject(MemberSerialization.OptIn)]
	[PeterHan.PLib.Options.ModInfo("Germicidal UV Lamps", "https://github.com/romen-h/ONI-Mods")]
	public class ModSettings : SingletonOptions<ModSettings>
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

		[JsonProperty]
		[Option("Kill Radiation Contaminents (Spaced Out!)", "Determines whether UVC light kills radiation contaminents.", category: "General")]
		public bool UVCKillsRadiation
		{ get; set; }

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
		[Option("Range", "The range of the light from the lamp.", category: "Germicidal UV Lamp")]
		public int BigLampRange
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
		[Option("Horiztonal Range", "The horiztonal range of the tiles affected by the lamp.", category: "Germicidal Ceiling Light")]
		public int CeilingLampRangeWidth
		{ get; set; }

		[JsonProperty]
		[Option("Vertical Range", "The vertical range of the tiles affected by the lamp.", category: "Germicidal Ceiling Light")]
		public int CeilingLampRangeHeight
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
		[Option("Sun Bug Range", "The range of UV light from Sun Bugs.", category: "Shine Bugs")]
		[Limit(1, 5)]
		public int SunBugRange
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

		[JsonProperty]
		[Option("Royal Bug Range", "The range of UV light from Royal Bugs.", category: "Shine Bugs")]
		[Limit(1, 5)]
		public int RoyalBugRange
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
			BigLampRange = 2;
			BigLampHeat = 1f;

			CeilingLampGivesSunburn = false;
			CeilingLampPowerCost = 50f;
			CeilingLampGermicidalStrength = 0.2f;
			CeilingLampRangeWidth = 4;
			CeilingLampRangeHeight = 4;
			CeilingLampLux = 600;
			CeilingLampHeat = 0.1f;

			EnableUVSunBugs = true;
			SunBugStrength = 0.1f;
			SunBugRange = 2;

			EnableUVRoyalBugs = true;
			RoyalBugStrength = 0.5f;
			RoyalBugRange = 3;
		}
	}
}
