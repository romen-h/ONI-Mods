using KSerialization;

using UnityEngine;

namespace RomenH.PlasticDoor
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public partial class Curtain : Workable, ISaveLoadable
	{
		public enum ControlState
		{
			Auto,
			Open,
			Locked
		}

		[MyCmpReq]
		public Building building;

		[MyCmpReq]
		KSelectable kSelectable;

		[MyCmpGet]
		public Flutterable flutterable;

		[Serialize]
		public ControlState CurrentState { get; set; }
		[Serialize]
		public ControlState RequestedState { get; set; }

		private WorkChore<Curtain> changeStateChore;

		[SerializeField]
		public Door.DoorType doorType;

		public bool AllowLiquidThrough = true;

		private readonly KAnimFile[] anims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_remote_kanim")
		};

		public Curtain()
		{
			SetOffsetTable(OffsetGroups.InvertedStandardTable);
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			overrideAnims = anims;
			synchronizeAnims = false;

			Subscribe((int)GameHashes.CopySettings, OnCopySettings);
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			flutterable.Listening = false;

			controller = new Controller.Instance(this);
			controller.StartSM();

			flutterable.controller = controller;

			// Change heat exchange extents
			HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(gameObject);
			StructureTemperaturePayload data = GameComps.StructureTemperatures.GetPayload(handle);
			data.OverrideExtents(new Extents((int)transform.position.x - 1, (int)transform.position.y, 3, 2));
			GameComps.StructureTemperatures.SetPayload(handle, ref data);

			SetDefaultCellFlags();
			UpdateState();
			RequestedState = CurrentState;
			ApplyRequestedControlState();
		}

		public void Open(bool updateControlState = true)
		{
			foreach (int cell in building.PlacementCells)
				Dig(cell);

			if (updateControlState)
			{
				CurrentState = ControlState.Open;
				UpdateController();
			}

		}

		public void Close()
		{
			CurrentState = ControlState.Auto;
			DisplaceElement();
			UpdateController();
		}

		public void Lock()
		{
			CurrentState = ControlState.Locked;
			DisplaceElement();
			UpdateController();
		}

		public void OnPassedBy()
		{
			Close();
			controller.GoTo(controller.sm.closed);
		}

		public override void OnCleanUp()
		{
			controller.GoTo(controller.sm.passingWaiting);
			controller.StopSM("");

			foreach (int cell in building.PlacementCells)
			{
				//SetCellPassable(cell, true, false);
				CleanSim(cell);
			}

			changeStateChore = null;
			base.OnCleanUp();
		}

		public override void OnCompleteWork(WorkerBase worker)
		{
			base.OnCompleteWork(worker);
			changeStateChore = null;
			ApplyRequestedControlState();
		}

		private void ApplyRequestedControlState()
		{
			if (changeStateChore != null)
			{
				changeStateChore.Cancel("");
			}

			changeStateChore = null;
			CurrentState = RequestedState;
			UpdateState();
			kSelectable.RemoveStatusItem(ModAssets.CurtainStatus);
			Trigger((int)GameHashes.DoorStateChanged, this);
		}

		public void QueueStateChange(ControlState state)
		{
			RequestedState = state;
			if (state == CurrentState)
			{
				if (changeStateChore != null)
				{
					changeStateChore.Cancel("");
					changeStateChore = null;
				}

				kSelectable.RemoveStatusItem(ModAssets.CurtainStatus, true);
				return;
			};

			if (DebugHandler.InstantBuildMode)
			{
				if (changeStateChore != null)
				{
					changeStateChore.Cancel("Debug state change");
				}

				ApplyRequestedControlState();
				return;
			}

			kSelectable.AddStatusItem(ModAssets.CurtainStatus, this);

			changeStateChore = new WorkChore<Curtain>(
				chore_type: Db.Get().ChoreTypes.Toggle,
				target: this,
				only_when_operational: false);
		}

		private void OnCopySettings(object obj)
		{
			var curtain = ((GameObject)obj).GetComponent<Curtain>();
			if (curtain != null)
			{
				QueueStateChange(curtain.RequestedState);
				return;
			}
		}

		private void UpdateState()
		{
			switch (CurrentState)
			{
				case ControlState.Open:
					Open();
					break;
				case ControlState.Locked:
					Lock();
					break;
				case ControlState.Auto:
				default:
					Close();
					break;
			}

			UpdatePassable();
		}

		private void SetDefaultCellFlags()
		{
			foreach (int cell in building.PlacementCells)
			{
				//Grid.FakeFloor[cell] = IsTopCell(cell);
				Grid.HasDoor[cell] = true;

				SimMessages.SetCellProperties(cell, (byte)Sim.Cell.Properties.Unbreakable);
			}
		}

		private void UpdatePassable()
		{
			bool passable = CurrentState != ControlState.Locked;
			bool permeable = CurrentState == ControlState.Open;

			foreach (int cell in building.PlacementCells)
				SetCellPassable(cell, passable, permeable);
		}

		private void CleanSim(int cell)
		{
			if (!Grid.IsValidCell(cell)) return;

			Grid.Foundation[cell] = false;
			Grid.HasDoor[cell] = false;
			Game.Instance.SetDupePassableSolid(cell, false, Grid.Solid[cell]);
			Grid.CritterImpassable[cell] = false;
			Grid.DupeImpassable[cell] = false;

			var flags = Sim.Cell.Properties.GasImpermeable | Sim.Cell.Properties.Unbreakable | Sim.Cell.Properties.Transparent;
			if (!AllowLiquidThrough)
			{
				flags |= Sim.Cell.Properties.LiquidImpermeable;
			}
			SimMessages.ClearCellProperties(cell, (byte)flags);
			SimMessages.ReplaceAndDisplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.DoorOpen, 0);

			Pathfinding.Instance.AddDirtyNavGridCell(cell);
		}

		private void DisplaceElement()
		{
			//var pe = GetComponent<PrimaryElement>();
			//float mass = pe.Mass / 2;

			foreach (int cell in building.PlacementCells)
			{
				//var item = new Game.CallbackInfo(() => meltCheck = true);
				//var cb = Game.Instance.callbackManager.Add(item);
				SimMessages.ReplaceAndDisplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.DoorClose, 0f);
				var flags = Sim.Cell.Properties.GasImpermeable | Sim.Cell.Properties.Transparent;
				if (!AllowLiquidThrough)
				{
					flags |= Sim.Cell.Properties.LiquidImpermeable;
				}
				SimMessages.SetCellProperties(cell, (byte)flags);
			}
		}

		private void Dig(int cell)
		{
			var flags = Sim.Cell.Properties.GasImpermeable;
			if (!AllowLiquidThrough)
			{
				flags |= Sim.Cell.Properties.LiquidImpermeable;
			}
			SimMessages.ClearCellProperties(cell, (byte)flags);
			//SimMessages.Dig(cell, cb.index, true);
		}

		private static void SetCellPassable(int cell, bool passable, bool permeable)
		{
			Game.Instance.SetDupePassableSolid(cell, passable, !permeable);
			Grid.DupeImpassable[cell] = !passable;
			Grid.CritterImpassable[cell] = !passable;
			Pathfinding.Instance.AddDirtyNavGridCell(cell);
		}

		private void UpdateController()
		{
			controller.sm.isOpen.Set(CurrentState == ControlState.Open, controller);
			controller.sm.isClosed.Set(CurrentState == ControlState.Auto, controller);
			controller.sm.isLocked.Set(CurrentState == ControlState.Locked, controller);
		}

		private bool IsTopCell(int cell)
		{
			return !(building.GetExtents().Contains(Grid.CellToXY(Grid.CellAbove(cell))));
		}
	}
}
