using KSerialization;
using UnityEngine;

namespace RomenH.PipedDeodorizer
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class PipedDeodorizer : StateMachineComponent<PipedDeodorizer.StatesInstance>
#if ENABLE_TINT
	, ISim1000ms
#endif
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
				off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, on, (StatesInstance smi) => smi.master.operational.IsOperational);
				on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, off, (StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(on.waiting);
				on.waiting.EventTransition(GameHashes.OnStorageChange, on.working_pre, (StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting());
				on.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(on.working);
				on.working.Enter(delegate (StatesInstance smi)
				{
					smi.master.operational.SetActive(value: true);
				}).QueueAnim("working_loop", loop: true).EventTransition(GameHashes.OnStorageChange, on.working_pst, (StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll())
					.Exit(delegate (StatesInstance smi)
					{
						smi.master.operational.SetActive(value: false);
					});
				on.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(on.waiting);
			}
		}

		[MyCmpGet]
		private Operational operational;

#if ENABLE_TINT
		private Color tintColor = new Color32(255, 251, 0, 255);
#endif

		protected override void OnSpawn()
		{
			base.OnSpawn();

#if ENABLE_TINT
			gameObject.GetComponent<KAnimControllerBase>().TintColour = tintColor;
#endif

			base.smi.StartSM();
		}

#if ENABLE_TINT
		public void Sim1000ms(float dt)
		{
			var anim = gameObject.GetComponent<KAnimControllerBase>();
			if (anim != null && anim.TintColour != tintColor)
			{
				anim.TintColour = tintColor;
			}
		}
#endif
	}
}
