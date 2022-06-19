using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;

using UnityEngine;

namespace RomenH.PlasticUtilities
{
	public static class FlowVisualizerPatches
	{
		internal static readonly HashSet<int> plasticLiquidConduitCells = new HashSet<int>();
		internal static readonly HashSet<int> plasticGasConduitCells = new HashSet<int>();

		private static readonly Color32 plasticTint = new Color32(94, 128, 172, 255);

		[HarmonyPatch(typeof(ConduitFlowVisualizer), "GetCellTintColour")]
		public static class ConduitFlowVisualizer_GetCellTintColour_Patch
		{
			public static bool Prefix(ref Color32 __result, int cell)
			{
				if (plasticLiquidConduitCells.Contains(cell) || plasticGasConduitCells.Contains(cell))
				{
					__result = plasticTint;
					return false;
				}

				return true;
			}
		}
	}
}
