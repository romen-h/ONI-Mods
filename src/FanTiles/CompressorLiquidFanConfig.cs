using STRINGS;
using UnityEngine;

namespace Fans
{
    public class CompressorLiquidFanConfig : IBuildingConfig
    {
        public const string Id = "CompressorTurbineBlock";
        public static string DisplayName = UI.FormatAsLink("Compressor Turbine Block", Id.ToUpper());
        public static string Description = $"Compresses {UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID")} from one side to the other.";
        public static string Effect = $"Compresses {UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID")}.";

        private const float SuckRate = 5.0f;
        private const ConduitType Type = ConduitType.Liquid;
        private const float OverPressureThreshold = -1.0f;

        public override BuildingDef CreateBuildingDef()
        {
            return BaseFanConfig.CreateBuildingDef(Id, "compressorliquidfan_kanim", false);
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            BaseFanConfig.ConfigureBuildingTemplate(go, prefab_tag, false);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            BaseFanConfig.DoPostConfigureComplete(go, SuckRate, Type, OverPressureThreshold);
        }
    }
}
