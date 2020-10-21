using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RomenMods.FestiveDecorMod
{
	public class CurrentFestivalExample
	{
		public bool IsCurrentlyHalloween()
		{
			Type tFestivalMgr = Type.GetType("RomenMods.FestiveDecorMod, FestivalManager");
			if (tFestivalMgr != null)
			{
				PropertyInfo propCurrentFestival = tFestivalMgr.GetProperty("CurrentFestival");
				int currentFestival = (int)propCurrentFestival.GetValue(null, null);
				return (currentFestival == 2);
			}
			return false;
		}
	}
}
