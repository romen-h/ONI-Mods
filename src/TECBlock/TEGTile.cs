using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Klei.AI;

using KSerialization;

using RomenH.Common;

using STRINGS;

using UnityEngine;

namespace RomenH.TECBlock
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class TEGTile : Generator
	{
		public class ActiveWattageStatusItem
		{
			public const string ID = "TEGTILE_ACTIVE_WATTAGE";
			public const string Prefix = "BUILDING";
			public static readonly LocString Name = StringUtils.StatusItemName(ID, Prefix, "Current Wattage: {Wattage}");
			public static readonly LocString Tooltip = StringUtils.StatusItemTooltip(ID, Prefix, "This TEG Tile is generating " + UI.FormatAsPositiveRate("{Wattage}") + "\n\nIncrease the " + UI.PRE_KEYWORD + "Temperature" + UI.PST_KEYWORD + " gradient to improve output.");
		}

		[SerializeField]
		public float maxTempDiff = 70f;

		[SerializeField]
		public float kDTUsPerWatt = 0.1f;

		[SerializeField]
		public float efficiency = 0.25f;

		private AttributeModifier efficiencyAttribute;

		private int topCell;
		private int bottomCell;
		private float maxHeatAbsorbedkDTU;

		private float currentGeneratedPower = 0;
		private float currentHeatAbsorbedkDTU = 0;
		private bool? topIsHotter = null;

		public class Instance : GameStateMachine<States, Instance, TEGTile, object>.GameInstance
		{
			public Instance(TEGTile master) : base(master)
			{ }

			public void UpdateState(float dt)
			{
				float topTemp = Grid.Temperature[master.topCell];
				float bottomTemp = Grid.Temperature[master.bottomCell];
				float minTemp = Mathf.Min(topTemp, bottomTemp);
				float temperatureDelta = Mathf.Abs(topTemp - bottomTemp);
				temperatureDelta = Mathf.Clamp(temperatureDelta, 0f, master.maxTempDiff);

				master.UpdateStatusItems();

				StateMachine.BaseState currentState = base.smi.GetCurrentState();
				if (temperatureDelta > 0 && minTemp > ModSettings.Instance.MinColdTemperature)
				{
					if (currentState != sm.on.generating)
					{
						smi.GoTo(sm.on.generating);
					}
				}
				else
				{
					if (currentState != sm.on.noGradient)
					{
						smi.GoTo(sm.on.noGradient);
					}
				}
			}

			public void UpdateGenerating(float dt)
			{
				float topTemp = Grid.Temperature[master.topCell];
				float bottomTemp = Grid.Temperature[master.bottomCell];
				float temperatureDelta = Mathf.Abs(topTemp - bottomTemp);
				master.topIsHotter = (topTemp > bottomTemp);
				temperatureDelta = Mathf.Clamp(temperatureDelta, 0f, master.maxTempDiff);

				float heatLoadRatio = temperatureDelta / master.maxTempDiff;
				float currentEfficiency = heatLoadRatio * master.efficiency;
				master.efficiencyAttribute.SetValue(currentEfficiency);

				float heatToAbsorbkDTU = currentEfficiency * master.maxHeatAbsorbedkDTU;

				if (heatToAbsorbkDTU > 0f)
				{
					float powerGenerated = heatToAbsorbkDTU / master.kDTUsPerWatt;
					master.currentGeneratedPower = powerGenerated;
					master.currentHeatAbsorbedkDTU = heatToAbsorbkDTU;
				}
				else
				{
					master.currentGeneratedPower = 0f;
					master.currentHeatAbsorbedkDTU = 0f;
				}
			}
		}

		public class States : GameStateMachine<States, Instance, TEGTile>
		{
			public class OnStates : State
			{
				public State noGradient;

				public State generating;
			}

			public State off;

			public OnStates on;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = off;

				off.EventTransition(GameHashes.OperationalChanged, on, (TEGTile.Instance smi) => smi.master.GetComponent<Operational>().IsOperational);
				off.PlayAnim("off");

				on.DefaultState(on.noGradient);
				on.EventTransition(GameHashes.OperationalChanged, off, (TEGTile.Instance smi) => !smi.master.GetComponent<Operational>().IsOperational);
				on.Update("UpdateOperational", delegate (TEGTile.Instance smi, float dt)
				{
					smi.UpdateState(dt);
				});

				on.noGradient.PlayAnim("off");

				on.generating.PlayAnim("on");
				on.generating.Enter(delegate (TEGTile.Instance smi)
				{
					smi.GetComponent<Operational>().SetActive(true);
				});
				on.generating.Update("UpdateGenerating", delegate (TEGTile.Instance smi, float dt)
				{
					smi.UpdateGenerating(dt);
				});
				on.generating.Exit(delegate (TEGTile.Instance smi)
				{
					smi.GetComponent<Operational>().SetActive(false);
					smi.master.currentGeneratedPower = 0f;
					smi.master.currentHeatAbsorbedkDTU = 0f;
				});
			}
		}

		private Instance smi;

		private static StatusItem activeWattageStatusItem;

		public static void InitializeStatusItems()
		{
			activeWattageStatusItem = new StatusItem(ActiveWattageStatusItem.ID, ActiveWattageStatusItem.Prefix, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.Power.ID);
			activeWattageStatusItem.resolveStringCallback = ResolveWattageStatus;
		}

		private static string ResolveWattageStatus(string str, object data)
		{
			TEGTile teg = (TEGTile)data;
			return str.Replace("{Wattage}", GameUtil.GetFormattedWattage(teg.currentGeneratedPower));
		}

		public void UpdateStatusItems()
		{
			KSelectable component = GetComponent<KSelectable>();
			StatusItem power_status_item = operational.IsActive ? activeWattageStatusItem : Db.Get().BuildingStatusItems.GeneratorOffline;
			component.SetStatusItem(Db.Get().StatusItemCategories.Power, power_status_item, this);
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			InitializeStatusItems();

			Attributes attributes = base.gameObject.GetAttributes();
			efficiencyAttribute = new AttributeModifier(Db.Get().Attributes.GeneratorOutput.Id, 0f, is_multiplier: true, is_readonly: false);
			attributes.Add(efficiencyAttribute);

			maxHeatAbsorbedkDTU = base.BaseWattageRating * kDTUsPerWatt;

			var topOffset = building.GetRotatedOffset(new CellOffset(0, 1));
			topCell = Grid.OffsetCell(building.GetCell(), topOffset);

			var bottomOffset = building.GetRotatedOffset(new CellOffset(0, -1));
			bottomCell = Grid.OffsetCell(building.GetCell(), bottomOffset);

			smi = new Instance(this);
			smi.StartSM();
		}

		public override void EnergySim200ms(float dt)
		{
			base.EnergySim200ms(dt);

			if (operational.isActiveAndEnabled)
			{
				GenerateJoules(currentGeneratedPower * dt);

				if (topIsHotter == true)
				{
					SimMessages.ModifyEnergy(topCell, -currentHeatAbsorbedkDTU * dt, 5000f, SimMessages.EnergySourceID.StructureTemperature);
				}
				else if (topIsHotter == false)
				{
					SimMessages.ModifyEnergy(bottomCell, -currentHeatAbsorbedkDTU * dt, 5000f, SimMessages.EnergySourceID.StructureTemperature);
				}
			}
		}
	}
}
