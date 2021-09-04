using STRINGS;
using UnityEngine;

namespace Fans
{
    public class LiquidFanConfig : IBuildingConfig
    {
        public const string Id = "TurbineBlock";
        public static string DisplayName = UI.FormatAsLink("Turbine Block", Id.ToUpper());
        public static string Description = $"Moves {UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID")} from one side to the other.";
        public static string Effect = $"Pumps around {UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID")}.";

        private const float SuckRate = 5.0f;
        private const ConduitType Type = ConduitType.Liquid;
        private const float OverPressureThreshold = 1000.0f;

        public override BuildingDef CreateBuildingDef()
        {
            return BaseFanConfig.CreateBuildingDef(Id, "liquidfan_kanim");
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            BaseFanConfig.ConfigureBuildingTemplate(go, prefab_tag);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            BaseFanConfig.DoPostConfigureComplete(go, SuckRate, Type, OverPressureThreshold);
        }
    }

}
