using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomenMods.Common
{
	/// <summary>
	/// A class that provides static utility methods for buildings.
	/// </summary>
	public static class BuildingUtils
	{
		public static void AddBuildingToPlanScreen(HashedString category, string buildingId, string addAfterBuildingId = null)
		{
			var index = TUNING.BUILDINGS.PLANORDER.FindIndex(x => x.category == category);

			if (index == -1)
				return;

			var planOrderList = TUNING.BUILDINGS.PLANORDER[index].data as IList<string>;
			if (planOrderList == null)
			{
				Debug.Log($"Could not add {buildingId} to the building menu.");
				return;
			}

			var neighborIdx = planOrderList.IndexOf(addAfterBuildingId);

			if (neighborIdx != -1)
				planOrderList.Insert(neighborIdx + 1, buildingId);
			else
				planOrderList.Add(buildingId);
		}

		public static void AddBuildingToTechnology(string tech, string buildingId)
		{
			var techList = new List<string>(Database.Techs.TECH_GROUPING[tech]) { buildingId };
			Database.Techs.TECH_GROUPING[tech] = techList.ToArray();
		}
	}

	/// <summary>
	/// A class that provides static utility methods for strings.
	/// </summary>
	public static class StringUtils
	{
		public static void AddBuildingStrings(string buildingId, string name, string description, string effect)
		{
			Strings.Add($"STRINGS.BUILDINGS.PREFABS.{buildingId.ToUpperInvariant()}.NAME", UI.FormatAsLink(name, buildingId));
			Strings.Add($"STRINGS.BUILDINGS.PREFABS.{buildingId.ToUpperInvariant()}.DESC", description);
			Strings.Add($"STRINGS.BUILDINGS.PREFABS.{buildingId.ToUpperInvariant()}.EFFECT", effect);
		}

		public static void AddStatusItemStrings(string id, string prefix, string name, string tooltip)
		{
			Strings.Add($"STRINGS." + prefix + ".STATUSITEMS." + id.ToUpper() + ".NAME", name);
			Strings.Add($"STRINGS." + prefix + ".STATUSITEMS." + id.ToUpper() + ".TOOLTIP", tooltip);
		}

		public static void AddSideScreenStrings(string key, string title, string tooltip)
		{
			Strings.Add("STRINGS.UI.UISIDESCREENS." + key + ".TITLE" , title);
			Strings.Add("STRINGS.UI.UISIDESCREENS." + key + ".TOOLTIP", tooltip);
		}
	}
}
