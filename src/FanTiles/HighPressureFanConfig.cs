using RomenH.Common;

using STRINGS;

using UnityEngine;

namespace RomenH.Fans
{
	public class HighPressureFanConfig : IBuildingConfig
	{
		public const string ID = "HighPressureFanBlock";
		public static readonly LocString Name = StringUtils.BuildingName(ID, "High Pressure Fan");
		public static readonly LocString Desc = StringUtils.BuildingDesc(ID, $"Moves {UI.FormatAsLink("Gases", "ELEMENTS_GAS")} from one side to the other.");
		public static readonly LocString Effect = StringUtils.BuildingEffect(ID, $"Blows around {UI.FormatAsLink("Gases", "ELEMENTS_GAS")} in high pressure areas.");

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
