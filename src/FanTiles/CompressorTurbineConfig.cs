using STRINGS;

using UnityEngine;

namespace Fans
{
	public class CompressorTurbineConfig : IBuildingConfig
	{
		public const string ID = "CompressorTurbineBlock";
		public static string Name = UI.FormatAsLink("Compressor Turbine", ID.ToUpper());
		public static string Desc = $"Compresses {UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID")} from one side to the other.";
		public static string Effect = $"Compresses {UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID")}.";

		public override BuildingDef CreateBuildingDef()
		{
			return FanTemplates.CreateAdvancedFan(ID, "compressorliquidfan_kanim", ModSettings.Instance.CompressorTurbineWattage);
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			FanTemplates.ConfigureBuildingTemplate(go, prefab_tag);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			FanTemplates.DoPostConfigureComplete(go, ModSettings.Instance.CompressorTurbineFlowRate, ConduitType.Liquid);
		}
	}
}
