using RomenH.Common;

using STRINGS;

using UnityEngine;

namespace RomenH.Fans
{
	public class LiquidTurbineConfig : IBuildingConfig
	{
		public const string ID = "TurbineBlock";
		public static readonly LocString Name = StringUtils.BuildingName(ID, "Turbine");
		public static readonly LocString Desc = StringUtils.BuildingDesc(ID, $"Moves {UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID")} from one side to the other.");
		public static readonly LocString Effect = StringUtils.BuildingEffect(ID, $"Pumps around {UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID")}.");

		public override BuildingDef CreateBuildingDef()
		{
			return FanTemplates.CreateBasicFan(ID, "liquidfan_kanim", ModSettings.Instance.TurbineWattage);
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			FanTemplates.ConfigureBuildingTemplate(go, prefab_tag);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			FanTemplates.DoPostConfigureComplete(go, ModSettings.Instance.TurbineFlowRate, ConduitType.Liquid, ModSettings.Instance.TurbinePressureLimit);
		}
	}

}
