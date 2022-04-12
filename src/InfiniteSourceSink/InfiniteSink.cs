using KSerialization;

using UnityEngine;

namespace InfiniteSourceSink
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class InfiniteSink : KMonoBehaviour
	{
		[SerializeField]
		public ConduitType Type;

		[MyCmpGet]
		public KBatchedAnimController anim;

		private int inputCell;
		private Operational.Flag incomingFlag = new Operational.Flag("incoming", Operational.Flag.Type.Requirement);

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			anim.SetSymbolVisiblity("effect", false);
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();

			var building = GetComponent<Building>();
			inputCell = building.GetUtilityInputCell();

			Conduit.GetFlowManager(Type).AddConduitUpdater(ConduitUpdate);
		}

		protected override void OnCleanUp()
		{
			Conduit.GetFlowManager(Type).RemoveConduitUpdater(ConduitUpdate);
			base.OnCleanUp();
		}

		private void ConduitUpdate(float dt)
		{
			var flowManager = Conduit.GetFlowManager(Type);
			if (flowManager == null || !flowManager.HasConduit(inputCell))
			{
				GetComponent<Operational>().SetFlag(incomingFlag, false);
				return;
			}

			var contents = flowManager.GetContents(inputCell);
			GetComponent<Operational>().SetFlag(incomingFlag, contents.mass > 0.0f);
			if (GetComponent<Operational>().IsOperational)
			{
				flowManager.RemoveElement(inputCell, contents.mass);
			}
		}
	}
}
