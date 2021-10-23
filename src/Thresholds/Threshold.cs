using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomenH.Thresholds
{
	public class Threshold : KMonoBehaviour
	{
		public static HashSet<int> ThresholdCells = new HashSet<int>();

		[MyCmpGet]
		public Building building;

		[MyCmpGet]
		public KBatchedAnimController anim;

		private int myCell;

		protected override void OnSpawn()
		{
			myCell = building.GetCell();
			ThresholdCells.Add(myCell);
		}

		protected override void OnCleanUp()
		{
			ThresholdCells.Remove(myCell);
		}
	}
}
