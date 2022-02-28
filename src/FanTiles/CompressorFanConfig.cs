using STRINGS;

using UnityEngine;

namespace Fans
{
	public class CompressorFanConfig : IBuildingConfig
	{
		public const string ID = "CompressorFanBlock";
		public static string Name = UI.FormatAsLink("Compressor Fan", ID.ToUpper());
		public static string Desc = $"Compresses {UI.FormatAsLink("Gasses", "ELEMENTS_GAS")} from one side to the other.";
		public static string Effect = $"Compresses {UI.FormatAsLink("Gasses", "ELEMENTS_GAS")}.";

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
