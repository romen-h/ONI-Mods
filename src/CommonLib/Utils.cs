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
			bool fallback = false;
			if (addAferID != null)
			{
				try
				{
					var categoryMenu = GetMenu(category);
					var buildings = categoryMenu.buildingAndSubcategoryData;
					var entry = new KeyValuePair<string, string>(buildingID, subcategory);
					var i = buildings.FindIndex(kvp => kvp.Key == addAferID);
					if (i == -1 || i == buildings.Count - 1) buildings.Add(entry);
					else buildings.Insert(i + 1, entry);

					return;
				}
				catch (Exception ex)
				{
					Debug.LogWarning("Failed to insert building at specific index. Adding building to end of list instead.");
					fallback = true;
				}
			}
			else
			{
				fallback = true;
			}

			if (fallback)
			{
				ModUtil.AddBuildingToPlanScreen(category, buildingID);
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

	/// <summary>
	/// A class that provides static utility methods for strings.
	/// </summary>
	public static class StringUtils
	{
		private static Dictionary<string, string> registeredStrings = new Dictionary<string, string>();

		public static void ExportTranslationTemplates()
		{
			WriteTextTemplate(ModCommon.Folder);
			WritePOTemplate(ModCommon.Folder);
		}

		public static void WriteTextTemplate(string modFolder)
		{
			try
			{
				string translationsFolder = Path.Combine(modFolder, "Translations");
				if (!Directory.Exists(translationsFolder))
				{
					Directory.CreateDirectory(translationsFolder);
				}

				string file = Path.Combine(translationsFolder, "en.txt");

				List<string> lines = new List<string>();
				foreach (var kvp in registeredStrings)
				{
					string line = kvp.Key + ": " + kvp.Value;
					lines.Add(line);
				}
				File.WriteAllLines(file, lines, Encoding.UTF8);
			}
			catch (Exception ex)
			{
				Debug.LogError("RomenH.CommonLib: Failed to write txt translations template.");
				Debug.LogException(ex);
			}
		}

		public static void WritePOTemplate(string modFolder)
		{
			try
			{
				string translationsFolder = Path.Combine(modFolder, "Translations");
				if (!Directory.Exists(translationsFolder))
				{
					Directory.CreateDirectory(translationsFolder);
				}

				string file = Path.Combine(translationsFolder, "en.pot");

				using (StreamWriter writer = new StreamWriter(file, false, new UTF8Encoding(false)))
				{
					writer.WriteLine("msgid \"\"");
					writer.WriteLine("msgstr \"\"");
					writer.WriteLine("\"Application: Oxygen Not Included\"");
					writer.WriteLine("\"POT Version: 2.0\"");
					writer.WriteLine("");

					foreach (var kvp in registeredStrings)
					{
						string key = kvp.Key;
						string value = kvp.Value;
						value = value.Replace("\"", "\\\"");
						value = value.Replace("\n", "\\n");
						writer.WriteLine("#. " + key);
						writer.WriteLine("msgctxt \"{0}\"", key);
						writer.WriteLine("msgid \"" + value + "\"");
						writer.WriteLine("msgstr \"\"");
						writer.WriteLine("");
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("RomenH.CommonLib: Failed to write po translations template.");
				Debug.LogException(ex);
			}
		}

		public static void LoadTranslations()
		{
			try
			{
				string translationsFolder = Path.Combine(ModCommon.Folder, "Translations");
				if (!Directory.Exists(translationsFolder))
				{
					Debug.Log("RomenH.CommonLib: Translations folder does not exist. Skipping translations.");
					return;
				}

				string localeCode = Localization.GetLocale()?.Code ?? Localization.DEFAULT_LANGUAGE_CODE;

				if (localeCode == Localization.DEFAULT_LANGUAGE_CODE) return;

				string locPOFile = Path.Combine(translationsFolder, localeCode + ".po");
				string locTxtFile = Path.Combine(translationsFolder, localeCode + ".txt");

				if (File.Exists(locTxtFile))
				{
					LoadTextTranslations(locTxtFile);
				}
				else if (File.Exists(locPOFile))
				{
					Localization.OverloadStrings(Localization.LoadStringsFile(locPOFile, false));
				}
				else
				{
					Debug.LogWarning($"RomenH.CommonLib: No translations file found for current locale: {localeCode}");
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("RomenH.CommonLib: Failed to load translations file.");
				Debug.LogException(ex);
			}
		}

		private static void LoadTextTranslations(string file)
		{
			string[] lines = File.ReadAllLines(file);
			foreach (string line in lines)
			{
				int splitIndex = line.IndexOf(':');
				if (splitIndex < 0)
				{
					Debug.LogWarning($"RomenH.CommonLib: Unexpected line in translations file: \"{line}\"");
					continue;
				}

				string key = line.Substring(0, splitIndex);
				string value = line.Substring(splitIndex).Trim();
				Strings.Add(key, value);
			}
		}

		public static void AddBuildingStrings(string id, string name, string description, string effect)
		{
			AddBuildingString(id, "NAME", UI.FormatAsLink(name, id));
			AddBuildingString(id, "DESC", description);
			AddBuildingString(id, "EFFECT", effect);
		}

		private static void AddBuildingString(string id, string postfix, string value)
		{
			string key = "STRINGS.BUILDINGS.PREFABS." + id.ToUpperInvariant() + "." + postfix.ToUpperInvariant();
			Strings.Add(key, value);
			registeredStrings[key] = value;
		}

		public static void AddStatusItemStrings(string id, string prefix, string name, string tooltip)
		{
			AddStatusItemString(id, prefix, "NAME", name);
			AddStatusItemString(id, prefix, "TOOLTIP", tooltip);
		}

		private static void AddStatusItemString(string id, string prefix, string postfix, string value)
		{
			string key = "STRINGS." + prefix.ToUpperInvariant() + ".STATUSITEMS." + id.ToUpperInvariant() + "." + postfix.ToUpperInvariant();
			Strings.Add(key, value);
			registeredStrings[key] = value;
		}

		public static void AddSideScreenStrings(string id, string title, string tooltip)
		{
			AddSideScreenString(id, "TITLE", title);
			AddSideScreenString(id, "TOOLTIP", tooltip);
		}

		private static void AddSideScreenString(string id, string postfix, string value)
		{
			string key = "STRINGS.UI.UISIDESCREENS." + id.ToUpperInvariant() + "." + postfix.ToUpperInvariant();
			Strings.Add(key, value);
			registeredStrings[key] = value;
		}
	}
}
