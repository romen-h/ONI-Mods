using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace RomenH.FestiveDecor
{
	public static class Util
	{
		public static void ReplaceAnim(BuildingDef def, string animName)
		{
			if (FestivalManager.CurrentFestival == Festival.None) return;

			KAnimFile anim = ModAssets.GetAnim(animName);
			if (anim != null)
			{
				def.AnimFiles = new KAnimFile[1] { anim };
			}
		}

		public static void ReplaceAnim(GameObject obj, string animName)
		{
			if (FestivalManager.CurrentFestival == Festival.None) return;

			KAnimFile anim = ModAssets.GetAnim(animName);
			if (anim != null)
			{
				var ac = obj.GetComponent<KBatchedAnimController>();
				if (ac != null)
				{
					ac.AnimFiles = new KAnimFile[1] { anim };
				}
			}
		}
	}
}
