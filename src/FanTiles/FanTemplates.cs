using TUNING;

using UnityEngine;

namespace Fans
{
	public static class FanTemplates
	{
		public static BuildingDef CreateBasicFan(string id, string kanim, float energyConsumption)
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(
				id: id,
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
				noise: NOISE_POLLUTION.NOISY.TIER2
			);

			ApplyCommonFanSettings(buildingDef);
			buildingDef.EnergyConsumptionWhenActive = energyConsumption;

			return buildingDef;
		}

		public static BuildingDef CreateAdvancedFan(string id, string kanim, float energyConsumption)
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(
				id: id,
				width: 1,
				height: 1,
				anim: kanim,
				hitpoints: BUILDINGS.HITPOINTS.TIER1,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER0,
				construction_mass: new float[2] {
					BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0],
					BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0]
				},
				construction_materials: new string[2]
				{
					  "RefinedMetal",
					  "Plastic"
				},
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				build_location_rule: BuildLocationRule.Tile,
				decor: BUILDINGS.DECOR.PENALTY.TIER1,
				noise: NOISE_POLLUTION.NOISY.TIER2
			);

			ApplyCommonFanSettings(buildingDef);
			buildingDef.EnergyConsumptionWhenActive = energyConsumption;

			return buildingDef;
		}

		private static void ApplyCommonFanSettings(BuildingDef buildingDef)
		{
			BuildingTemplates.CreateFoundationTileDef(buildingDef);

			buildingDef.RequiresPowerInput = true;
			buildingDef.ExhaustKilowattsWhenActive = 0f;
			buildingDef.SelfHeatKilowattsWhenActive = 0f;
			buildingDef.Floodable = false;
			buildingDef.Entombable = false;
			buildingDef.Overheatable = true;
			buildingDef.ViewMode = OverlayModes.Power.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.PowerInputOffset = new CellOffset(0, 0);
			buildingDef.PermittedRotations = PermittedRotations.R360;
			buildingDef.ThermalConductivity = 1.0f;
			buildingDef.UseStructureTemperature = false;
			buildingDef.BaseTimeUntilRepair = -1.0f;
			buildingDef.ObjectLayer = ObjectLayer.Building;
			buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
			buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
			buildingDef.DragBuild = true;
		}

		public static void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);

			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

			SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
			simCellOccupier.doReplaceElement = true;
			simCellOccupier.setLiquidImpermeable = true;
			simCellOccupier.setGasImpermeable = true;
			simCellOccupier.notifyOnMelt = true;

			go.AddOrGet<Insulator>();

			go.AddOrGet<TileTemperature>();

			go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		}

		public static void DoPostConfigureComplete(GameObject go, float flowRate, ConduitType conduitType, float overPressureThreshold = -1f)
		{
			go.AddOrGetDef<OperationalController.Def>();

			go.AddOrGet<EnergyConsumer>();

			FanRotatablePassiveElementConsumer elementConsumer = go.AddComponent<FanRotatablePassiveElementConsumer>();
			switch (conduitType)
			{
				case ConduitType.Gas:
					elementConsumer.configuration = ElementConsumer.Configuration.AllGas;
					break;
				case ConduitType.Liquid:
					elementConsumer.configuration = ElementConsumer.Configuration.AllLiquid;
					break;
			}
			elementConsumer.consumptionRate = flowRate;
			elementConsumer.storeOnConsume = true;
			elementConsumer.showInStatusPanel = false;
			elementConsumer.consumptionRadius = 1;
			elementConsumer.rotatableCellOffset = new Vector3(0f, -1f);
			elementConsumer.showDescriptor = false;

			Storage storage = go.AddOrGet<Storage>();
			storage.capacityKg = 2 * flowRate;
			storage.showInUI = true;

			Fan fan = go.AddOrGet<Fan>();
			fan.conduitType = conduitType;
			fan.overpressureMass = overPressureThreshold;

			GeneratedBuildings.RemoveLoopingSounds(go);

			go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
		}

	}
}
