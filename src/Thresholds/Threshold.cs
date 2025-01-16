using System.Collections.Generic;

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

		public override void OnSpawn()
		{
			myCell = building.GetCell();
			ThresholdCells.Add(myCell);
		}

		public override void OnCleanUp()
		{
			ThresholdCells.Remove(myCell);
		}
	}
}
