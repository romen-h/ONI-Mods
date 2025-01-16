
using HarmonyLib;

using Rendering;
using SoggyCarpets;
using UnityEngine;

namespace RomenH.SoggyCarpets
{
	[HarmonyPatch(typeof(CarpetTileConfig))]
	[HarmonyPatch("DoPostConfigureComplete")]
	public static class CarpetTileConfig_Patch
	{
		public static void Postfix(GameObject go)
		{
			var storage = go.AddOrGet<Storage>();
			storage.capacityKg = 500f;
			storage.showCapacityStatusItem = true;

			var elementConsumer = go.AddOrGet<PassiveElementConsumer>();
			elementConsumer.configuration = ElementConsumer.Configuration.AllLiquid;
			elementConsumer.consumptionRate = 1f;
			elementConsumer.storeOnConsume = true;
			elementConsumer.showInStatusPanel = true;
			elementConsumer.sampleCellOffset = new Vector3(0, 1);
			elementConsumer.consumptionRadius = 1;
			elementConsumer.storage = storage;

			var carpetDrip = go.AddOrGet<SoggyCarpet>();
		}
	}

	[HarmonyPatch(typeof(BlockTileRenderer))]
	[HarmonyPatch("GetCellColour")]
	public static class BlockTileRenderer_GetCellColour_Patch
	{
		private static readonly Color waterLoggedColor = new Color(0.5f, 0.5f, 0.5f, 1f);

		public static void Postfix(int cell, Color __result)
		{
			if (SoggyCarpet.SoggyCells.Contains(cell))
			{
				__result = waterLoggedColor * __result;
			}
		}
	}
}
