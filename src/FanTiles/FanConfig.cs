using STRINGS;

using UnityEngine;

namespace Fans
{
	public class FanConfig : IBuildingConfig
	{
		public const string ID = "FanBlock";
		public static string Name = UI.FormatAsLink("Fan", ID.ToUpper());
		public static string Desc = $"Moves {UI.FormatAsLink("Gasses", "ELEMENTS_GAS")} from one side to the other.";
		public static string Effect = $"Blows around {UI.FormatAsLink("Gasses", "ELEMENTS_GAS")}.";

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
