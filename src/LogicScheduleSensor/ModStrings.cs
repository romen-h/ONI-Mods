using STRINGS;

namespace RomenH.LogicScheduleSensor.ModStrings
{
	public class STRINGS
	{
		public class BUILDINGS
		{
			public class LOGICSCHEDULESENSOR
			{
				public static LocString NAME = UI.FormatAsLink("Schedule Sensor", "LOGICSCHEDULESENSOR");

				public static LocString DESC = "Schedule sensors allow systems to be synchronized to a specific schedule and shift.";

				public static LocString EFFECT = string.Concat("Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when the selected ",
					UI.FormatAsLink("Schedule", "SCHEDULE"),
					" enters the selected shift.");

				public static LocString LOGIC_PORT = UI.FormatAsLink("Schedule", "SCHEDULE") + " Shift";

				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when the selected " + UI.FormatAsLink("Schedule", "SCHEDULE") + " enters the selected shift";

				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}
		}
	}
}
