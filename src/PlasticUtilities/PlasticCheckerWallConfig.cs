using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace RomenH.PlasticUtilities
{
	public class PlasticCheckerWallConfig : IBuildingConfig
	{
		public const string ID = "RomenH_PlasticCheckerWall";

		public const string Name = "Plastic Checker Wall";

		public const string Desc = "Plastic walls can be used in conjunction with tiles to build airtight rooms on the surface.";

		public const string Effect = "A checkerboard plastic wall tile.";

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 1,
				anim: "plastic_checker_wall_kanim",
				hitpoints: 30,
				construction_time: 30f,
				construction_mass: TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
				construction_materials: TUNING.MATERIALS.PLASTICS,
				melting_point: 800f,
				build_location_rule: BuildLocationRule.NotInTiles,
				decor: TUNING.DECOR.BONUS.TIER0,
				noise: TUNING.NOISE_POLLUTION.NONE
			);
			def.Entombable = false;
			def.Floodable = false;
			def.Overheatable = false;
			def.AudioCategory = "Metal";
			def.AudioSize = "small";
			def.BaseTimeUntilRepair = -1f;
			def.DefaultAnimState = "off";
			def.ObjectLayer = ObjectLayer.Backwall;
			def.SceneLayer = Grid.SceneLayer.Backwall;
			return def;
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
		}
	}
}
