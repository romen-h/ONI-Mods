using System;

using KSerialization;

using UnityEngine;

namespace RomenH.LogicScheduleSensor
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class LogicScheduleSensor : Switch, ISaveLoadable, ISim200ms
	{
		[MyCmpGet]
		public KBatchedAnimController anim;

		private MeterController meter;

		[Serialize]
		public int scheduleIndex = 0;

		[Serialize]
		public int blockTypeIndex = 0;

		private bool wasOn = false;

		protected override void OnSpawn()
		{
			base.OnSpawn();
			base.OnToggle += OnSwitchToggled;
			meter = new MeterController(anim, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.Building, Array.Empty<string>());
			UpdateLogicCircuit();
			UpdateVisualState(force: true);
			wasOn = switchedOn;
		}

		public void Sim200ms(float dt)
		{
			try
			{
				Schedule s = ScheduleManager.Instance.GetSchedules()[scheduleIndex];
				int currentScheduleBlock = Schedule.GetBlockIdx();

				float meterPercent = (float)(currentScheduleBlock + 1) / 24f;
				meter.SetPositionPercent(meterPercent);

				ScheduleBlock b = s.GetBlock(currentScheduleBlock);
				string currentScheduleGroup = b.GroupId;

				anim.SetSymbolTint("face", GetBlockColor(b));

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
			if (ScheduleScreen_OnPrefabInit_Patch.blockColors == null) return Color.white;
			if (ScheduleScreen_OnPrefabInit_Patch.blockColors.TryGetValue(b.GroupId, out ColorStyleSetting col))
			{
				return col.activeColor;
			}

			return Color.white;
		}
	}
}
