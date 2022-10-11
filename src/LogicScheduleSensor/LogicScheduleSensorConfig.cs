using System.Collections.Generic;

using RomenH.Common;

using STRINGS;

using TUNING;

using UnityEngine;

namespace RomenH.LogicScheduleSensor
{
	public class LogicScheduleSensorConfig : IBuildingConfig
	{
		public const string ID = "LogicScheduleSensor";

		public static readonly LocString Name = StringUtils.BuildingName(ID, "Logic Schedule Sensor");

		public static readonly LocString Desc = StringUtils.BuildingDesc(ID, "Schedule sensors allow systems to be synchronized to a specific schedule and shift.");

		public static readonly LocString Effect = StringUtils.BuildingEffect(ID, string.Concat("Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when the selected ",
					UI.FormatAsLink("Schedule", "SCHEDULE"),
					" enters the selected shift."));

		public static readonly LocString LogicPort = StringUtils.BuildingLogicPortName(ID, "Schedule Shift");

		public static readonly LocString LogicPortActive = StringUtils.BuildingLogicPortActive(ID, "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when the selected " + UI.FormatAsLink("Schedule", "SCHEDULE") + " enters the selected shift");

		public static readonly LocString LogicPortInactive = StringUtils.BuildingLogicPortInactive(ID, "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));

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
				LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), LogicPort.ToString(), LogicPortActive.ToString(), LogicPortInactive.ToString())
			};

			GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);

			return buildingDef;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);

			LogicScheduleSensor logicScheduleSensor = go.AddOrGet<LogicScheduleSensor>();
			logicScheduleSensor.manuallyControlled = false;
		}
	}
}
