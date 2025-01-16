using RomenH.Common;

using TUNING;

using UnityEngine;

namespace RomenH.TECBlock
{
	public class TECTileConfig : IBuildingConfig
	{
		public const string ID = "TECTile";

		public static readonly LocString Name = StringUtils.BuildingName(ID, "TEC Tile");

		public static readonly LocString Desc = StringUtils.BuildingDesc(ID, "Desc Todo");

		public static readonly LocString Effect = StringUtils.BuildingEffect(ID, "Uses electricity to transfer heat from one side of the tile to the other. As the temperature difference approaches its maximum heat gradient, the TEC transfers less heat.");

		public override BuildingDef CreateBuildingDef()
		{
			string secondIngredient;
			if (ModSettings.Instance.RealisticMaterials)
			{
				secondIngredient = "Lead";
			}
			else
			{
				secondIngredient = TUNING.MATERIALS.REFINED_METAL;
			}

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
					"Ceramic",
					secondIngredient
				},
				melting_point: 1600f,
				build_location_rule: BuildLocationRule.Tile,
				decor: TUNING.BUILDINGS.DECOR.PENALTY.TIER2,
				noise: NOISE_POLLUTION.NONE);

			BuildingTemplates.CreateFoundationTileDef(def);

			def.RequiresPowerInput = true;
			def.EnergyConsumptionWhenActive = Mathf.Max(0f, ModSettings.Instance.CoolerWattage);
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

			go.AddOrGet<Insulator>();

			TileTemperature tt = go.AddOrGet<TileTemperature>();
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			go.AddOrGet<BuildingCellVisualizer>();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
			go.AddOrGet<LogicOperationalController>();

			var tec = go.AddOrGet<TECTile>();
			tec.efficiency = Mathf.Max(0.1f, ModSettings.Instance.CoolerEfficiency);
			tec.minColdTemp = Mathf.Max(1f, ModSettings.Instance.MinColdTemperature);
			tec.kDTUsPerWatt = Mathf.Max(0.01f, ModSettings.Instance.KiloDTUPerWatt);
			tec.maxTempDiff = Mathf.Max(1f, ModSettings.Instance.MaxTemperatureDifference);
			if (ModSettings.Instance.GenerateInefficiencyHeat)
			{
				tec.wasteHeat = ModSettings.Instance.CoolerWattage * ModSettings.Instance.KiloDTUPerWatt * (1f - ModSettings.Instance.CoolerEfficiency);
			}

			var tinkerable = Tinkerable.MakePowerTinkerable(go);
			tinkerable.SetWorkTime(120f);

			go.AddOrGet<BuildingCellVisualizer>();
		}
	}
}
