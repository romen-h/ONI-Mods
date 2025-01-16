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
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 1,
				anim: "logic_schedule_sensor_kanim",
				hitpoints: 30,
				construction_time: 30f,
				construction_mass: TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0,
				construction_materials: MATERIALS.REFINED_METALS,
				melting_point: 1600f,
				build_location_rule: BuildLocationRule.Anywhere,
				decor: TUNING.BUILDINGS.DECOR.PENALTY.TIER0,
				noise: NOISE_POLLUTION.NONE);
			def.Overheatable = false;
			def.Floodable = false;
			def.Entombable = false;
			def.ViewMode = OverlayModes.Logic.ID;
			def.AudioCategory = "Metal";
			def.SceneLayer = Grid.SceneLayer.Building;
			def.AlwaysOperational = true;
			def.LogicOutputPorts = new List<LogicPorts.Port>
			{
				LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), LogicPort, LogicPortActive, LogicPortInactive, true, false)
			};
			SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
			SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
			GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);

			return def;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);

			LogicScheduleSensor logicScheduleSensor = go.AddOrGet<LogicScheduleSensor>();
			logicScheduleSensor.manuallyControlled = false;
		}
	}
}
