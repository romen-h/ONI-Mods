using TUNING;
using UnityEngine;

namespace Fans
{
    public static class BaseFanConfig
    {
        private static BuildingDef MakeSimpleDef(string Id, string kanim)
        {
            var buildingDef = BuildingTemplates.CreateBuildingDef(
                id: Id,
                width: 1,
                height: 1,
                anim: kanim,
                hitpoints: BUILDINGS.HITPOINTS.TIER1,
                construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER0,
                construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER0,
                construction_materials: MATERIALS.ALL_METALS,
                melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
                build_location_rule: BuildLocationRule.Tile,
                decor: BUILDINGS.DECOR.PENALTY.TIER1,
                NOISE_POLLUTION.NOISY.TIER2,
                0.2f);
            return buildingDef;
        }

        private static BuildingDef MakeComplexDef(string Id, string kanim)
        {
            string[] construction_materials = new string[2]
            {
                  "RefinedMetal",
                  "Plastic"
            };
            var buildingDef = BuildingTemplates.CreateBuildingDef(
                id: Id,
                width: 1,
                height: 1,
                anim: kanim,
                hitpoints: BUILDINGS.HITPOINTS.TIER1,
                construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER0,
                construction_mass: new float[2] {
                    BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0],
                    BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0]
                },
                construction_materials: construction_materials,
                melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
                build_location_rule: BuildLocationRule.Tile,
                decor: BUILDINGS.DECOR.PENALTY.TIER1,
                NOISE_POLLUTION.NOISY.TIER2,
                0.2f);
            return buildingDef;
        }

        public static BuildingDef CreateBuildingDef(string Id, string kanim, bool makeSimpleDef = true)
        {
            BuildingDef buildingDef;
            if (makeSimpleDef)
            {
                buildingDef = MakeSimpleDef(Id, kanim);
            }
            else
            {
                buildingDef = MakeComplexDef(Id, kanim);
            }
            BuildingTemplates.CreateFoundationTileDef(buildingDef);
            buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = makeSimpleDef ? 60f : 120f;
            buildingDef.ExhaustKilowattsWhenActive = 0f;
            buildingDef.SelfHeatKilowattsWhenActive = 0f;
            buildingDef.Floodable = false;
            buildingDef.Entombable = false;
            buildingDef.Overheatable = true;
            buildingDef.ViewMode = OverlayModes.Power.ID;
            buildingDef.AudioCategory = "Metal";
            buildingDef.PowerInputOffset = new CellOffset(0, 0);
            buildingDef.PermittedRotations = PermittedRotations.R360;
            // tile property
            buildingDef.ThermalConductivity = 1.0f;
            buildingDef.UseStructureTemperature = false;
            buildingDef.BaseTimeUntilRepair = -1.0f;
            buildingDef.ObjectLayer = ObjectLayer.Building;
            buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
            buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
            buildingDef.isSolidTile = true;
            buildingDef.DragBuild = true;
            return buildingDef;
        }

        public static void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag, bool makeSimpleDef = true)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
            SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
            if (!makeSimpleDef)
            {
                simCellOccupier.setLiquidImpermeable = true;
                simCellOccupier.setGasImpermeable = true;
            }
            simCellOccupier.doReplaceElement = makeSimpleDef;
            simCellOccupier.notifyOnMelt = true;
            go.AddOrGet<Insulator>();
            go.AddOrGet<TileTemperature>();
            go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
        }

        public static void DoPostConfigureComplete(GameObject go, float suckRate, ConduitType conduitType, float overPressureThreshold)
        {
            go.AddOrGetDef<OperationalController.Def>();
            go.AddOrGet<EnergyConsumer>();
            FanRotatablePassiveElementConsumer elementConsumer1 = go.AddComponent<FanRotatablePassiveElementConsumer>();
            switch (conduitType)
            {
                case ConduitType.Gas:
                    elementConsumer1.configuration = ElementConsumer.Configuration.AllGas;
                    break;
                case ConduitType.Liquid:
                    elementConsumer1.configuration = ElementConsumer.Configuration.AllLiquid;
                    break;
            }
            elementConsumer1.consumptionRate = suckRate;
            elementConsumer1.storeOnConsume = true;
            elementConsumer1.showInStatusPanel = false;
            elementConsumer1.consumptionRadius = 1;
            elementConsumer1.rotatableCellOffset = new Vector3(0f, -1f);
            elementConsumer1.showDescriptor = false;
            Storage storage = go.AddOrGet<Storage>();
            storage.capacityKg = 2 * suckRate;
            storage.showInUI = true;
            Fan fan = go.AddOrGet<Fan>();
            fan.conduitType = conduitType;
            fan.overpressureMass = overPressureThreshold;

            GeneratedBuildings.RemoveLoopingSounds(go);
            go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
        }

    }
}
