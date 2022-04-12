using System;

using KSerialization;

using STRINGS;

using UnityEngine;

namespace InfiniteSourceSink
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class InfiniteSource : KMonoBehaviour
	{
		private static StatusItem filterStatusItem = null;

		[SerializeField]
		public ConduitType conduitType = ConduitType.None;

		[SerializeField]
		[Serialize]
		public float Flow
		{ get; set; } = 10000f;

		[SerializeField]
		[Serialize]
		public float Temp
		{ get; set; } = 300f;

		[MyCmpGet]
		public KSelectable selectable;

		[MyCmpGet]
		public Building building;

		[MyCmpGet]
		public Operational operational;

		[MyCmpGet]
		public KBatchedAnimController anim;

		[MyCmpGet]
		public Filterable filterable;

		private int outputCell = -1;
		private SimHashes filteredElement = SimHashes.Vacuum;
		private float minTemp;
		private float maxTemp;
		private Operational.Flag filterFlag = new Operational.Flag("filter", Operational.Flag.Type.Requirement);

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			InitializeStatusItems();
		}

		private void InitializeStatusItems()
		{
			if (filterStatusItem != null) return;

			filterStatusItem = new StatusItem("Filter", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.LiquidConduits.ID, true, 129022);
			filterStatusItem.resolveStringCallback = (str, data) =>
			{
				InfiniteSource infiniteSource = (InfiniteSource)data;
				if (infiniteSource.filteredElement == SimHashes.Void)
				{
					str = string.Format(BUILDINGS.PREFABS.GASFILTER.STATUS_ITEM, BUILDINGS.PREFABS.GASFILTER.ELEMENT_NOT_SPECIFIED);
				}
				else
				{
					Element elementByHash = ElementLoader.FindElementByHash(infiniteSource.filteredElement);
					str = string.Format(BUILDINGS.PREFABS.GASFILTER.STATUS_ITEM, elementByHash.name);
				}
				return str;
			};
			filterStatusItem.conditionalOverlayCallback = new Func<HashedString, object, bool>(ShowInUtilityOverlay);
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();

			outputCell = building.GetUtilityOutputCell();

			Conduit.GetFlowManager(conduitType).AddConduitUpdater(ConduitUpdate);

			filterable.onFilterChanged += new Action<Tag>(OnFilterChanged);
			OnFilterChanged(filterable.SelectedTag);

			selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, filterStatusItem, this);
		}

		protected override void OnCleanUp()
		{
			Conduit.GetFlowManager(conduitType).RemoveConduitUpdater(ConduitUpdate);
			base.OnCleanUp();
		}

		private bool refreshing = false;

		private void OnFilterChanged(Tag tag)
		{
			if (refreshing) return;

			refreshing = true;

			Element element = ElementLoader.GetElement(tag);
			if (element != null)
			{
				filteredElement = element.id;
				anim.SetSymbolTint("gasframes", element.substance.uiColour);
				anim.SetSymbolTint("liquid", element.substance.uiColour);
				anim.SetSymbolTint("liquid_top", element.substance.uiColour);

				minTemp = element.lowTemp;
				maxTemp = element.highTemp;

				Temp = Mathf.Clamp(Temp, element.lowTemp, element.highTemp);
			}

			bool invalidElement = (!tag.IsValid || tag == GameTags.Void);
			selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NoFilterElementSelected, invalidElement, null);
			operational.SetFlag(filterFlag, !invalidElement);

			RefreshSideScreen();

			refreshing = false;
		}

		private void RefreshSideScreen()
		{
			if (refreshing)

			if (base.GetComponent<KSelectable>().IsSelected)
			{
				refreshing = true;
				DetailsScreen.Instance.Refresh(base.gameObject);
			}
		}

		private bool ShowInUtilityOverlay(HashedString mode, object data)
		{
			bool flag = false;
			switch (conduitType)
			{
				case ConduitType.Gas:
					flag = mode == OverlayModes.GasConduits.ID;
					break;
				case ConduitType.Liquid:
					flag = mode == OverlayModes.LiquidConduits.ID;
					break;
			}
			return flag;
		}

		private void ConduitUpdate(float dt)
		{
			if (operational.IsOperational)
			{
				if (filteredElement == SimHashes.Void || filteredElement == SimHashes.Vacuum) return;

				var flowManager = Conduit.GetFlowManager(conduitType);
				if (flowManager == null) return;
				if (flowManager.HasConduit(outputCell))
				{
					flowManager.AddElement(outputCell, filteredElement, Flow / 1000, Temp, 0, 0);
				}
			}
		}
	}
}
