using RomenH.Common;

using STRINGS;

using UnityEngine;

namespace RomenH.Fans
{
	public class CompressorFanConfig : IBuildingConfig
	{
		public const string ID = "CompressorFanBlock";
		public static readonly LocString Name = StringUtils.BuildingName(ID, "Compressor Fan");
		public static readonly LocString Desc = StringUtils.BuildingDesc(ID, $"Compresses {UI.FormatAsLink("Gases", "ELEMENTS_GAS")} from one side to the other.");
		public static readonly LocString Effect = StringUtils.BuildingEffect(ID, $"Compresses {UI.FormatAsLink("Gases", "ELEMENTS_GAS")}.");

		public override BuildingDef CreateBuildingDef()
		{
			return FanTemplates.CreateAdvancedFan(ID, "compressorgasfan_kanim", ModSettings.Instance.CompressorFanWattage);
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			FanTemplates.ConfigureBuildingTemplate(go, prefab_tag);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			FanTemplates.DoPostConfigureComplete(go, ModSettings.Instance.CompressorFanFlowRate, ConduitType.Gas);
		}
	}
}
