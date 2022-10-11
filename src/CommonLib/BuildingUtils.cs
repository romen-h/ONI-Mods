using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using STRINGS;

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

			Debug.LogWarning($"Failed to find plan menu.");
			throw new Exception();
		}

		public static void AddBuildingToPlanScreen(string buildingID, HashedString category, string addAferID = null, string subcategory = "")
		{
			if (addAferID != null)
			{
				ModUtil.AddBuildingToPlanScreen(category, buildingID, subcategory, addAferID, ModUtil.BuildingOrdering.After);
			}
			else
			{
				ModUtil.AddBuildingToPlanScreen(category, buildingID, subcategory);
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
				Debug.LogWarning($"Failed to find tech ID: {techID}");
			}
		}
	}
}
