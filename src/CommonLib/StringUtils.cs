using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using STRINGS;

namespace RomenH.Common
{
	/// <summary>
	/// A class that provides static utility methods for strings.
	/// </summary>
	public static class StringUtils
	{
		private static Dictionary<string, string> registeredStrings = new Dictionary<string, string>();

		public static LocString BuildingName(string ID, string value)
		{
			return new LocString(UI.FormatAsLink(value, ID.ToUpperInvariant()), "STRINGS.BUILDINGS.PREFABS." + ID.ToUpperInvariant() + ".NAME");
		}

		public static LocString BuildingDesc(string ID, string value)
		{
			return new LocString(value, "STRINGS.BUILDINGS.PREFABS." + ID.ToUpperInvariant() + ".DESC");
		}

		public static LocString BuildingEffect(string ID, string value)
		{
			return new LocString(value, "STRINGS.BUILDINGS.PREFABS." + ID.ToUpperInvariant() + ".EFFECT");
		}

		public static LocString BuildingLogicPortName(string ID, string value)
		{
			return new LocString(value, "STRINGS.BUILDINGS.PREFABS." + ID.ToUpperInvariant() + ".LOGIC_PORT");
		}

		public static LocString BuildingLogicPortActive(string ID, string value)
		{
			return new LocString(value, "STRINGS.BUILDINGS.PREFABS." + ID.ToUpperInvariant() + ".LOGIC_PORT_ACTIVE");
		}

		public static LocString BuildingLogicPortInactive(string ID, string value)
		{
			return new LocString(value, "STRINGS.BUILDINGS.PREFABS." + ID.ToUpperInvariant() + ".LOGIC_PORT_INACTIVE");
		}

		public static LocString CreatureName(string ID, string value)
		{
			return new LocString(value, "STRINGS.CREATURES.SPECIES." + ID.ToUpperInvariant() + ".NAME");
		}

		public static LocString CreatureDesc(string ID, string value)
		{
			return new LocString(value, "STRINGS.CREATURES.SPECIES." + ID.ToUpperInvariant() + ".DESC");
		}

		public static LocString CreatureVariantName(string ID, string variant, string value)
		{
			return new LocString(value, "STRINGS.CREATURES.SPECIES." + ID.ToUpperInvariant() + "." + variant.ToUpperInvariant() + ".NAME");
		}

		public static LocString CreatureVariantDesc(string ID, string variant, string value)
		{
			return new LocString(value, "STRINGS.CREATURES.SPECIES." + ID.ToUpperInvariant() + "." + variant.ToUpperInvariant() + ".DESC");
		}

		public static LocString StatusItemName(string ID, string prefix, string value)
		{
			return new LocString(value, "STRINGS." + prefix.ToUpperInvariant() + ".STATUSITEMS." + ID.ToUpperInvariant() + ".NAME");
		}

		public static LocString StatusItemTooltip(string ID, string prefix, string value)
		{
			return new LocString(value, "STRINGS." + prefix.ToUpperInvariant() + ".STATUSITEMS." + ID.ToUpperInvariant() + ".TOOLTIP");
		}

		public static LocString TechName(string ID, string value)
		{
			return new LocString(value, "STRINGS.RESEARCH.TECHS." + ID.ToUpperInvariant() + ".NAME");
		}

		public static LocString TechDesc(string ID, string value)
		{
			return new LocString(value, "STRINGS.RESEARCH.TECHS." + ID.ToUpperInvariant() + ".DESC");
		}

		internal static void RegisterAllLocStrings()
		{
			try
			{
				foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
				{
					if (type.FullName.StartsWith("RomenH."))
					{
						foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
						{
							if (field.FieldType == typeof(LocString))
							{
								LocString ls = (LocString)field.GetValue(null);
								if (ls.key.IsValid())
								{
									Strings.Add(ls.key.String, ls.text);
									registeredStrings.Add(ls.key.String, ls.text);
								}
							}
						}
					}
				}
				Debug.Log($"{ModCommon.Name}: String registration complete.");
			}
			catch (Exception ex)
			{
				Debug.LogWarning($"{ModCommon.Name}: Failed to register strings.\n{ex}");
			}

			try
			{
				ExportTranslationTemplates();
				Debug.Log($"{ModCommon.Name}: Exported translation templates.");
			}
			catch (Exception ex)
			{
				Debug.LogWarning($"{ModCommon.Name}: Failed to export translation templates.\n{ex}");
			}
		}

		internal static void ExportTranslationTemplates()
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
					string line = kvp.Key + ": " + kvp.Value.Replace(Environment.NewLine, "\\n");
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

#if false
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

		public static void AddEquipmentStrings(string id, string name, string description, string effect, string recipeDescription)
		{
			AddEquipmentString(id, "NAME", UI.FormatAsLink(name, id));
			AddEquipmentString(id, "DESC", description);
			AddEquipmentString(id, "EFFECT", effect);
			AddEquipmentString(id, "RECIPE_DESC", recipeDescription);
		}

		private static void AddEquipmentString(string id, string postfix, string value)
		{
			string key = "STRINGS.EQUIPMENT.PREFABS." + id.ToUpperInvariant() + "." + postfix.ToUpperInvariant();
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
#endif
	}
}
