using System;
using System.Collections.Generic;
using System.Linq;

using RomenH.Common;

using UnityEngine;

namespace RomenH.DumpingSign
{
	public class DumpingSign : KMonoBehaviour
	{
		[MyCmpReq]
		private Storage storage;

		[MyCmpGet]
		private KBatchedAnimController anim;

		[MyCmpGet]
		private TreeFilterable filterable;

		private int cell;

		protected FilteredStorage filteredStorage;

		private GameObject signObject;
		private KBatchedAnimController signAnim;

		public override void OnPrefabInit()
		{
			ChoreType choreType = Db.Get().ChoreTypes.Get(Db.Get().ChoreTypes.StorageFetch.Id);

			filteredStorage = new FilteredStorage(this, null, null, false, choreType);
			filteredStorage.SetHasMeter(false);
		}

		public override void OnSpawn()
		{
			anim.SetSymbolVisiblity("item_target", false);

			Vector3 pos = this.transform.position;

			cell = Grid.PosToCell(pos);

			signObject = new GameObject();
			signObject.transform.parent = this.gameObject.transform;
			signObject.transform.position = new Vector3(pos.x, pos.y + 0.66f, pos.z - 0.1f);
			signObject.SetActive(false);
			signAnim = signObject.AddComponent<KBatchedAnimController>();
			signAnim.AnimFiles = new KAnimFile[] { Assets.GetAnim("egg_hatch_fixed_kanim") };
			signAnim.initialAnim = "ui";
			signAnim.sceneLayer = Grid.SceneLayer.BuildingFront;
			signAnim.animScale *= 0.33f;
			signObject.SetActive(true);
			signAnim.enabled = false;

			filterable.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Combine(filterable.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
			storage.SetOnlyFetchMarkedItems(true);
			Subscribe((int)GameHashes.OnStorageChange, OnStorageChanged);

			Refresh();
		}

		public override void OnCleanUp()
		{
			filterable.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Remove(filterable.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
			filteredStorage.CleanUp();

			GameObject.DestroyImmediate(signObject);
		}

		private void OnStorageChanged(object data)
		{
			storage.DropAll(vent_gas: true, dump_liquid: true);
		}

		private void Refresh()
		{
			OnFilterChanged(filterable.GetTags());
		}

		private void OnFilterChanged(HashSet<Tag> tags)
		{
			try
			{
				if (tags == null || tags.Count == 0)
				{
					anim.SetSymbolVisiblity("disabled", true);
					signAnim.enabled = false;
				}
				else
				{
					anim.SetSymbolVisiblity("disabled", false);

					KAnimFile[] animFiles = null;
					string animToPlay = null;
					Vector2 offset = Vector2.zero;
					float scale = 0.0015f;

					if (tags.Count == 1)
					{
						var prefab = Assets.GetPrefab(tags.First());
						if (prefab != null)
						{
							animFiles = prefab.GetComponent<KBatchedAnimController>()?.AnimFiles;
							animToPlay = "ui";
							var id = prefab.GetComponent<KPrefabID>();
							if (id.HasTag(GameTags.Seed))
							{
								scale *= 1.5f;
							}
							else if (id.HasTag(GameTags.Egg))
							{
								scale *= 1.5f;
								offset = new Vector2(0, -0.2f);
							}
						}
					}
					else
					{
						int discoveredCount = DiscoveredResources.Instance.GetDiscovered().Count;

						if (tags.Count != discoveredCount)
						{
							Tag category = GetMajorityCategory(tags);

							if (SignCategories.CategoryIcons.TryGetValue(category, out KAnimFile kanim))
							{
								if (kanim != null)
								{
									animFiles = new KAnimFile[] { kanim };
									animToPlay = "ui";
									offset = new Vector2(0, -0.2f);
									scale *= 1.25f;
								}
							}
						}
					}

					if (animFiles == null)
					{
						animFiles = new KAnimFile[] { Assets.GetAnim("box_kanim") };
						animToPlay = "ui";
						offset = new Vector2(0, -0.2f);
						scale *= 1.5f;
					}

					if (animFiles.Length > 0 && animFiles[0] != null)
					{
						signAnim.SwapAnims(animFiles);
						signAnim.Play(animToPlay);
						signAnim.Offset = offset;
						signAnim.animScale = scale;
					}
					else
					{
						Debug.LogWarning("DumpingSign: Failed to find a valid animation for the selected tags. Icon did not update.");
					}

					signAnim.enabled = true;
				}
			}
			catch (Exception ex)
			{
				ModCommon.Log.Error("Caught error in OnFilterChanged", ex);
			}
		}

		private Tag GetMajorityCategory(HashSet<Tag> tags)
		{
			Tag majorityCategory = Tag.Invalid;

			foreach (Tag category in storage.storageFilters)
			{
				int count = GetNumTagsFromCategory(category, tags);

				bool majority = count > (tags.Count - count);

				if (majority)
				{
					majorityCategory = category;
					break;
				}
			}

			return majorityCategory;
		}

		private int GetNumTagsFromCategory(Tag category, HashSet<Tag> tags)
		{
			int c = 0;
			foreach (Tag tag in DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(category))
			{
				if (tags.Contains(tag))
				{
					c++;
				}
			}

			return c;
		}

		private void CancelSweepChoresBelowSign()
		{
			
		}
	}
}
