using TUNING;

using UnityEngine;

namespace RomenH.TECBlock
{
	public class TECTileConfig : IBuildingConfig
	{
		public const string ID = "TECTile";

		public const string Name = "TEC Tile";

		public const string Desc = "";

		public const string Effect = "Uses electricity to transfer heat from one side of the tile to the other. As the temperature difference approaches its maximum heat gradient, the TEC transfers less heat.";

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 1,
				anim: "tec_block_kanim",
				hitpoints: 100,
				construction_time: 30f,
				construction_mass: new float[] {
					200f,
					100f
				},
				construction_materials: new string[] {
					MATERIALS.REFINED_METALS[0],
					"Ceramic"
				},
				melting_point: 1600f,
				build_location_rule: BuildLocationRule.Tile,
				decor: TUNING.BUILDINGS.DECOR.PENALTY.TIER2,
				noise: NOISE_POLLUTION.NONE);

			BuildingTemplates.CreateFoundationTileDef(def);

			def.RequiresPowerInput = true;
			def.EnergyConsumptionWhenActive = Mathf.Max(0f, ModSettings.Instance.Wattage);
			if (ModSettings.Instance.GenerateInefficiencyHeat)
			{
				def.SelfHeatKilowattsWhenActive = ModSettings.Instance.Wattage * ModSettings.Instance.KiloDTUPerWatt * (1f - ModSettings.Instance.Efficiency);
			}
			def.Floodable = false;
			def.Entombable = false;
			def.Overheatable = true;
			def.OverheatTemperature = 523.15f; // +250C
			def.ForegroundLayer = Grid.SceneLayer.BuildingBack;
			def.ViewMode = OverlayModes.Temperature.ID;
			def.AudioCategory = "HollowMetal";
			def.AudioSize = "small";
			def.BaseTimeUntilRepair = -1f;
			def.ObjectLayer = ObjectLayer.Building;
			def.SceneLayer = Grid.SceneLayer.TileMain;
			def.PermittedRotations = PermittedRotations.R360;
			def.DragBuild = false;
			def.ThermalConductivity = 0.01f;
			def.UseStructureTemperature = false;
			def.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
			SimCellOccupier simcelloccupier = go.AddOrGet<SimCellOccupier>();
			simcelloccupier.doReplaceElement = true;
			simcelloccupier.movementSpeedMultiplier = ModSettings.Instance.RunSpeedPenalty;
			simcelloccupier.notifyOnMelt = true;

			go.AddOrGet<TileTemperature>();
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			go.AddOrGet<BuildingCellVisualizer>();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
			go.AddOrGet<LogicOperationalController>();
			//go.AddOrGet<Insulator>();

			var tec = go.AddOrGet<TECTile>();
			tec.minColdTemp = Mathf.Max(1f, ModSettings.Instance.MinColdTemperature);
			tec.maxTempDiff = Mathf.Max(1f, ModSettings.Instance.MaxTemperatureDifference);
			tec.kDTUsPerWatt = Mathf.Max(0.01f, ModSettings.Instance.KiloDTUPerWatt);
			tec.efficiency = Mathf.Max(0.1f, ModSettings.Instance.Efficiency);

			go.AddOrGet<BuildingCellVisualizer>();
		}
	}
}
