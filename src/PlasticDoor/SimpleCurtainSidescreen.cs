using PeterHan.PLib.Core;
using PeterHan.PLib.UI;

using UnityEngine;

namespace Curtain
{
	public class SimpleCurtainSidescreen : SideScreenContent
	{
		private Curtain curtain;

		protected override void OnPrefabInit()
		{
			BuildPanel();

			base.OnPrefabInit();
		}

		public override string GetTitle()
		{
			return "Plastic Door Permissions";
		}

		public override bool IsValidForTarget(GameObject target)
		{
			return target.GetComponent<Curtain>() != null;
		}

		public override void SetTarget(GameObject target)
		{
			if (target == null)
				PUtil.LogError("Invalid target specified");
			else
			{
				curtain = target.GetComponent<Curtain>();
			}
		}

		public override void ClearTarget()
		{
			curtain = null;
		}

		private void BuildPanel()
		{
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

			var mainRow = new PPanel("Toggle Lock")
			{
				FlexSize = Vector2.right,
				Alignment = TextAnchor.MiddleCenter,
				Spacing = 10,
				Direction = PanelDirection.Horizontal,
				Margin = margin
			};

			var lockBtn = new PButton("LockButton")
			{
				Text = "Lock"
			};
			lockBtn.SetKleiBlueStyle();
			lockBtn.OnClick = (obj) =>
			{
				if (curtain == null) return;
				curtain.QueueStateChange(Curtain.ControlState.Locked);
			};

			var unlockBtn = new PButton("UnlockButton")
			{
				Text = "Auto"
			};
			unlockBtn.SetKleiBlueStyle();
			unlockBtn.OnClick = (obj) =>
			{
				if (curtain == null) return;
				curtain.QueueStateChange(Curtain.ControlState.Auto);
			};

			var openBtn = new PButton("OpenButton")
			{
				Text = "Open"
			};
			openBtn.SetKleiBlueStyle();
			openBtn.OnClick = (obj) =>
			{
				if (curtain == null) return;
				curtain.QueueStateChange(Curtain.ControlState.Open);
			};

			mainRow.AddChild(lockBtn);
			mainRow.AddChild(unlockBtn);
			mainRow.AddChild(openBtn);
			mainPanel.AddChild(mainRow);
			ContentContainer = mainPanel.Build();
			ContentContainer.SetParent(gameObject);
		}
	}
}
