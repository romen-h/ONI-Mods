#if ENABLE_SIDESCREEN
using PeterHan.PLib;
using PeterHan.PLib.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RomenMods.StirlingEngineMod
{
	public class StirlingEngineSideScreen : SideScreenContent, ISim1000ms
	{
		private StirlingEngine engine;

		protected override void OnPrefabInit()
		{
			RebuildPanel();
			base.OnPrefabInit();
		}

		private void RebuildPanel()
		{
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

			if (engine != null)
			{
				var efficiencyRow = new PPanel("EfficiencyRow")
				{
					FlexSize = Vector2.right,
					Alignment = TextAnchor.MiddleCenter,
					Spacing = 10,
					Direction = PanelDirection.Horizontal,
					Margin = margin
				};

				efficiencyRow.AddChild(new PLabel("Efficiency")
				{
					TextAlignment = TextAnchor.MiddleRight,
					ToolTip = "Indicates the percentage of input heat that is currently being converted to power.",
					Text = $"Current Efficiency: {(engine.currentEfficiency * 100f):F0} %",
					TextStyle = PUITuning.Fonts.TextDarkStyle
				});

				mainPanel.AddChild(efficiencyRow);

				var powerRow = new PPanel("PowerRow")
				{
					FlexSize = Vector2.right,
					Alignment = TextAnchor.MiddleCenter,
					Spacing = 10,
					Direction = PanelDirection.Horizontal,
					Margin = margin
				};

				powerRow.AddChild(new PLabel("Power")
				{
					TextAlignment = TextAnchor.MiddleRight,
					ToolTip = "Indicates the amount of power currently being output into the circuit.",
					Text = $"Estimated Power Output: {engine.currentGeneratedPower:F0} W",
					TextStyle = PUITuning.Fonts.TextDarkStyle
				});

				mainPanel.AddChild(powerRow);

				var heatRow = new PPanel("HeatRow")
				{
					FlexSize = Vector2.right,
					Alignment = TextAnchor.MiddleCenter,
					Spacing = 10,
					Direction = PanelDirection.Horizontal,
					Margin = margin
				};

				heatRow.AddChild(new PLabel("Heat")
				{
					TextAlignment = TextAnchor.MiddleRight,
					ToolTip = "Indicates the amount of heat currently being added to the building temperature.",
					Text = $"Estimated Heat Output: {engine.currentGeneratedHeat:F0} DTU/s",
					TextStyle = PUITuning.Fonts.TextDarkStyle
				});

				mainPanel.AddChild(heatRow);

				ContentContainer = mainPanel.Build();
				ContentContainer.SetParent(gameObject);
			}
		}

		public override string GetTitle()
		{
			return "Stirling Engine Status";
		}

		public override void ClearTarget()
		{
			engine = null;
		}

		public override bool IsValidForTarget(GameObject target)
		{
			return target.GetComponent<StirlingEngine>() != null;
		}

		public override void SetTarget(GameObject target)
		{
			if (target == null)
				PUtil.LogError("Invalid target specified");
			else
			{
				engine = target.GetComponent<StirlingEngine>();
				RebuildPanel();
			}
		}

		public void Sim1000ms(float dt)
		{
			if (engine != null)
			{
				RebuildPanel();
			}
		}
	}
}
#endif