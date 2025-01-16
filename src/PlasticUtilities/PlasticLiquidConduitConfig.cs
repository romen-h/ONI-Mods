using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RomenH.Common;

using STRINGS;

using UnityEngine;

namespace RomenH.PlasticUtilities
{
	public class PlasticLiquidConduitConfig : IBuildingConfig
	{
		public const string ID = "RomenH_PlasticLiquidConduit";
		public static readonly LocString Name = StringUtils.BuildingName(ID, "Plastic Liquid Pipe");
		public static readonly LocString Desc = StringUtils.BuildingDesc(ID, "Plastic pipes provide slightly improved insulation and have no decor penalty.");
		public static readonly LocString Effect = StringUtils.BuildingEffect(ID, string.Concat(new string[]
		{
			"Carries ",
			UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
			" between ",
			UI.FormatAsLink("Outputs", "LIQUIDPIPING"),
			" and ",
			UI.FormatAsLink("Intakes", "LIQUIDPIPING"),
			".\n\nCan be run through wall and floor tile.",
			"\n\nProvides better insulation than normal liquid pipes."
		}));

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 1,
				anim: "plastic_utilities_liquid_kanim",
				hitpoints: 10,
				construction_time: 3f,
				construction_mass: TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				construction_materials: TUNING.MATERIALS.PLASTICS,
				melting_point: 800f,
				build_location_rule: BuildLocationRule.Anywhere,
				decor: TUNING.BUILDINGS.DECOR.NONE,
				noise: TUNING.NOISE_POLLUTION.NONE
			);
			def.ThermalConductivity = 0.5f;
			def.Floodable = false;
			def.Overheatable = false;
			def.Entombable = false;
			def.ViewMode = OverlayModes.LiquidConduits.ID;
			def.ObjectLayer = ObjectLayer.LiquidConduit;
			def.TileLayer = ObjectLayer.LiquidConduitTile;
			def.ReplacementLayer = ObjectLayer.ReplacementLiquidConduit;
			def.AudioCategory = "Metal";
			def.AudioSize = "small";
			def.BaseTimeUntilRepair = -1f;
			def.UtilityInputOffset = new CellOffset(0, 0);
			def.UtilityOutputOffset = new CellOffset(0, 0);
			def.SceneLayer = Grid.SceneLayer.LiquidConduits;
			def.isKAnimTile = true;
			def.isUtility = true;
			def.DragBuild = true;
			def.ReplacementTags = new List<Tag>();
			def.ReplacementTags.Add(GameTags.Pipes);
			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, ID);
			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);

			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

			go.AddOrGet<Conduit>().type = ConduitType.Liquid;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.GetComponent<Building>().Def.BuildingUnderConstruction.GetComponent<Constructable>().isDiggingRequired = false;

			go.AddComponent<EmptyConduitWorkable>();

			KAnimGraphTileVisualizer kanimGraphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
			kanimGraphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Liquid;
			kanimGraphTileVisualizer.isPhysicalBuilding = true;

			go.GetComponent<KPrefabID>().AddTag(GameTags.Pipes, false);

			go.AddComponent<PipeCellTracking>();

			LiquidConduitConfig.CommonConduitPostConfigureComplete(go);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			KAnimGraphTileVisualizer kanimGraphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
			kanimGraphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Liquid;
			kanimGraphTileVisualizer.isPhysicalBuilding = false;
		}
	}
}
