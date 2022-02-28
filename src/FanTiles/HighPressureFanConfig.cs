using STRINGS;

using UnityEngine;

namespace Fans
{
	public class HighPressureFanConfig : IBuildingConfig
	{
		public const string ID = "HighPressureFanBlock";
		public static string Name = UI.FormatAsLink("High Pressure Fan", ID.ToUpper());
		public static string Desc = $"Moves {UI.FormatAsLink("Gasses", "ELEMENTS_GAS")} from one side to the other.";
		public static string Effect = $"Blows around {UI.FormatAsLink("Gasses", "ELEMENTS_GAS")} in high pressure areas.";

		public override BuildingDef CreateBuildingDef()
		{
			return FanTemplates.CreateBasicFan(ID, "highgasfan_kanim", ModSettings.Instance.HighPressureFanWattage);
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			FanTemplates.ConfigureBuildingTemplate(go, prefab_tag);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			FanTemplates.DoPostConfigureComplete(go, ModSettings.Instance.HighPressureFanFlowRate, ConduitType.Gas, ModSettings.Instance.HighPressureFanPressureLimit);
		}
	}
}
