using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomenMods.GermicideLampMod.ModStrings
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

    public class UVCSHINEBUG
    {
        public static LocString NAME = "Ultraviolet Bug";

        public static LocString DESC = "Sorry, it's not a thing yet.";
	}
}
