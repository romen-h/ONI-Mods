
using RomenH.Common;

using TUNING;

using UnityEngine;

namespace RomenH.Thresholds
{
	public class LogicThresholdWallConfig : IBuildingConfig
	{
		public const string ID = "RomenH_LogicThresholdWall";
		public static readonly LocString Name = StringUtils.BuildingName(ID, "Programmable Threshold Wall");
		public static readonly LocString Desc = StringUtils.BuildingDesc(ID, "A backwall with an embedded LED matrix that can display various patterns.");
		public static readonly LocString Effect = StringUtils.BuildingEffect(ID, "Can be controlled with automation to toggle the threshold wall effect and Duplicant pathing.");
		public static readonly LocString InputPortDesc = StringUtils.LogicPortDesc(ID, "Bit 1: Room Toggle\n    Bit 2: Pathing Toggle\n    Bits 3 & 4: Pattern Select");
		public static readonly LocString InputPortActive = StringUtils.LogicPortInputActive(ID, "Active");
		public static readonly LocString InputPortInactive = StringUtils.BuildingLogicPortInactive(ID, "Inactive");

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 1,
				anim: "ledmatrix_kanim",
				hitpoints: 30,
				construction_time: 10f,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				construction_materials: MATERIALS.REFINED_METALS,
				melting_point: 1600f,
				build_location_rule: BuildLocationRule.NotInTiles,
				decor: DECOR.BONUS.TIER0,
				noise: NOISE_POLLUTION.NONE);
			buildingDef.IsFoundation = true;
			buildingDef.Entombable = false;
			buildingDef.Floodable = false;
			buildingDef.Overheatable = false;
			buildingDef.AudioCategory = "Metal";
			buildingDef.AudioSize = "small";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.DefaultAnimState = "off";
			buildingDef.ObjectLayer = ObjectLayer.Backwall;
			buildingDef.SceneLayer = Grid.SceneLayer.LogicGatesFront;
			buildingDef.PermittedRotations = PermittedRotations.R360;
			buildingDef.LogicInputPorts = new System.Collections.Generic.List<LogicPorts.Port> { LogicPorts.Port.RibbonInputPort(LogicThresholdWall.INPUT_PORT_ID, new CellOffset(0, 0), InputPortDesc.ToString(), InputPortActive.ToString(), InputPortInactive.ToString(), true, false) };
			GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);
			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
			go.AddComponent<ZoneTile>();
			go.AddComponent<LogicThresholdWall>();
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RemoveLoopingSounds(go);
			go.AddOrGet<Threshold>();
		}
	}
}
