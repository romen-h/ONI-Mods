using System.Collections.Generic;

using PeterHan.PLib.Core;
using PeterHan.PLib.UI;

using RomenH.Common;

using UnityEngine;
using UnityEngine.UI;

using Dropdown = UnityEngine.UI.Dropdown;

namespace RomenH.LogicScheduleSensor
{
	public class LogicScheduleSensorSideScreen : SideScreenContent
	{
		public static readonly LocString Name = StringUtils.SideScreenName(nameof(LogicScheduleSensorSideScreen), "Schedule Sensor Settings");

		public static readonly LocString ScheduleLabel = new LocString("Schedule:", "STRINGS.UI.LOGICSCHEDULESENSORSIDESCREEN.SCHEDULE");

		public static readonly LocString ShiftLabel = new LocString("Shift:", "STRINGS.UI.LOGICSCHEDULESENSORSIDESCREEN.SHIFT");

		private LogicScheduleSensor sensor;

		private Button _testButton = null;
		private Dropdown _scheduleDropdown = null;
		private Dropdown _blockTypeDropdown = null;

		public override string GetTitle()
		{
			return Name;
		}

		public override void OnPrefabInit()
		{
			ScheduleScreenEntry_OnNameChanged_Patch.ScheduleNameChanged += ScheduleScreenEntry_OnNameChanged_Patch_ScheduleNameChanged;
			ScheduleManager.Instance.onSchedulesChanged += ScheduleManager_onSchedulesChanged;
			BuildPanel();

			base.OnPrefabInit();
		}

		public override void OnCleanUp()
		{
			ScheduleScreenEntry_OnNameChanged_Patch.ScheduleNameChanged -= ScheduleScreenEntry_OnNameChanged_Patch_ScheduleNameChanged;
			ScheduleManager.Instance.onSchedulesChanged -= ScheduleManager_onSchedulesChanged;

			base.OnCleanUp();
		}

		public override bool IsValidForTarget(GameObject target)
		{
			return target.GetComponent<LogicScheduleSensor>() != null;
		}

		public override void SetTarget(GameObject target)
		{
			if (target == null) return;

			sensor = target.GetComponent<LogicScheduleSensor>();
			UpdateScheduleSelection();
			UpdateBlockTypeSelection();
		}

		public override void ClearTarget()
		{
			sensor = null;
			UpdateScheduleSelection();
			UpdateBlockTypeSelection();
		}

		private void ScheduleScreenEntry_OnNameChanged_Patch_ScheduleNameChanged(Schedule obj)
		{
			UpdateScheduleOptions();
			UpdateScheduleSelection();
		}

		private void ScheduleManager_onSchedulesChanged(List<Schedule> list)
		{
			UpdateScheduleOptions();
			UpdateScheduleSelection();
		}

		private void BuildPanel()
		{
			GameObject contentContainer = new GameObject();
			var rootPanelTransform = contentContainer.AddComponent<RectTransform>();
			var rootPanelLayout = contentContainer.AddComponent<LayoutElement>();
			var rootPanelLayoutGroup = contentContainer.AddComponent<VerticalLayoutGroup>();
			rootPanelLayoutGroup.childAlignment = TextAnchor.UpperLeft;
			rootPanelLayoutGroup.childScaleHeight = true;
			rootPanelLayoutGroup.padding = new RectOffset(10, 10, 10, 10);
			rootPanelLayoutGroup.spacing = 10f;
			rootPanelLayoutGroup.childControlHeight = false;
			
			contentContainer.SetParent(gameObject);
			contentContainer.SetActive(true);
			ContentContainer = contentContainer;
			
			var scheduleLabelObj = UIFactory.MakeLabel(ScheduleLabel);
			scheduleLabelObj.SetParent(contentContainer);

			var scheduleDropdownObj = UIFactory.MakeDropdown((value) =>
			{
				if (sensor != null)
				{
					sensor.scheduleIndex = value;
				}
			});
			_scheduleDropdown = scheduleDropdownObj.SafeGetComponent<Dropdown>();
			scheduleDropdownObj.SetParent(contentContainer);

			var shiftLabelObj = UIFactory.MakeLabel(ShiftLabel);
			shiftLabelObj.SetParent(contentContainer);

			var blockTypeDropdownObj = UIFactory.MakeDropdown((value) =>
			{
				if (sensor != null)
				{
					sensor.blockTypeIndex = value;
				}
			});
			_blockTypeDropdown = blockTypeDropdownObj.GetComponent<Dropdown>();
			blockTypeDropdownObj.SetParent(contentContainer);

			UpdateScheduleOptions();
			UpdateBlockTypeOptions();

			UpdateScheduleSelection();
			UpdateBlockTypeSelection();
		}

		private void UpdateScheduleOptions()
		{
			if (_scheduleDropdown == null) return;

			var schedules = ScheduleManager.Instance.GetSchedules();
			_scheduleDropdown.ClearOptions();
			foreach (var schedule in schedules)
			{
				_scheduleDropdown.options.Add(new Dropdown.OptionData(schedule.name));
			}
		}

		private void UpdateScheduleSelection()
		{
			if (_scheduleDropdown == null) return;
			if (sensor == null)
			{
				_scheduleDropdown.enabled = false;
				return;
			}
			else
			{
				_scheduleDropdown.enabled = true;
			}

			_scheduleDropdown.SetValueWithoutNotify(sensor.scheduleIndex);
		}

		private void UpdateBlockTypeOptions()
		{
			if (_blockTypeDropdown == null) return;

			var groups = Db.Get().ScheduleGroups.allGroups;
			_blockTypeDropdown.ClearOptions();
			foreach (ScheduleGroup g in groups)
			{
				_blockTypeDropdown.options.Add(new Dropdown.OptionData(g.Name));
			}
		}

		private void UpdateBlockTypeSelection()
		{
			if (_blockTypeDropdown == null) return;
			if (sensor == null)
			{
				_blockTypeDropdown.enabled = false;
				return;
			}
			else
			{
				_blockTypeDropdown.enabled = true;
			}

			_blockTypeDropdown.SetValueWithoutNotify(sensor.blockTypeIndex);
		}
	}
}
