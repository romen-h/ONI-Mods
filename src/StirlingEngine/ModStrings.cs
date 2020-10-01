using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomenMods.StirlingEngineMod.ModStrings
{
	public class STIRLINGENGINE
	{
		public static LocString NAME = UI.FormatAsLink("Stirling Engine", "STIRLINGENGINE");

		public static LocString DESC = $"Draws up to {StirlingEngine.WattsToHeat(Mod.Config.MaxWattOutput):F0} DTU/s of heat from the cell below the floor and converts it to power. The amount of heat drawn is based on the ratio of building temperature vs temperature below the floor tile.";

		public static LocString EFFECT = "Stirling Engines draw " + UI.FormatAsLink("Heat", "HEAT") + " from the room below and harness that heat to generate " + UI.FormatAsLink("Power", "POWER") + ".";
	}

	public class STIRLINGENGINE_ACTIVE
	{
		public static string ID = "STIRLING_ACTIVE";

		public static LocString NAME = "Active";

		public static LocString TOOLTIP = "This engine is running at <b>{Efficiency}</b> efficiency.";
	}

	public class STIRLINGENGINE_NO_HEAT_GRADIENT
	{
		public static string ID = "STIRLING_NO_HEAT_GRADIENT";

		public static LocString NAME = "Temperature gradient is less than <b>{Min_Temperature_Gradient}</b>.";

		public static LocString TOOLTIP = "This engine requires a " + UI.PRE_KEYWORD + "Temperature" + UI.PST_KEYWORD + " difference of at least <b>{Min_Temperature_Gradient}</b> to generate power.";
	}

	public class STIRLINGENGINE_TOO_HOT
	{
		public static string ID = "STIRLING_TOO_HOT";

		public static LocString NAME = "Engine Too Hot";

		public static LocString TOOLTIP = "This engine must be below <b>{Overheat_Temperature}</b> to properly function.";
	}

	public class STIRLINGENGINE_ACTIVE_WATTAGE
	{
		public static string ID = "STIRLING_ACTIVE_WATTAGE";

		public static LocString NAME = "Current Wattage: {Wattage}";

		public static LocString TOOLTIP = "This stirling engine is generating " + UI.FormatAsPositiveRate("{Wattage}") + "\n\nIt is running at <b>{Efficiency}</b> of full capacity. Increase the " + UI.PRE_KEYWORD + "Temperature" + UI.PST_KEYWORD + " gradient to improve output.";
	}

	public class STIRLINGENGINE_HEAT_PUMPED
	{
		public static string ID = "STIRLING_HEAT_PUMPED";

		public static LocString NAME = "Heat Input: {HeatPumped}";

		public static LocString TOOLTIP = "TODO";
	}
}
