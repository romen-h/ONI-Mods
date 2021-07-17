using STRINGS;

namespace RomenH.GermicideLamp.ModStrings
{
	public class STRINGS
	{
		public class BUILDINGS
		{
			public class GERMICIDELAMP
			{
				public static LocString NAME = UI.FormatAsLink("Germicidal UV Lamp", "GERMICIDELAMP");

				public static LocString DESC = "";

				public static LocString EFFECT = "Germicidal UV Lamps give off UVC radiation that quickly kills germs in a large area.";
			}

			public class CEILINGGERMICIDELAMP
			{
				public static LocString NAME = UI.FormatAsLink("Germicidal Ceiling Light", "CEILINGGERMICIDELAMP");

				public static LocString DESC = "";

				public static LocString EFFECT = "Provides a small amount of light while killing germs beneath them with UVC radiation.";
			}
		}
	}
}
