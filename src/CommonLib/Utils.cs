using STRINGS;
using System.Collections.Generic;

namespace RomenH.Common
{
	/// <summary>
	/// A class that provides static utility methods for buildings.
	/// </summary>
	public static class BuildingUtils
	{
		private static PlanScreen.PlanInfo GetMenu(HashedString category)
		{
			foreach (var menu in TUNING.BUILDINGS.PLANORDER)
			{
				if (menu.category == category) return menu;
			}

			throw new System.Exception("The plan menu was not found in TUNING.BUILDINGS.PLANORDER.");
		}

		public static void AddBuildingToPlanScreen(string buildingID, HashedString category, string addAferID = null)
		{
			var categoryMenu = GetMenu(category);
			if (categoryMenu.data is List<string> buildings)
			{
				if (addAferID != null)
				{
					var i = buildings.IndexOf(addAferID);
					if (i == -1 || i == buildings.Count - 1) buildings.Add(buildingID);
					else buildings.Insert(i + 1, buildingID);
				}
				else
				{
					buildings.Add(buildingID);
				}
			}
		}

		public static void AddBuildingToTech(string buildingID, string techID)
		{
			var tech = Db.Get().Techs.Get(techID);
			if (tech != null)
			{
				tech.unlockedItemIDs.Add(buildingID);
			}
			else
			{
				Debug.LogWarning($"AddBuildingToTech() Failed to find tech ID: {techID}");
			}
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
			Strings.Add($"STRINGS.{prefix.ToUpperInvariant()}.STATUSITEMS.{id.ToUpperInvariant()}.NAME", name);
			Strings.Add($"STRINGS.{prefix.ToUpperInvariant()}.STATUSITEMS.{id.ToUpperInvariant()}.TOOLTIP", tooltip);
		}

		public static void AddSideScreenStrings(string key, string title, string tooltip)
		{
			Strings.Add($"STRINGS.UI.UISIDESCREENS.{key.ToUpperInvariant()}.TITLE" , title);
			Strings.Add($"STRINGS.UI.UISIDESCREENS.{key.ToUpperInvariant()}.TOOLTIP", tooltip);
		}
	}
}
