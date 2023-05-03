using System.Collections.Generic;
using RomenH.Common;
using STRINGS;
using TUNING;
using UnityEngine;

using BUILDINGS = TUNING.BUILDINGS;

using static ResearchTypes;

namespace RomenH.LogicScheduleSensor
{
	public class LogicScheduleSensorConfig : IBuildingConfig
	{
		public const string ID = "LogicScheduleSensor";

		public static readonly LocString Name = StringUtils.BuildingName(ID, "Logic Schedule Sensor");

		public static readonly LocString Desc = StringUtils.BuildingDesc(ID,
			"Schedule sensors allow systems to be synchronized to a specific schedule and shift.");

		public static readonly LocString Effect = StringUtils.BuildingEffect(ID, string.Concat("Sends a ",
			UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
			" when the selected ",
			UI.FormatAsLink("Schedule", "SCHEDULE"),
			" enters the selected shift."));

		public static readonly LocString _LogicPort = StringUtils.BuildingLogicPortName(ID, "Schedule Shift");

		public static readonly LocString _LogicPortActive = StringUtils.BuildingLogicPortActive(ID,
			"Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when the selected " +
			UI.FormatAsLink("Schedule", "SCHEDULE") + " enters the selected shift");

		public static readonly LocString _LogicPortInactive = StringUtils.BuildingLogicPortInactive(ID,
			"Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));

		public override BuildingDef CreateBuildingDef()
		{
			string iD = "LogicScheduleSensor";
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(
				id: iD,
				width: 1,
				height: 1,
				decor: DECOR.NONE,
				noise: NOISE_POLLUTION.NONE,
				anim: "logic_schedule_sensor_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER1,
				build_location_rule: BuildLocationRule.Anywhere,
				construction_materials: MATERIALS.REFINED_METALS,
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER3,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER0,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2
			);
		
			buildingDef.Floodable = false;
			buildingDef.Entombable = false;
			buildingDef.Overheatable = false;
			buildingDef.AlwaysOperational = true;

			buildingDef.AudioCategory = "Metal";

			buildingDef.ViewMode = OverlayModes.Logic.ID;
			buildingDef.SceneLayer = Grid.SceneLayer.Building;

			SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
			SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);

			buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
			{
				LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), _LogicPort.ToString(),
					_LogicPortActive.ToString(), _LogicPortInactive.ToString(), false, false)
			};

			GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);

			return buildingDef;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<LogicPorts>();

			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			GeneratedBuildings.InitializeLogicPorts(go, CreateBuildingDef());

			LogicScheduleSensor logicScheduleSensor = go.AddOrGet<LogicScheduleSensor>();
			logicScheduleSensor.manuallyControlled = false;
		}
	}
}
