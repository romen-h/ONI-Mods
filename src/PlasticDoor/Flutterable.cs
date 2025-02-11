using System.Collections.Generic;
using System.Linq;

using KSerialization;

namespace RomenH.PlasticDoor
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Flutterable : StateMachineComponent<Flutterable.StatesInstance>
	{
		private HandleVector<int>.Handle pickupablesChangedEntry;
		//[MyCmpReq] Curtain curtain;

		[MyCmpReq]
		public Curtain curtain;

		[MyCmpReq]
		public Building building;

		public Curtain.Controller.Instance controller;

		private Extents pickupableExtents;
		public bool passingLeft;

		[Serialize]
		public bool Listening = false;

		private Dictionary<string, string> soundEvents = new Dictionary<string, string>
		{
			{ "swoosh", GlobalAssets.GetSound("drecko_ruffle_scales_short") },
			{ "open", GlobalAssets.GetSound("sauna_door_open") },
			{ "close", GlobalAssets.GetSound("sauna_door_close") },
			{ "lock", GlobalAssets.GetSound("dupe_scratch_single") },
			{ "unlock", GlobalAssets.GetSound("jobstation_job_grab") }
		};

		public override void OnSpawn()
		{
			base.OnSpawn();

			smi.StartSM();
			StartPartitioner();
		}

		public void Flutter()
		{
			Listening = false;
			controller.GoTo(controller.sm.passing);
			PlaySoundEffect("swoosh", 4);
		}

		public void PlaySoundEffect(string sound, float vol)
		{
			if (soundEvents == null) return;
			SoundEvent.EndOneShot(
				SoundEvent.BeginOneShot(
					soundEvents[sound],
					transform.position,
					vol,
					SoundEvent.ObjectIsSelectedAndVisible(gameObject)));
		}

		private void StartPartitioner()
		{
			pickupableExtents = Extents.OneCell(building.PlacementCells[0]);
			pickupablesChangedEntry = GameScenePartitioner.Instance.Add("Curtain.PickupablesChanged", gameObject, pickupableExtents, GameScenePartitioner.Instance.pickupablesChangedLayer, OnPickupablesChanged);
		}

		private void OnPickupablesChanged(object obj)
		{
			if (!Listening) return;

			Pickupable p = obj as Pickupable;
			if (p && IsDupe(p) && Waiting)
				UpdateMovement(p);
		}

		private void UpdateMovement(Pickupable dupe)
		{
			var navigator = dupe.GetComponent<Navigator>();
			if (navigator != null && navigator.TryGetNextTransition(out NavGrid.Transition transition))
			{
				passingLeft = transition.x > 0;
				Trigger((int)GameHashes.WalkBy, this);
			}
			else
			{
				smi.GoTo(smi.sm.idlingInside);
			}
		}

		public bool IsDupeStandingHere()
		{
			var pooledList = GatherEntries();
			foreach (var entry in pooledList.Select(e => e.obj as Pickupable))
			{
				if (IsDupe(entry))
					return true;
			}

			return false;
		}

		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			if (smi.IsRunning())
			{
				smi.StopSM("cleanup");
			}
			GameScenePartitioner.Instance.Free(ref pickupablesChangedEntry);
		}

		public void SetInactive()
		{
			smi.GoTo(smi.sm.inactive);
		}

		private bool IsDupe(Pickupable pickupable) => pickupable.KPrefabID.HasTag(GameTags.DupeBrain);
		private bool Waiting => smi.IsInsideState(smi.sm.waiting);

		private ListPool<ScenePartitionerEntry, Door>.PooledList GatherEntries()
		{
			var pooledList = ListPool<ScenePartitionerEntry, Door>.Allocate();
			GameScenePartitioner.Instance.GatherEntries(pickupableExtents, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
			return pooledList;
		}

		public class States : GameStateMachine<States, StatesInstance, Flutterable>
		{
			public State passing;
			public State idlingInside;
			public State waiting;
			public State inactive;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = waiting;

				waiting
					.EventTransition(GameHashes.WalkBy, passing);
				passing
					.Enter(smi => smi.master.Flutter())
					.ScheduleGoTo(.5f, idlingInside);
				idlingInside
					.Transition(waiting, Not(smi => smi.master.IsDupeStandingHere()), UpdateRate.RENDER_200ms)
					.Exit(smi => smi.master.curtain.OnPassedBy());
			}
		}

		public class StatesInstance : GameStateMachine<States, StatesInstance, Flutterable, object>.GameInstance
		{
			public StatesInstance(Flutterable smi) : base(smi) { }
		}
	}
}
