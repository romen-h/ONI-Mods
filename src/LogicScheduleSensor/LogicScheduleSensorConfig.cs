using System.Collections.Generic;

using TUNING;

using UnityEngine;

namespace RomenH.LogicScheduleSensor
{
	public class LogicScheduleSensorConfig : IBuildingConfig
	{
		public static string ID = "LogicScheduleSensor";

		public static readonly LogicPorts.Port OUTPUT_PORT = LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), ModStrings.STRINGS.BUILDINGS.LOGICSCHEDULESENSOR.LOGIC_PORT, ModStrings.STRINGS.BUILDINGS.LOGICSCHEDULESENSOR.LOGIC_PORT_ACTIVE, ModStrings.STRINGS.BUILDINGS.LOGICSCHEDULESENSOR.LOGIC_PORT_INACTIVE);

		public override BuildingDef CreateBuildingDef()
		{
			string iD = ID;
			int width = 1;
			int height = 1;
			string anim = "logic_schedule_sensor_kanim";
			int hitpoints = 30;
			float construction_time = 30f;
			float[] tIER = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
			string[] rEFINED_METALS = MATERIALS.REFINED_METALS;
			float melting_point = 1600f;
			BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
			EffectorValues nONE = NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(iD, width, height, anim, hitpoints, construction_time, tIER, rEFINED_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, nONE);
			buildingDef.Overheatable = false;
			buildingDef.Floodable = false;
			buildingDef.Entombable = false;
			buildingDef.ViewMode = OverlayModes.Logic.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.SceneLayer = Grid.SceneLayer.Building;
			SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
			SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);

			buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
			{
				OUTPUT_PORT
			};

			GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);

			return buildingDef;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			LogicPorts logicPorts = go.AddOrGet<LogicPorts>();
			logicPorts.inputPortInfo = null;
			logicPorts.outputPortInfo = new LogicPorts.Port[1]
			{
				OUTPUT_PORT
			};
			LogicScheduleSensor logicScheduleSensor = go.AddOrGet<LogicScheduleSensor>();
			logicScheduleSensor.manuallyControlled = false;
		}
	}
}
