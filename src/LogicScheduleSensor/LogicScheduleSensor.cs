using System;
using System.Collections.Generic;
using KSerialization;
using RomenH.ScheduleSensor;
using UnityEngine;

namespace RomenH.LogicScheduleSensor
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class LogicScheduleSensor : Switch, ISaveLoadable, ISim200ms
	{
		[Serialize]
		public int scheduleIndex = 0;

		[Serialize]
		public int blockTypeIndex = 0;

		[MyCmpGet]
		public KBatchedAnimController anim;

		private MeterController meter;

		private bool wasOn = false;

		public override void OnSpawn()
		{
			base.OnSpawn();
			base.OnToggle += OnSwitchToggled;

			ScheduleManager.Instance.onSchedulesChanged += ScheduleManager_onSchedulesChanged;

			meter = new MeterController(anim, "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.Building, Array.Empty<string>());
			UpdateLogicCircuit();
			UpdateVisualState(force: true);
			wasOn = switchedOn;
		}

		public override void OnCleanUp()
		{
			ScheduleManager.Instance.onSchedulesChanged -= ScheduleManager_onSchedulesChanged;
		}

		// Clean up selected schedule index whenever the schedules change
		private void ScheduleManager_onSchedulesChanged(List<Schedule> list)
		{
			if (scheduleIndex < 0 || scheduleIndex >= list.Count)
			{
				scheduleIndex = 0;
			}
		}

		public void Sim200ms(float dt)
		{
			try
			{
				Schedule s = ScheduleManager.Instance.GetSchedules()[scheduleIndex];
				
				float meterPercent = GameClock.Instance.GetCurrentCycleAsPercentage();
				meter.SetPositionPercent(meterPercent);

				ScheduleBlock b = s.GetCurrentScheduleBlock();
				string currentScheduleGroup = b.GroupId;

				meter.SetSymbolTint("face", GetBlockColor(b));

				string selectedScheduleGroup = Db.Get().ScheduleGroups.allGroups[blockTypeIndex].Id;

				bool state = (string.Equals(currentScheduleGroup, selectedScheduleGroup));
				SetState(state);
			}
			catch (Exception ex)
			{
				Debug.Log($"Failed to update Schedule Sensor: {ex.ToString()}");
				SetState(false);
			}
		}

		private void OnSwitchToggled(bool toggled_on)
		{
			UpdateLogicCircuit();
			UpdateVisualState();
		}

		private void UpdateLogicCircuit()
		{
			LogicPorts component = GetComponent<LogicPorts>();
			component?.SendSignal(LogicSwitch.PORT_ID, switchedOn ? 1 : 0);
		}

		private void UpdateVisualState(bool force = false)
		{
			if (wasOn != switchedOn || force)
			{
				wasOn = switchedOn;
				anim.Play((switchedOn) ? "on" : "off");
			}
		}

		private Color GetBlockColor(ScheduleBlock b)
		{
			if (ScheduleGroupInfo.BlockColors == null) return Color.white;
			if (ScheduleGroupInfo.BlockColors.TryGetValue(b.GroupId, out Color col))
			{
				return col;
			}

			return Color.white;
		}
	}
}
