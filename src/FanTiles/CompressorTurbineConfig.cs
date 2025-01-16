using RomenH.Common;

using STRINGS;

using UnityEngine;

namespace RomenH.Fans
{
	public class CompressorTurbineConfig : IBuildingConfig
	{
		public const string ID = "CompressorTurbineBlock";
		public static readonly LocString Name = StringUtils.BuildingName(ID, "Compressor Turbine");
		public static readonly LocString Desc = StringUtils.BuildingDesc(ID, $"Compresses {UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID")} from one side to the other.");
		public static readonly LocString Effect = StringUtils.BuildingEffect(ID, $"Compresses {UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID")}.");

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
