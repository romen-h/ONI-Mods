using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using HarmonyLib;
using RomenH.Common;

namespace Fans
{
    public class CompressorGasFanConfig : IBuildingConfig
    {
        public const string Id = "CompressorFanBlock";
        public static string DisplayName = UI.FormatAsLink("Compressor Fan Block", Id.ToUpper());
        public static string Description = $"Compresses {UI.FormatAsLink("Gasses", "ELEMENTS_GAS")} from one side to the other.";
        public static string Effect = $"Compresses {UI.FormatAsLink("Gasses", "ELEMENTS_GAS")}.";

        private const float SuckRate = 0.5f;
        private const ConduitType Type = ConduitType.Gas;
        private const float OverPressureThreshold = -1.0f;

        public override BuildingDef CreateBuildingDef()
        {
            return BaseFanConfig.CreateBuildingDef(Id, "compressorgasfan_kanim", false);
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
