
using RomenH.Common;

using TUNING;

using UnityEngine;

namespace RomenH.Thresholds
{
	public class CautionThresholdWallConfig : IBuildingConfig
	{
		public const string ID = "RomenH_CautionThresholdWall";
		public static readonly LocString Name = StringUtils.BuildingName(ID, "Industrial Threshold Wall");
		public static readonly LocString Desc = StringUtils.BuildingDesc(ID, "A cautionary backwall pattern that marks the edge of a room.");
		public static readonly LocString Effect = StringUtils.BuildingEffect(ID, "Can be used to separate rooms or subtract cells from room sizes without occupying the cell.");

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 1,
				anim: "caution_threshold_walls_kanim",
				hitpoints: 30,
				construction_time: 10f,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				construction_materials: MATERIALS.RAW_MINERALS,
				melting_point: 1600f,
				build_location_rule: BuildLocationRule.NotInTiles,
				decor: DECOR.PENALTY.TIER0,
				noise: NOISE_POLLUTION.NONE);
			buildingDef.PermittedRotations = PermittedRotations.FlipH;
			buildingDef.IsFoundation = true;
			buildingDef.Entombable = false;
			buildingDef.Floodable = false;
			buildingDef.Overheatable = false;
			buildingDef.AudioCategory = "Metal";
			buildingDef.AudioSize = "small";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.DefaultAnimState = "off";
			buildingDef.ObjectLayer = ObjectLayer.Backwall;
			buildingDef.SceneLayer = ModSettings.Instance.HidePipesAndWires ? Grid.SceneLayer.LogicGatesFront : Grid.SceneLayer.Backwall;
			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
			go.AddComponent<ZoneTile>();
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RemoveLoopingSounds(go);
			go.AddOrGet<Threshold>();
		}
	}
}
