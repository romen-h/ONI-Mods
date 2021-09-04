using STRINGS;
using UnityEngine;

namespace Fans
{
    public class HighPressureGasFan : IBuildingConfig
    {
        public const string Id = "HighPressureFanBlock";
        public static string DisplayName = UI.FormatAsLink("High Pressure Fan Block", Id.ToUpper());
        public static string Description = $"Moves {UI.FormatAsLink("Gasses", "ELEMENTS_GAS")} from one side to the other.";
        public static string Effect = $"Blows around {UI.FormatAsLink("Gasses", "ELEMENTS_GAS")} in high pressure areas.";

        private const float SuckRate = 0.5f;
        private const ConduitType Type = ConduitType.Gas;
        private const float OverPressureThreshold = 20.0f;

        public override BuildingDef CreateBuildingDef()
        {
            return BaseFanConfig.CreateBuildingDef(Id, "highgasfan_kanim");
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
