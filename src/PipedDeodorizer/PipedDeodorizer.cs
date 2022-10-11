using KSerialization;

namespace RomenH.PipedDeodorizer
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class PipedDeodorizer : StateMachineComponent<PipedDeodorizer.StatesInstance>
	{
		public class StatesInstance : GameStateMachine<States, StatesInstance, PipedDeodorizer, object>.GameInstance
		{
			public StatesInstance(PipedDeodorizer smi) : base(smi)
			{ }
		}

		public class States : GameStateMachine<States, StatesInstance, PipedDeodorizer>
		{
			public class OnStates : State
			{
				public State waiting;

				public State working_pre;

				public State working;

				public State working_pst;
			}

			public State off;

			public OnStates on;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = off;

				off.PlayAnim("off");
				off.EventTransition(GameHashes.OperationalChanged, on, (StatesInstance smi) => smi.master.operational.IsOperational);

				on.PlayAnim("on");
				on.EventTransition(GameHashes.OperationalChanged, off, (StatesInstance smi) => !smi.master.operational.IsOperational);
				on.DefaultState(on.waiting);

				on.waiting.EventTransition(GameHashes.OnStorageChange, on.working_pre, (StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting());

				on.working_pre.PlayAnim("working_pre");
				on.working_pre.OnAnimQueueComplete(on.working);

				on.working.Enter(delegate (StatesInstance smi)
				{
					smi.master.operational.SetActive(value: true);
				});
				on.working.QueueAnim("working_loop", loop: true);
				on.working.EventTransition(GameHashes.OnStorageChange, on.working_pst, (StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll());
				on.working.Exit(delegate (StatesInstance smi)
				{
					smi.master.operational.SetActive(value: false);
				});

				on.working_pst.PlayAnim("working_pst");
				on.working_pst.OnAnimQueueComplete(on.waiting);
			}
		}

		[MyCmpGet]
		private Operational operational;

		protected override void OnSpawn()
		{
			base.OnSpawn();
			base.smi.StartSM();
		}
	}
}
