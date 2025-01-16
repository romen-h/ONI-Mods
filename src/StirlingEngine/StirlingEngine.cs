#define USE_EFFICIENCY_FOR_HEAT
#define USE_ALL_HEAT
#define PRODUCE_PROPORTIONAL_HEAT

using System;

using KSerialization;

using RomenH.Common;

using STRINGS;

using UnityEngine;

namespace RomenH.StirlingEngine
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class StirlingEngine : Generator
	{
		public class ActiveStatusItem
		{
			public const string ID = "STIRLING_ACTIVE";
			public static readonly LocString Name = StringUtils.StatusItemName(ID, "BUILDING", "Active");
			public static readonly LocString Tooltip = StringUtils.StatusItemTooltip(ID, "BUILDING", "This engine is running at <b>{Efficiency}</b> efficiency.");
		}

		public class NoHeatGradientStatusItem
		{
			public const string ID = "STIRLING_NO_HEAT_GRADIENT";
			public static readonly LocString Name = StringUtils.StatusItemName(ID, "BUILDING", "Temperature gradient is less than <b>{Min_Temperature_Gradient}</b>.");
			public static readonly LocString Tooltip = StringUtils.StatusItemTooltip(ID, "BUILDING", "This engine requires a " + UI.PRE_KEYWORD + "Temperature" + UI.PST_KEYWORD + " difference of at least <b>{Min_Temperature_Gradient}</b> to generate power.");
		}

		public class TooHotStatusItem
		{
			public const string ID = "STIRLING_TOO_HOT";
			public static readonly LocString Name = StringUtils.StatusItemName(ID, "BUILDING", "Engine Too Hot");
			public static readonly LocString Tooltip = StringUtils.StatusItemTooltip(ID, "BUILDING", "This engine must be below <b>{Overheat_Temperature}</b> to properly function.");
		}

		public class ActiveWattageStatusItem
		{
			public const string ID = "STIRLING_ACTIVE_WATTAGE";
			public static readonly LocString Name = StringUtils.StatusItemName(ID, "BUILDING", "Current Wattage: {Wattage}");
			public static readonly LocString Tooltip = StringUtils.StatusItemTooltip(ID, "BUILDING", "This stirling engine is generating " + UI.FormatAsPositiveRate("{Wattage}") + "\n\nIt is running at <b>{Efficiency}</b> of full capacity. Increase the " + UI.PRE_KEYWORD + "Temperature" + UI.PST_KEYWORD + " gradient to improve output.");
		}

		public class HeatPumpedStatusItem
		{
			public const string ID = "STIRLING_HEAT_PUMPED";
			public static readonly LocString Name = StringUtils.StatusItemName(ID, "BUILDING", "Heat Input: {HeatPumped}");
			public static readonly LocString Tooltip = StringUtils.StatusItemTooltip(ID, "BUILDING", "This stiling engine is moving " + UI.FormatAsPositiveRate("{HeatPumped}"));
		}

		internal static float HeatToWatts(float dtu)
		{
			return dtu / ModSettings.Instance.DTUPerWatt;
		}

		internal static float WattsToHeat(float w)
		{
			return w * ModSettings.Instance.DTUPerWatt;
		}

		public class States : GameStateMachine<States, Instance, StirlingEngine>
		{
			public class OperationalStates : State
			{
				public State idle;

				public State active;

				public State noGradient;

				public State tooHot;
			}

			public State inoperational;

			public OperationalStates operational;

			private static readonly HashedString[] ACTIVE_ANIMS = new HashedString[2]
			{
				"working_pre",
				"working_loop"
			};

			private static readonly HashedString[] NOTWORKING_ANIMS = new HashedString[1]
			{
				"off"
			};

			public override void InitializeStates(out BaseState default_state)
			{
				InitializeStatusItems();
				default_state = operational;
				inoperational.EventTransition(GameHashes.OperationalChanged, operational.active, (StirlingEngine.Instance smi) => smi.master.GetComponent<Operational>().IsOperational);
				inoperational.QueueAnim("off");

				operational.DefaultState(operational.active);
				operational.EventTransition(GameHashes.OperationalChanged, inoperational, (StirlingEngine.Instance smi) => !smi.master.GetComponent<Operational>().IsOperational);
				operational.Update("UpdateOperational", delegate (StirlingEngine.Instance smi, float dt)
				{
					smi.UpdateState(dt);
				});
				operational.Exit(delegate (StirlingEngine.Instance smi)
				{
					smi.DisableStatusItems();
				});

				operational.idle.QueueAnim("on");
				operational.active.ToggleStatusItem((StirlingEngine.Instance smi) => activeStatusItem, (StirlingEngine.Instance smi) => smi.master);
				operational.active.Enter(delegate (StirlingEngine.Instance smi)
				{
					smi.GetComponent<KAnimControllerBase>().Play(ACTIVE_ANIMS, KAnim.PlayMode.Loop);
					smi.GetComponent<Operational>().SetActive(value: true);
				});
				operational.active.Update("UpdateActive", delegate (StirlingEngine.Instance smi, float dt)
				{
					smi.master.PumpHeat(dt);
				});
				operational.active.Exit(delegate (StirlingEngine.Instance smi)
				{
					smi.master.GetComponent<Generator>().ResetJoules();
					smi.GetComponent<Operational>().SetActive(value: false);
					smi.master.currentEfficiency = 0f;
					smi.master.currentGeneratedPower = 0f;
					smi.master.currentGeneratedHeat = 0f;
				});

				operational.noGradient.Enter(delegate (StirlingEngine.Instance smi)
				{
					smi.GetComponent<KAnimControllerBase>().Play(NOTWORKING_ANIMS, KAnim.PlayMode.Loop);
				});

				operational.tooHot.Enter(delegate (StirlingEngine.Instance smi)
				{
					smi.GetComponent<KAnimControllerBase>().Play(NOTWORKING_ANIMS, KAnim.PlayMode.Loop);
				});
			}
		}

		public class Instance : GameStateMachine<States, Instance, StirlingEngine, object>.GameInstance
		{
			public bool buildingTooHot;
			public bool insufficientGradient;

			private Guid buildingTooHotHandle = Guid.Empty;
			private Guid insufficientGradientHandle = Guid.Empty;
			private Guid activeWattageHandle = Guid.Empty;
			private Guid heatProductionHandle = Guid.Empty;
			private Guid heatPumpedHandle = Guid.Empty;

			public Instance(StirlingEngine master) : base(master)
			{ }

			public void UpdateState(float dt)
			{
				IsTooHot(ref buildingTooHot);

				var srcElement = Grid.Element[base.master.heatSourceCell];
				var srcMass = Grid.Mass[base.master.heatSourceCell];
				var srcTemperature = Grid.Mass[base.master.heatSourceCell];

				float hotTempK = Grid.Temperature[base.master.heatSourceCell];
				float coldTempK = coldTempK = GameComps.StructureTemperatures.GetPayload(base.master.structureTemperature).Temperature;
				float tempDiffK = hotTempK - coldTempK;
				if (tempDiffK < 0) tempDiffK = 0;



				// Check if temperature gradient is sufficient
				if (srcMass > 0f)
				{
					float gradientK = (hotTempK - coldTempK);
					insufficientGradient = (gradientK < base.master.minTemperatureDifferenceK);
				}
				else
				{
					insufficientGradient = true;
				}

				// Calculate efficiency
				if (srcMass > 0f && coldTempK > 0 && hotTempK > 0)
				{
					base.master.currentEfficiency = Mathf.Clamp(1f - (coldTempK / hotTempK), 0f, 1f);
				}
				else
				{
					base.master.currentEfficiency = 0f;
				}

				// Calculate heat to move
				if (!buildingTooHot && srcMass > 0f && tempDiffK >= base.master.minTemperatureDifferenceK)
				{
#if USE_THERMAL_CONDUCTIVITY
					float heatMass = srcElement.specificHeatCapacity * srcMass;
					base.master.heatToPumpDTU = base.master.thermalConductivity * srcElement.thermalConductivity * tempDiffK * heatMass * 0.5f;
					//Debug.Log($"StirlingEngineDump: {base.master.thermalConductivity} * {srcElement.thermalConductivity} * {tempDiffK} * {heatMass} * 0.5");
#elif USE_EFFICIENCY_FOR_HEAT
					base.master.heatToPumpDTU = base.master.maxHeatToPumpDTU * base.master.currentEfficiency;
#else
					base.master.heatToPumpDTU = base.master.maxHeatToPumpDTU;
#endif

				}
				else
				{
					base.master.heatToPumpDTU = 0f;
				}

				// Clamp heat to move
				base.master.heatToPumpDTU = Mathf.Clamp(base.master.heatToPumpDTU, 0f, base.master.maxHeatToPumpDTU);

				UpdateStatusItems();
				StateMachine.BaseState currentState = base.smi.GetCurrentState();
				if (buildingTooHot)
				{
					if (currentState != base.sm.operational.tooHot)
					{
						base.smi.GoTo(base.sm.operational.tooHot);
					}
				}
				else if (insufficientGradient)
				{
					if (currentState != base.sm.operational.noGradient)
					{
						base.smi.GoTo(base.sm.operational.noGradient);
					}
				}
				else if (!buildingTooHot && !insufficientGradient)
				{
					if (currentState != base.sm.operational.active)
					{
						base.smi.GoTo(base.sm.operational.active);
					}
				}
				else if (currentState != base.sm.operational.idle)
				{
					base.smi.GoTo(base.sm.operational.idle);
				}
			}

			private bool IsTooHot(ref bool building_too_hot)
			{
				//building_too_hot = (base.gameObject.GetComponent<PrimaryElement>().Temperature > base.smi.master.maxBuildingTemperature);
				buildingTooHot = false;
				return building_too_hot;
			}

			public void UpdateStatusItems()
			{
				KSelectable component = GetComponent<KSelectable>();
				insufficientGradientHandle = UpdateStatusItem(insufficientTemperatureGradientStatusItem, insufficientGradient, insufficientGradientHandle, component);
				buildingTooHotHandle = UpdateStatusItem(buildingTooHotStatusItem, buildingTooHot, buildingTooHotHandle, component);
				heatPumpedHandle = UpdateStatusItem(heatPumpedStatusItem, true, heatPumpedHandle, component);

				StatusItem power_status_item = base.master.operational.IsActive ? activeWattageStatusItem : Db.Get().BuildingStatusItems.GeneratorOffline;
				activeWattageHandle = component.SetStatusItem(Db.Get().StatusItemCategories.Power, power_status_item, base.master);

				StatusItem heat_status_item = base.master.operational.IsActive ? heatProducedStatusItem : Db.Get().BuildingStatusItems.GeneratorOffline;
				heatProductionHandle = component.SetStatusItem(Db.Get().StatusItemCategories.Heat, heat_status_item, base.master);
			}

			private Guid UpdateStatusItem(StatusItem item, bool show, Guid current_handle, KSelectable ksel)
			{
				Guid result = current_handle;
				if (show != (current_handle != Guid.Empty))
				{
					result = ((!show) ? ksel.RemoveStatusItem(current_handle) : ksel.AddStatusItem(item, base.master));
				}
				return result;
			}

			public void DisableStatusItems()
			{
				KSelectable component = GetComponent<KSelectable>();
				component.RemoveStatusItem(buildingTooHotHandle);
				component.RemoveStatusItem(insufficientGradientHandle);
				component.RemoveStatusItem(activeWattageHandle);
			}
		}

		private static StatusItem insufficientTemperatureGradientStatusItem;

		private static StatusItem buildingTooHotStatusItem;

		private static StatusItem activeWattageStatusItem;

		private static StatusItem heatProducedStatusItem;

		private static StatusItem activeStatusItem;

		private static StatusItem heatPumpedStatusItem;

		public static void InitializeStatusItems()
		{
			activeStatusItem = new StatusItem(ActiveStatusItem.ID, "BUILDING", "", StatusItem.IconType.Info, NotificationType.Good, allow_multiples: false, OverlayModes.None.ID);
			insufficientTemperatureGradientStatusItem = new StatusItem(NoHeatGradientStatusItem.ID, "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
			insufficientTemperatureGradientStatusItem.resolveStringCallback = ResolveStrings;
			insufficientTemperatureGradientStatusItem.resolveTooltipCallback = ResolveStrings;
			buildingTooHotStatusItem = new StatusItem(TooHotStatusItem.ID, "BUILDING", "status_item_plant_temperature", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
			buildingTooHotStatusItem.resolveTooltipCallback = ResolveStrings;
			activeWattageStatusItem = new StatusItem(ActiveWattageStatusItem.ID, "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.Power.ID);
			activeWattageStatusItem.resolveStringCallback = ResolveWattageStatus;
			heatProducedStatusItem = new StatusItem("OPERATINGENERGY", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.Temperature.ID);
			heatProducedStatusItem.resolveStringCallback = ResolveHeatStatus;
			heatPumpedStatusItem = new StatusItem(HeatPumpedStatusItem.ID, "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.Temperature.ID);
			heatPumpedStatusItem.resolveStringCallback = ResolveStrings;
		}

		private static string ResolveStrings(string str, object data)
		{
			StirlingEngine engine = (StirlingEngine)data;
			str = str.Replace("{Min_Temperature_Gradient}", GameUtil.GetFormattedTemperature(engine.minTemperatureDifferenceK, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative));
			str = str.Replace("{Overheat_Temperature}", GameUtil.GetFormattedTemperature(engine.maxBuildingTemperature));
			str = str.Replace("{Efficiency}", GameUtil.GetFormattedPercent(engine.currentEfficiency * 100f));
			str = str.Replace("{HeatPumped}", GameUtil.GetFormattedHeatEnergyRate(engine.heatToPumpDTU));
			return str;
		}

		private static string ResolveWattageStatus(string str, object data)
		{
			StirlingEngine engine = (StirlingEngine)data;
			return str.Replace("{Wattage}", GameUtil.GetFormattedWattage(engine.currentGeneratedPower)).Replace("{Efficiency}", GameUtil.GetFormattedPercent(engine.currentEfficiency * 100f));
		}

		private static string ResolveHeatStatus(string str, object data)
		{
			StirlingEngine engine = (StirlingEngine)data;
			return str.Replace("{0}", GameUtil.GetFormattedHeatEnergy(engine.currentGeneratedHeat));
		}

		// Startup
		private Instance smi;
		private MeterController meter;
		private HandleVector<int>.Handle structureTemperature;
		private int heatSourceCell;

		[MyCmpGet]
		private KBatchedAnimController anim;

		// Configurable
		private float maxBuildingTemperature = StirlingEngineConfig.MAX_TEMP;
		private float minTemperatureDifferenceK;
		private float maxHeatToPumpDTU;
		private float wasteHeatRatio;

		// Runtime
		private float lastSampleTime = -1f;
		private float heatToPumpDTU = 0f;
		private float currentEfficiency = 1.0f;
		private float currentGeneratedPower = 0f;
		private float currentGeneratedHeat = 0f;

		private int audioChannel = -1;

		public override void OnSpawn()
		{
			base.OnSpawn();

			// Create efficiency meter
			meter = new MeterController(GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, "meter_target", "meter_fill", "meter_frame", "meter_OL");

			// Determine heat source cell
			int x, y;
			Grid.CellToXY(Grid.PosToCell(this), out x, out y);
			heatSourceCell = Grid.XYToCell(x, y - 2);

			// Get structure temperature handle
			structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);

			// Get configurable attributes
			minTemperatureDifferenceK = ModSettings.Instance.MinimumTemperatureDifference;
			maxHeatToPumpDTU = WattsToHeat(ModSettings.Instance.MaxWattOutput);
			wasteHeatRatio = ModSettings.Instance.WasteHeatRatio;

			anim.PlaySpeedMultiplier = 1.5f;

			// Set up state machine
			smi = new Instance(this);
			smi.StartSM();
		}

		public override void OnCleanUp()
		{
			if (smi != null)
			{
				smi.StopSM("cleanup");
			}
			base.OnCleanUp();
		}

		private void PumpHeat(float dt)
		{
			float workHeat = heatToPumpDTU;

#if PRODUCE_LEFTOVER_HEAT
			currentGeneratedHeat = Mathf.Clamp(heatToPumpDTU - workHeat, 0, heatToPumpDTU);
#elif PRODUCE_CONSTANT_HEAT
			currentGeneratedHeat = 0f;
#elif PRODUCE_PROPORTIONAL_HEAT
			currentGeneratedHeat = workHeat * wasteHeatRatio;
#else
			currentGeneratedHeat = 1f;
#endif

			currentGeneratedPower = HeatToWatts(Mathf.Clamp(workHeat, 0, heatToPumpDTU));

			SimMessages.ModifyEnergy(heatSourceCell, -(heatToPumpDTU * dt / 1000f), 5000f, SimMessages.EnergySourceID.StructureTemperature);
		}

		public override void EnergySim200ms(float dt)
		{
			base.EnergySim200ms(dt);

			ushort circuitID = base.CircuitID;
			operational.SetFlag(Generator.wireConnectedFlag, circuitID != ushort.MaxValue);

			float display_dt = (lastSampleTime > 0f) ? (Time.time - lastSampleTime) : 1f;
			lastSampleTime = Time.time;

			if (operational.IsOperational)
			{
				GenerateJoules(currentGeneratedPower * dt);
				float meterPercent = Mathf.Clamp((currentEfficiency * 0.95f), 0f, 1f);
				meter.SetPositionPercent(meterPercent);
				GameComps.StructureTemperatures.ProduceEnergy(structureTemperature, currentGeneratedHeat * dt / 1000f, "StirlingEngine", display_dt);
			}
			else
			{
				meter.SetPositionPercent(0);
			}
		}
	}
}
