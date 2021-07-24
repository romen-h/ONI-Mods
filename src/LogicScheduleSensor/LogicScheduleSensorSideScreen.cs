using System.Collections.Generic;

using PeterHan.PLib.Core;
using PeterHan.PLib.UI;

using RomenH.Common;

using UnityEngine;

namespace RomenH.LogicScheduleSensor
{
	public class LogicScheduleSensorSideScreen : SideScreenContent
	{
		List<StringListOption> schedules;
		List<StringListOption> groups;

		private LogicScheduleSensor sensor;

		private GameObject scheduleCombo;
		private GameObject groupCombo;

		protected override void OnPrefabInit()
		{
			ScheduleScreenEntry_OnNameChanged_Patch.ScheduleNameChanged += ScheduleScreenEntry_OnNameChanged_Patch_ScheduleNameChanged;
			ScheduleManager.Instance.onSchedulesChanged += ScheduleManager_onSchedulesChanged;
			RebuildPanel();

			base.OnPrefabInit();
		}

		private void ScheduleScreenEntry_OnNameChanged_Patch_ScheduleNameChanged(Schedule obj)
		{
			if (sensor != null)
				RebuildPanel();
		}

		private void ScheduleManager_onSchedulesChanged(List<Schedule> list)
		{
			if (sensor != null)
				RebuildPanel();
		}

		private void RebuildPanel()
		{
			// Update schedule data

			int selectedSchedule = (sensor != null) ? sensor.scheduleIndex : 0;
			int selectedGroup = (sensor != null) ? sensor.blockTypeIndex : 0;

			schedules = new List<StringListOption>();
			var slist = ScheduleManager.Instance.GetSchedules();
			foreach (Schedule s in slist)
			{
				schedules.Add(new StringListOption(s.name));
			}

			if (selectedSchedule < 0 || selectedSchedule >= schedules.Count)
			{
				selectedSchedule = 0;
				if (sensor != null)
				{
					sensor.scheduleIndex = selectedSchedule;
				}
			}

			groups = new List<StringListOption>();
			var glist = Db.Get().ScheduleGroups.allGroups;
			foreach (ScheduleGroup g in glist)
			{
				groups.Add(new StringListOption(g.Name));
			}

			if (selectedGroup < 0 || selectedGroup >= groups.Count)
			{
				selectedGroup = 0;
				if (sensor != null)
				{
					sensor.blockTypeIndex = selectedGroup;
				}
			}

			// Rebuild UI

			if (ContentContainer != null)
			{
				Destroy(ContentContainer);
				ContentContainer = null;
			}

			var margin = new RectOffset(8, 8, 8, 8);
			var baseLayout = gameObject.GetComponent<BoxLayoutGroup>();
			if (baseLayout != null)
			{
				baseLayout.Params = new BoxLayoutParams()
				{
					Margin = margin,
					Direction = PanelDirection.Vertical,
					Alignment = TextAnchor.UpperCenter,
					Spacing = 8
				};
			}

			var mainPanel = new PPanel();

			var scheduleRow = new PPanel("Schedule Select")
			{
				FlexSize = Vector2.right,
				Alignment = TextAnchor.MiddleCenter,
				Spacing = 10,
				Direction = PanelDirection.Horizontal,
				Margin = margin
			};

			scheduleRow.AddChild(new PLabel("Schedule")
			{
				TextAlignment = TextAnchor.MiddleRight,
				ToolTip = "TODO: Schedule Label Tooltip",
				Text = "Schedule",
				TextStyle = PUITuning.Fonts.TextDarkStyle
			});

			var scb = new PeterHan.PLib.UI.PComboBox<StringListOption>("Schedule Select")
			{
				Content = schedules,
				MinWidth = 100,
				InitialItem = schedules[selectedSchedule],
				ToolTip = "TODO: Schedule Select Tooltip",
				TextStyle = PUITuning.Fonts.TextLightStyle,
				TextAlignment = TextAnchor.MiddleLeft,
				OnOptionSelected = SetSchedule
			};

			scb.OnRealize += (obj) =>
			{
				scheduleCombo = obj;
			};
			scheduleRow.AddChild(scb);
			mainPanel.AddChild(scheduleRow);

			var groupRow = new PPanel("Group Select")
			{
				FlexSize = Vector2.right,
				Alignment = TextAnchor.MiddleCenter,
				Spacing = 10,
				Direction = PanelDirection.Horizontal,
				Margin = margin
			};

			groupRow.AddChild(new PLabel("Group")
			{
				TextAlignment = TextAnchor.MiddleRight,
				ToolTip = "TODO: Group Label Tooltip",
				Text = "Group",
				TextStyle = PUITuning.Fonts.TextDarkStyle
			});

			var bcb = new PComboBox<StringListOption>("Group Select")
			{
				Content = groups,
				MinWidth = 100,
				InitialItem = groups[selectedGroup],
				ToolTip = "TODO: Group Select Tooltip",
				TextStyle = PUITuning.Fonts.TextLightStyle,
				TextAlignment = TextAnchor.MiddleLeft,
				OnOptionSelected = SetGroup
			};

			bcb.OnRealize += (obj) =>
			{
				groupCombo = obj;
			};
			groupRow.AddChild(bcb);
			mainPanel.AddChild(groupRow);

			ContentContainer = mainPanel.Build();
			ContentContainer.SetParent(gameObject);

			if (scheduleCombo != null)
				PComboBox<StringListOption>.SetSelectedItem(scheduleCombo, schedules[selectedSchedule]);

			if (groupCombo != null)
				PComboBox<StringListOption>.SetSelectedItem(groupCombo, groups[selectedGroup]);
		}

		private void SetSchedule(GameObject obj, StringListOption option)
		{
			int index = schedules.IndexOf(option);

			if (sensor != null && index >= 0 && index < schedules.Count)
			{
				sensor.scheduleIndex = index;
			}
		}

		private void SetGroup(GameObject obj, StringListOption option)
		{
			int index = groups.IndexOf(option);

			if (sensor != null && index >= 0 && index < groups.Count)
			{
				sensor.blockTypeIndex = index;
			}
		}

		public override string GetTitle()
		{
			return "Schedule Sensor Settings";
		}

		public override bool IsValidForTarget(GameObject target)
		{
			return target.GetComponent<LogicScheduleSensor>() != null;
		}

		public override void ClearTarget()
		{
			sensor = null;
		}

		public override void SetTarget(GameObject target)
		{
			if (target == null)
				PUtil.LogError("Invalid target specified");
			else
			{
				sensor = target.GetComponent<LogicScheduleSensor>();
				RebuildPanel();
			}
		}
	}
}
