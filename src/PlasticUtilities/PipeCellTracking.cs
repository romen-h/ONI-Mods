using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomenH.PlasticUtilities
{
	public class PipeCellTracking : KMonoBehaviour
	{
		[MyCmpGet]
		public Building building;

		[MyCmpGet]
		public Conduit conduit;

		private ISet<int> cells;

		public override void OnSpawn()
		{
			if (conduit.ConduitType == ConduitType.Liquid)
			{
				cells = FlowVisualizerPatches.plasticLiquidConduitCells;
			}
			else if (conduit.ConduitType == ConduitType.Gas)
			{
				cells = FlowVisualizerPatches.plasticGasConduitCells;
			}
			else
			{
				cells = null;
			}

			if (cells != null)
			{
				cells.Add(building.GetCell());
			}
		}

		public override void OnCleanUp()
		{
			if (cells != null)
			{
				cells.Remove(building.GetCell());
			}
		}
	}
}
