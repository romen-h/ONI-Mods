using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RomenH.Common;

using STRINGS;

using UnityEngine;

namespace RomenH.PlasticTiles
{
	public class PlasticMeshTileConfig : IBuildingConfig
	{
		public const string ID = "RomenH_PlasticMeshTile";
		public static readonly LocString Name = StringUtils.BuildingName(ID, "Plastic Mesh Tile");
		public static readonly LocString Desc = StringUtils.BuildingDesc(ID, "Mesh tile can be used to make Duplicant pathways in areas where liquid flows.");
		public static readonly LocString Effect = StringUtils.BuildingEffect(ID, string.Concat(new string[]
		{
			"Used to build the walls and floors of rooms.\n\nDoes not obstruct ",
			UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
			" or ",
			UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
			" flow."
		}));

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 1,
				anim: "tiles_plastic_mesh_kanim",
				hitpoints: 100,
				construction_time: 30f,
				construction_mass: TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				construction_materials: TUNING.MATERIALS.PLASTICS,
				melting_point: 800f,
				build_location_rule: BuildLocationRule.Tile,
				TUNING.BUILDINGS.DECOR.BONUS.TIER0,
				TUNING.NOISE_POLLUTION.NONE
			);

			BuildingTemplates.CreateFoundationTileDef(def);
			def.Floodable = false;
			def.Entombable = false;
			def.Overheatable = false;
			def.UseStructureTemperature = false;
			def.AudioCategory = "Metal";
			def.AudioSize = "small";
			def.BaseTimeUntilRepair = -1f;
			def.SceneLayer = Grid.SceneLayer.TileMain;
			def.isKAnimTile = true;
			def.BlockTileAtlas = AssetLoader.GetCustomTileAtlas("tiles_plastic_mesh.png");
			def.BlockTilePlaceAtlas = AssetLoader.GetCustomTileAtlas("tiles_plastic_mesh_place.png");
			def.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
			def.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_plastic_tops_decor_info");
			def.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_plastic_tops_place_decor_info");
			def.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
			SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
			simCellOccupier.doReplaceElement = false;
			simCellOccupier.movementSpeedMultiplier = 1.5f; // TUNING.DUPLICANTSTATS.MOVEMENT_MODIFIERS.BONUS_3;
			go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = PlasticTileConfig.BlockTileConnectorID;
			go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RemoveLoopingSounds(go);
			go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
			go.AddComponent<SimTemperatureTransfer>();
			go.AddComponent<ZoneTile>();
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			base.DoPostConfigureUnderConstruction(go);
			go.AddOrGet<KAnimGridTileVisualizer>();
		}
	}
}
