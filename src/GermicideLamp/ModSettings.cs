using Newtonsoft.Json;

using PeterHan.PLib.Options;

using UnityEngine;

namespace RomenMods.GermicideLampMod
{
	[RestartRequired]
	[JsonObject(MemberSerialization.OptIn)]
	[PeterHan.PLib.ModInfo("Germicidal UV Lamps", "https://github.com/romen-h/ONI-Mods")]
	public class ModSettings
	{
		#region General

		internal Color LightColor
		{ get; private set; }

		[JsonProperty]
		[PeterHan.PLib.Option("Kill Food Poisoning", "Determines whether UVC light kills food poisoning germs.", category: "General")]
		public bool UVCKillsFoodPoisoning
		{ get; set; }

		[JsonProperty]
		[PeterHan.PLib.Option("Kill Slimelung", "Determines whether UVC light kills slimelung germs.", category: "General")]
		public bool UVCKillsSlimelung
		{ get; set; }

		[JsonProperty]
		[PeterHan.PLib.Option("Kill Zombie Spores", "Determines whether UVC light kills zombie spores.", category: "General")]
		public bool UVCKillsZombieSpores
		{ get; set; }

		#endregion

		#region Big Lamp

		[JsonProperty]
		[PeterHan.PLib.Option("Gives Dupes Sunburn", "Determines whether dupes that go near the lamp will receive sunburn.", category: "Germicidal UV Lamp (Big)")]
		public bool BigLampGivesSunburn
		{ get; set; }

		[JsonProperty]
		[PeterHan.PLib.Option("Power Consumption", "The amount of power consumed by the lamp in Watts.", category: "Germicidal UV Lamp (Big)", Format = "F0")]
		public float BigLampPowerCost
		{ get; set; }

		[JsonProperty]
		[PeterHan.PLib.Option("Strength", "The percentage of germs killed per tick. (1.0 = 100%)", category: "Germicidal UV Lamp (Big)", Format = "F2")]
		[PeterHan.PLib.Limit(0f, 1f)]
		public float BigLampGermicidalStrength
		{ get; set; }

		[JsonProperty]
		[PeterHan.PLib.Option("Heat Output", "The amount of heat produced by the lamp in kDTU/s.", category: "Germicidal UV Lamp (Big)", Format = "F3")]
		public float BigLampHeat
		{ get; set; }

		#endregion

		#region Ceiling Light

		[JsonProperty]
		[PeterHan.PLib.Option("Gives Dupes Sunburn", "Determines whether dupes that go near the lamp will receive sunburn.", category: "Germicidal Ceiling Light")]
		public bool CeilingLampGivesSunburn
		{ get; set; }

		[JsonProperty]
		[PeterHan.PLib.Option("Power Consumption", "The amount of power consumed by the light in Watts.", category: "Germicidal Ceiling Light", Format = "F0")]
		public float CeilingLampPowerCost
		{ get; set; }

		[JsonProperty]
		[PeterHan.PLib.Option("Strength", "The percentage of germs killed per tick. (1.0 = 100%)", category: "Germicidal Ceiling Light", Format = "F2")]
		[PeterHan.PLib.Limit(0f, 1f)]
		public float CeilingLampGermicidalStrength
		{ get; set; }

		[JsonProperty]
		[PeterHan.PLib.Option("Lux", "The power of the light in lux.", category: "Germicidal Ceiling Light")]
		public int CeilingLampLux
		{ get; set; }

		[JsonProperty]
		[PeterHan.PLib.Option("Heat Output", "The amount of heat produced by the light in kDTU/s.", category: "Germicidal Ceiling Light", Format = "F3")]
		public float CeilingLampHeat
		{ get; set; }

		#endregion

		#region ShineBugs

		[JsonProperty]
		[PeterHan.PLib.Option("Enable UV Sun Bugs", "Determines whether Sun Bugs will emit UVC light.", "Shine Bugs")]
		public bool EnableUVSunBugs
		{ get; set; }

		[PeterHan.PLib.Option("Sun Bug Strength", "The percentage of germs killed per tick. (1.0 = 100%)", category: "Shine Bugs", Format = "F2")]
		[PeterHan.PLib.Limit(0f, 1f)]
		public float SunBugStrength
		{ get; set; }

		[JsonProperty]
		[PeterHan.PLib.Option("Enable UV Royal Bugs", "Determines whether Royal Bugs will emit UVC light.", "Shine Bugs")]
		public bool EnableUVRoyalBugs
		{ get; set; }

		[JsonProperty]
		[PeterHan.PLib.Option("Royal Bug Strength", "The percentage of germs killed per tick. (1.0 = 100%)", category: "Shine Bugs", Format = "F2")]
		[PeterHan.PLib.Limit(0f, 1f)]
		public float RoyalBugStrength
		{ get; set; }

		#endregion

		public ModSettings()
		{
			LightColor = new Color(0, 2f, 2f);

			UVCKillsFoodPoisoning = true;
			UVCKillsSlimelung = true;
			UVCKillsZombieSpores = false;

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
