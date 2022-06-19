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
	public class PlasticGasConduitConfig : IBuildingConfig
	{
		public const string ID = "RomenH_PlasticGasConduit";

		public const string Name = "Plastic Gas Pipe";

		public const string Desc = "Plastic pipes provide slightly improved insulation and have no decor penalty.";

		public static readonly string Effect = string.Concat(new string[]
		{
			"Carries ",
			UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
			" between ",
			UI.FormatAsLink("Outputs", "GASPIPING"),
			" and ",
			UI.FormatAsLink("Intakes", "GASPIPING"),
			".\n\nCan be run through wall and floor tile.",
			"\n\nProvides better insulation than normal gas pipes."
		});


		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 1,
				anim: "plastic_utilities_gas_kanim",
				hitpoints: 10,
				construction_time: 3f,
				construction_mass: TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0,
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
			def.ViewMode = OverlayModes.GasConduits.ID;
			def.ObjectLayer = ObjectLayer.GasConduit;
			def.TileLayer = ObjectLayer.GasConduitTile;
			def.ReplacementLayer = ObjectLayer.ReplacementGasConduit;
			def.AudioCategory = "Metal";
			def.AudioSize = "small";
			def.BaseTimeUntilRepair = 0f;
			def.UtilityInputOffset = new CellOffset(0, 0);
			def.UtilityOutputOffset = new CellOffset(0, 0);
			def.SceneLayer = Grid.SceneLayer.GasConduits;
			def.isKAnimTile = true;
			def.isUtility = true;
			def.DragBuild = true;
			def.ReplacementTags = new List<Tag>();
			def.ReplacementTags.Add(GameTags.Vents);
			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, ID);
			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);

			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

			go.AddOrGet<Conduit>().type = ConduitType.Gas;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.GetComponent<Building>().Def.BuildingUnderConstruction.GetComponent<Constructable>().isDiggingRequired = false;

			go.AddComponent<EmptyConduitWorkable>();

			KAnimGraphTileVisualizer kanimGraphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
			kanimGraphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Gas;
			kanimGraphTileVisualizer.isPhysicalBuilding = true;
			
			go.GetComponent<KPrefabID>().AddTag(GameTags.Vents, false);

			go.AddComponent<PipeCellTracking>();

			LiquidConduitConfig.CommonConduitPostConfigureComplete(go);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			KAnimGraphTileVisualizer kanimGraphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
			kanimGraphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Gas;
			kanimGraphTileVisualizer.isPhysicalBuilding = false;
		}
	}
}
