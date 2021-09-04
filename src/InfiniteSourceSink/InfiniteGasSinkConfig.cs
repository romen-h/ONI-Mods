using RomenH.Common;
using TUNING;
using UnityEngine;

namespace InfiniteSourceSink
{
    public class InfiniteGasSinkConfig : IBuildingConfig
    {
        public const string Id = "GasSink";
        public const string DisplayName = "Infinite Gas Sink";
        public const string Description = "Voids all gas sent into it.";
        public const string Effect = "Where does all the gas go?";

        public override BuildingDef CreateBuildingDef()
        {
            var buildingDef = BuildingTemplates.CreateBuildingDef(
                id: Id,
                width: 1,
                height: 1,
                anim: "sink_gas_kanim",
                hitpoints: BUILDINGS.HITPOINTS.TIER2,
                construction_time: ModSettings.Instance.BuildTimeSeconds,
                construction_mass: ModSettings.Instance.BuildMassKg,
                construction_materials: ModSettings.Instance.SandboxOnly ? GameStrings.MaterialLists.Neutronium : MATERIALS.ALLOYS,
                melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER4,
                build_location_rule: BuildLocationRule.Anywhere,
                decor: BUILDINGS.DECOR.PENALTY.TIER1,
                noise: NOISE_POLLUTION.NOISY.TIER4
            );
			buildingDef.Overheatable = false;
            buildingDef.InputConduitType = ConduitType.Gas;
            buildingDef.Floodable = false;
            buildingDef.ViewMode = OverlayModes.GasConduits.ID;
            buildingDef.AudioCategory = "Metal";
            buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
            buildingDef.UtilityInputOffset = new CellOffset(0, 0);
            buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
            go.AddOrGet<InfiniteSink>().Type = ConduitType.Gas;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<LogicOperationalController>();
            go.AddOrGet<Operational>();

            Object.DestroyImmediate(go.GetComponent<RequireInputs>());
            Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
            Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());

            go.AddOrGetDef<OperationalController.Def>();
            BuildingTemplates.DoPostConfigure(go);
        }
    }
}
