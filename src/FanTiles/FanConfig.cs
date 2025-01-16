using RomenH.Common;

using STRINGS;

using UnityEngine;

namespace RomenH.Fans
{
	public class FanConfig : IBuildingConfig
	{
		public const string ID = "FanBlock";
		public static readonly LocString Name = StringUtils.BuildingName(ID, "Fan");
		public static readonly LocString Desc = StringUtils.BuildingDesc(ID, $"Moves {UI.FormatAsLink("Gases", "ELEMENTS_GAS")} from one side to the other.");
		public static readonly LocString Effect = StringUtils.BuildingEffect(ID, $"Blows around {UI.FormatAsLink("Gases", "ELEMENTS_GAS")}.");

		public override BuildingDef CreateBuildingDef()
		{
			return FanTemplates.CreateBasicFan(ID, "gasfan_kanim", ModSettings.Instance.FanWattage);
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			FanTemplates.ConfigureBuildingTemplate(go, prefab_tag);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			FanTemplates.DoPostConfigureComplete(go, ModSettings.Instance.FanFlowRate, ConduitType.Gas, ModSettings.Instance.FanPressureLimit);
		}
	}

}
