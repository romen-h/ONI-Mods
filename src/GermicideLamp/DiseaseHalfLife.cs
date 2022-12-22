using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;

using Klei.AI;

using RomenH.Common;

namespace RomenH.GermicideLamp
{
	public static class DiseaseHalfLife
	{
		private static int diseaseCount = 0;
		private static readonly Dictionary<int, Func<float>> halfLifeDelegates = new Dictionary<int, Func<float>>();

		private static float GetDefaultUVHalfLife() => ModSettings.Instance.DefaultModdedGermsHalfLife;

		private static float GetFoodPoisoningUVHalfLife() => ModSettings.Instance.FoodPoisoningHalfLife;

		private static float GetPollenUVHalfLife() => ModSettings.Instance.PollenHalfLife;

		private static float GetRadiationUVHalfLife() => 0f;

		private static float GetSlimelungUVHalfLife() => ModSettings.Instance.SlimelungHalfLife;

		private static float GetZombieSporeUVHalfLife() => ModSettings.Instance.ZombieSporeHalfLife;

		private static bool exponentsDirty = true;
		private static bool[] enabledDiseases = null;
		private static float[] exponents = null;

		public static void PrecomputeExponents()
		{
			if (exponentsDirty)
			{
				for (int i = 0; i < diseaseCount; i++)
				{
					float hl = halfLifeDelegates[i].Invoke();
					enabledDiseases[i] = hl != 0 && !float.IsNaN(hl) && !float.IsInfinity(hl);
					exponents[i] = 0.2f / hl;
				}
				exponentsDirty = false;
			}
		}

		public static bool GetEnabled(int diseaseIndex)
		{
			if (diseaseIndex == byte.MaxValue) return false;
			return enabledDiseases[diseaseIndex];
		}

		public static float GetExponent(int diseaseIndex)
		{
			return exponents[diseaseIndex];
		}

		public static void MarkExponentsDirty()
		{
			exponentsDirty = true;
		}

		[HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
		public static class Db_Initialize_Patch
		{
			[HarmonyPriority(1)] // Don't do this unless you're a Senior Modder.
			public static void Postfix()
			{
				Debug.Log("GermicidalLamp: Setting up DiseaseHalfLife system...");
				var db = Db.Get();
				diseaseCount = db.Diseases.Count;
				Debug.Log($"GermicidalLamps: Db has {diseaseCount} diseases.");
				enabledDiseases = new bool[diseaseCount];
				exponents = new float[diseaseCount];
				for (int i = 0; i < diseaseCount; i++)
				{
					var disease = db.Diseases.GetResource(i) as Disease;

					try
					{
						switch (disease.Id)
						{
							case GameStrings.Diseases.FoodPoisoning:
								halfLifeDelegates[i] = GetFoodPoisoningUVHalfLife;
								break;

							case GameStrings.Diseases.Pollen:
								halfLifeDelegates[i] = GetPollenUVHalfLife;
								break;

							case GameStrings.Diseases.Radiation:
								halfLifeDelegates[i] = GetRadiationUVHalfLife;
								break;

							case GameStrings.Diseases.Slimelung:
								halfLifeDelegates[i] = GetSlimelungUVHalfLife;
								break;

							case GameStrings.Diseases.ZombieSpores:
								halfLifeDelegates[i] = GetZombieSporeUVHalfLife;
								break; ;

							default:
								var killRateProperty = disease.GetType().GetProperty("UVHalfLife", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
								halfLifeDelegates[i] = (Func<float>)killRateProperty.GetGetMethod().CreateDelegate(typeof(Func<float>), disease);
								break;
						}

						Debug.Log($"GermicidalLamps: Found disease {disease.Name} with UVHalfLife = {halfLifeDelegates[i].Invoke()}");
					}
					catch (Exception ex)
					{
						halfLifeDelegates[i] = GetDefaultUVHalfLife;
						Debug.LogWarning($"GermicidalLamps: Failed to get UVHalfLife delegate for disease {disease.Name}. This disease will use the default half-life value from the settings.");
					}
				}
			}
		}
	}
}
