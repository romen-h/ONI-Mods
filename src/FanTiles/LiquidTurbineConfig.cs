using STRINGS;

using UnityEngine;

namespace Fans
{
	public class LiquidTurbineConfig : IBuildingConfig
	{
		public const string ID = "TurbineBlock";
		public static string Name = UI.FormatAsLink("Turbine", ID.ToUpper());
		public static string Desc = $"Moves {UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID")} from one side to the other.";
		public static string Effect = $"Pumps around {UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID")}.";

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
