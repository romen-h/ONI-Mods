using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RomenH.Common;

using TUNING;

using UnityEngine;

namespace RomenH.TECBlock
{
	public class TEGTileConfig : IBuildingConfig
	{
		public const string ID = "RomenH_TEGTile";

		public static readonly LocString Name = StringUtils.BuildingName(ID, "TEG Tile");

		public static readonly LocString Desc = StringUtils.BuildingDesc(ID, "Desc Todo");

		public static readonly LocString Effect = StringUtils.BuildingEffect(ID, "Generates electricity from the heat difference on each side of the building. As the temperature difference approaches its maximum heat gradient, the TEG generates more power.");

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
				anim: "teg_block_kanim",
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

			def.RequiresPowerOutput = true;
			def.PowerOutputOffset = new CellOffset(0, 0);
			def.GeneratorWattageRating = ModSettings.Instance.GeneratorWattage;
			def.GeneratorBaseCapacity = def.GeneratorWattageRating;

			def.Floodable = false;
			def.Entombable = false;
			def.Overheatable = true;
			def.OverheatTemperature = 398.15f; // +125C
			def.ForegroundLayer = Grid.SceneLayer.BuildingBack;
			def.ViewMode = OverlayModes.Power.ID;
			def.AudioCategory = "HollowMetal";
			def.AudioSize = "small";
			def.BaseTimeUntilRepair = -1f;
			def.ObjectLayer = ObjectLayer.Building;
			def.SceneLayer = Grid.SceneLayer.TileMain;
			def.PermittedRotations = PermittedRotations.R360;
			def.DragBuild = false;
			def.ThermalConductivity = 0.1f;
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

			var teg = go.AddComponent<TEGTile>();
			teg.efficiency = Mathf.Max(0.1f, ModSettings.Instance.GeneratorEfficiency);
			teg.kDTUsPerWatt = Mathf.Max(0.01f, ModSettings.Instance.KiloDTUPerWatt);
			teg.maxTempDiff = Mathf.Max(1f, ModSettings.Instance.MaxTemperatureDifference);

			go.AddOrGet<BuildingCellVisualizer>();
		}
	}
}
