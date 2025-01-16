using System.Collections.Generic;
using Newtonsoft.Json;

using PeterHan.PLib.Options;
using RomenH.Common;
using UnityEngine;

namespace RomenH.GermicideLamp
{
	[RestartRequired]
	[JsonObject(MemberSerialization.OptIn)]
	[ModInfo("Germicidal UV Lamps", "https://github.com/romen-h/ONI-Mods")]
	public class ModSettings : SingletonOptions<ModSettings>, IOptions
	{
		#region General

		internal Color LightColor
		{ get; private set; }

		[JsonProperty]
		[Option("Liquid UV Attenuation", "A factor that determines how effective UV light is in liquid cells.", category: "General", Format = "F3")]
		[Limit(0, 1)]
		public float LiquidUVAttenuation
		{ get; set; }

		#endregion

		#region Kill Rates

		[JsonProperty]
		[Option("Food Poisoning Half-Life", "Determines how many seconds it takes for UV light to kill half of the germs.", category: "Disease Resistances", Format = "F3")]
		[Limit(0, 100)]
		public float FoodPoisoningHalfLife
		{ get; set; }

		[JsonProperty]
		[Option("Pollen Half-Life", "Determines how many seconds it takes for UV light to kill half of the germs.", category: "Disease Resistances", Format = "F3")]
		[Limit(0, 100)]
		public float PollenHalfLife
		{ get; set; }

		[JsonProperty]
		[Option("Slimelung Half-Life", "Determines how many seconds it takes for UV light to kill half of the germs.", category: "Disease Resistances", Format = "F3")]
		[Limit(0, 100)]
		public float SlimelungHalfLife
		{ get; set; }

		[JsonProperty]
		[Option("Zombie Spore Half-Life", "Determines how many seconds it takes for UV light to kill half of the germs.", category: "Disease Resistances", Format = "F3")]
		[Limit(0, 100)]
		public float ZombieSporeHalfLife
		{ get; set; }

		[JsonProperty]
		[Option("Modded Germ Half-Life (Fallback)", "Determines how many seconds it takes for UV light to kill half of the germs.", category: "Disease Resistances", Format = "F3")]
		[Limit(0, 100)]
		public float DefaultModdedGermsHalfLife
		{ get; set; }

		#endregion

		#region Big Lamp

		[JsonProperty]
		[Option("Gives Dupes Sunburn", "Determines whether dupes that go near the lamp will receive sunburn.", category: "Germicidal UV Lamp (Big)")]
		public bool BigLampGivesSunburn
		{ get; set; }

		[JsonProperty]
		[Option("Power Consumption", "The amount of power consumed by the lamp in Watts.", category: "Germicidal UV Lamp (Big)", Format = "F0")]
		[Limit(0, 10000)]
		public float BigLampPowerCost
		{ get; set; }

		[JsonProperty]
		[Option("Strength", "A coefficient that determines how powerful the UV light is. (1.0 = 100%)", category: "Germicidal UV Lamp (Big)", Format = "F2")]
		[Limit(0f, 5f)]
		public float BigLampGermicidalStrength
		{ get; set; }

		[JsonProperty]
		[Option("Range", "The range of the light from the lamp.", category: "Germicidal UV Lamp")]
		[Limit(0, 5)]
		public int BigLampRange
		{ get; set; }

		[JsonProperty]
		[Option("Heat Output", "The amount of heat produced by the lamp in kDTU/s.", category: "Germicidal UV Lamp (Big)", Format = "F3")]
		[Limit(0, 1000)]
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
		[Limit(0, 10000)]
		public float CeilingLampPowerCost
		{ get; set; }

		[JsonProperty]
		[Option("Strength", "A coefficient that determines how powerful the UV light is. (1.0 = 100%)", category: "Germicidal Ceiling Light", Format = "F2")]
		[Limit(0, 5)]
		public float CeilingLampGermicidalStrength
		{ get; set; }

		[JsonProperty]
		[Option("Horiztonal Range", "The horiztonal range of the tiles affected by the lamp.", category: "Germicidal Ceiling Light")]
		[Limit(2, 6)]
		public int CeilingLampRangeWidth
		{ get; set; }

		[JsonProperty]
		[Option("Vertical Range", "The vertical range of the tiles affected by the lamp.", category: "Germicidal Ceiling Light")]
		[Limit(1, 16)]
		public int CeilingLampRangeHeight
		{ get; set; }

		[JsonProperty]
		[Option("Lux", "The power of the light in lux.", category: "Germicidal Ceiling Light")]
		[Limit(0, 10000)]
		public int CeilingLampLux
		{ get; set; }

		[JsonProperty]
		[Option("Heat Output", "The amount of heat produced by the light in kDTU/s.", category: "Germicidal Ceiling Light", Format = "F3")]
		[Limit(0, 1000)]
		public float CeilingLampHeat
		{ get; set; }

		#endregion

		#region ShineBugs

		[JsonProperty]
		[Option("Enable UV Sun Bugs", "Determines whether Sun Bugs will emit UVC light.", "Shine Bugs")]
		public bool EnableUVSunBugs
		{ get; set; }

		[JsonProperty]
		[Option("Sun Bug Strength", "A coefficient that determines how powerful the UV light is. (1.0 = 100%)", category: "Shine Bugs", Format = "F2")]
		[Limit(0, 5)]
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
		[Option("Royal Bug Strength", "A coefficient that determines how powerful the UV light is. (1.0 = 100%)", category: "Shine Bugs", Format = "F2")]
		[Limit(0, 5)]
		public float RoyalBugStrength
		{ get; set; }

		[JsonProperty]
		[Option("Royal Bug Range", "The range of UV light from Royal Bugs.", category: "Shine Bugs")]
		[Limit(1, 5)]
		public int RoyalBugRange
		{ get; set; }

		#endregion

		[JsonProperty]
		[Option("Debug Logging", "Determines whether this mod will print tons of extra log lines.")]
		public bool DebugLogging
		{ get; set; }

		public ModSettings()
		{
			LightColor = new Color(0, 2f, 2f);
			LiquidUVAttenuation = 0.2f;

			FoodPoisoningHalfLife = 1.0f;
			PollenHalfLife = 0.0f;
			SlimelungHalfLife = 3.0f;
			ZombieSporeHalfLife = 30.0f;
			DefaultModdedGermsHalfLife = 0.0f;

			BigLampGivesSunburn = true;
			BigLampPowerCost = 1000f;
			BigLampGermicidalStrength = 1.0f;
			BigLampRange = 2;
			BigLampHeat = 1f;

			CeilingLampGivesSunburn = false;
			CeilingLampPowerCost = 50f;
			CeilingLampGermicidalStrength = 0.05f;
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

			DebugLogging = false;
		}

		public IEnumerable<IOptionsEntry> CreateOptions()
		{
			return null;
		}

		public void OnOptionsChanged()
		{
			ModCommon.Log.DebugLoggingEnabled = DebugLogging;
		}
	}
}
