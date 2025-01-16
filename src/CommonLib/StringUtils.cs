using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using KMod;
using STRINGS;

using Directory = System.IO.Directory;

namespace RomenH.Common
{
	/// <summary>
	/// A class that provides static utility methods for strings.
	/// </summary>
	public static class StringUtils
	{
		private static Dictionary<string, LocString> registeredStrings = new Dictionary<string, LocString>();

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

		public static LocString LogicPortDesc(string ID, string value)
		{
			return new LocString(value, "STRINGS.BUILDINGS.PREFABS." + ID + ".LOGIC_PORT");
		}

		public static LocString LogicPortInputActive(string ID, string value)
		{
			return new LocString(value, "STRINGS.BUILDINGS.PREFABS." + ID + ".INPUT_PORT_ACTIVE");
		}

		public static LocString LogicPortOutputActive(string ID, string value)
		{
			return new LocString(value, "STRINGS.BUILDINGS.PREFABS." + ID + ".INPUT_PORT_INACTIVE");
		}

		public static LocString TechName(string ID, string value)
		{
			return new LocString(value, "STRINGS.RESEARCH.TECHS." + ID.ToUpperInvariant() + ".NAME");
		}

		public static LocString TechDesc(string ID, string value)
		{
			return new LocString(value, "STRINGS.RESEARCH.TECHS." + ID.ToUpperInvariant() + ".DESC");
		}

		public static LocString SideScreenName(string ID, string value)
		{
			return new LocString(value, "STRINGS.UI." + ID.ToUpperInvariant() + ".NAME");
		}

		internal static void RegisterAllLocStrings()
		{
			try
			{
				ModCommon.Log.Debug("Searching for translatable strings...");
				foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
				{
					if (type?.FullName?.StartsWith("RomenH.") ?? false)
					{
						foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
						{
							if (field.FieldType == typeof(LocString))
							{
								LocString ls = (LocString)field.GetValue(null);
								if (ls.key.IsValid())
								{
									ModCommon.Log.Debug($"Found string: {ls.key.String}");
									Strings.Add(ls.key.String, ls.text);
									registeredStrings.Add(ls.key.String, ls);
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				ModCommon.Log.Error("Failed to register strings.", ex);
			}

			try
			{
				ModCommon.Log.Debug("Exporting translation template...");
				WritePOTemplate(ModCommon.Folder);
			}
			catch (Exception ex)
			{
				ModCommon.Log.Error("Failed to export translation template.", ex);
			}

			ModCommon.Log.Debug("RegisterAllLocStrings End");
		}

		public static void WritePOTemplate(string modFolder)
		{
			try
			{
				ModCommon.Log.Debug("Writing pot file...");
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
				ModCommon.Log.Error("RomenH.CommonLib: Failed to write po translations template.", ex);
			}

			ModCommon.Log.Debug("WritePOTemplate End");
		}

		public static void ApplyTranslationsPatch(Harmony harmony)
		{
			try
			{
				ModCommon.Log.Debug("Applying translation loader patch...");
				var targetMethod = typeof(Localization).GetMethod(nameof(Localization.Initialize));
				HarmonyMethod postfixPatch = new HarmonyMethod(typeof(StringUtils), nameof(LoadTranslations));
				harmony.Patch(targetMethod, postfix: postfixPatch);
			}
			catch (Exception ex)
			{
				ModCommon.Log.Error("Failed to apply translation patch.", ex);
			}

			ModCommon.Log.Debug("ApplyTranslationsPatch End");
		}

		public static void LoadTranslations()
		{
			try
			{
				ModCommon.Log.Debug("Loading translations...");
				

				string localeCode = Localization.GetLocale()?.Code ?? Localization.DEFAULT_LANGUAGE_CODE;
				ModCommon.Log.Debug($"Language code is {localeCode}.");
				if (localeCode == Localization.DEFAULT_LANGUAGE_CODE) return;

				// Try to find the .po file...
				string translationsFolder = Path.Combine(ModCommon.Folder, "Translations");
				if (!Directory.Exists(translationsFolder))
				{
					ModCommon.Log.Warn("Translations folder not found. This mod will not be translated.");
					return;
				}
				else
				{
					ModCommon.Log.Debug("Translations folder found.");
				}

				string locPOFile = Path.Combine(translationsFolder, $"{localeCode}.po");
				if (File.Exists(locPOFile))
				{
					ModCommon.Log.Debug($"Translation file is: {locPOFile}");
					// Load the .po file and overwrite strings
					var strings = Localization.LoadStringsFile(locPOFile, false);
					if (strings.Count == 0)
					{
						ModCommon.Log.Warn("Translation file has no strings. This mod will not be translated.");
						return;
					}

					FieldInfo locStringTextField = typeof(LocString).GetField("_text", BindingFlags.Instance | BindingFlags.NonPublic);

					foreach (var kvp in strings)
					{
						if (registeredStrings.TryGetValue(kvp.Key, out LocString locStr))
						{
							ModCommon.Log.Debug($"Applying translation for : {kvp.Key}");
							locStringTextField.SetValue(locStr, kvp.Value);
							Strings.Add(kvp.Key, kvp.Value);
						}
						else
						{
							ModCommon.Log.Debug($"LocString not found for: {kvp.Key}");
						}
					}

					ModCommon.Log.Info($"Loaded translations file: {localeCode}.po");
				}
				else
				{
					ModCommon.Log.Info($"No translations file found for current locale. ({localeCode})");
				}
			}
			catch (Exception ex)
			{
				ModCommon.Log.Error("Failed to load translations file.", ex);
			}
		}
	}
}
