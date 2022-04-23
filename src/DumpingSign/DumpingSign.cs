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

		protected FilteredStorage filteredStorage;

		private GameObject signObject;
		private KBatchedAnimController signAnim;

		//private static readonly KAnimFile genericIconsKanim = Assets.GetAnim("dumping_sign_icons_kanim");

		private static readonly Dictionary<Tag, KAnimFile> categoryKanims = new Dictionary<Tag, KAnimFile>();
		private static readonly Dictionary<Tag, Vector2> iconOffsets = new Dictionary<Tag, Vector2>();
		private static readonly Dictionary<Tag, Vector2> iconScales = new Dictionary<Tag, Vector2>();

		private static bool init = false;

		private static void Init()
		{
			if (init) return;

			categoryKanims[GameTags.Agriculture] = Assets.GetAnim("farmtilerotating_kanim");
			categoryKanims[GameTags.BuildableProcessed] = Assets.GetAnim("rockrefinery_kanim");
			categoryKanims[GameTags.Clothes] = Assets.GetAnim("clothes_kanim");
			categoryKanims[GameTags.Compostable] = Assets.GetAnim("compost_kanim");
			categoryKanims[GameTags.Egg] = Assets.GetAnim("incubator_kanim");
			categoryKanims[GameTags.Farmable] = Assets.GetAnim("farmtilerotating_kanim");
			categoryKanims[GameTags.Filter] = Assets.GetAnim("waterpurifier_kanim");
			categoryKanims[GameTags.IndustrialProduct] = Assets.GetAnim("metalrefinery_kanim");
			categoryKanims[GameTags.Liquifiable] = Assets.GetAnim("liquid_tank_kanim");
			categoryKanims[GameTags.ManufacturedMaterial] = Assets.GetAnim("supermaterial_refinery_kanim");
			categoryKanims[GameTags.Metal] = Assets.GetAnim("metalrefinery_kanim");
			categoryKanims[GameTags.RefinedMetal] = Assets.GetAnim("metalrefinery_kanim");
			categoryKanims[GameTags.RareMaterials] = Assets.GetAnim("supermaterial_refinery_kanim");
			categoryKanims[GameTags.Seed] = Assets.GetAnim("farmtilerotating_kanim");

			init = true;
		}

		protected override void OnPrefabInit()
		{
			Init();

			ChoreType choreType = Db.Get().ChoreTypes.Get(Db.Get().ChoreTypes.StorageFetch.Id);

			filteredStorage = new FilteredStorage(this, null, null, null, false, choreType);
			filteredStorage.SetHasMeter(false);
		}

		protected override void OnSpawn()
		{
			anim.SetSymbolVisiblity("item_target", false);

			Vector3 pos = this.transform.position;

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

			filterable.OnFilterChanged = (Action<Tag[]>)Delegate.Combine(filterable.OnFilterChanged, new Action<Tag[]>(this.OnFilterChanged));
			storage.SetOnlyFetchMarkedItems(true);
			Subscribe((int)GameHashes.OnStorageChange, OnStorageChanged);

			Refresh();
		}

		protected override void OnCleanUp()
		{
			filterable.OnFilterChanged = (Action<Tag[]>)Delegate.Remove(filterable.OnFilterChanged, new Action<Tag[]>(this.OnFilterChanged));
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

		private void OnFilterChanged(Tag[] tags)
		{
			try
			{
				if (tags == null || tags.Length == 0)
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

					if (tags.Length == 1)
					{
						var prefab = Assets.GetPrefab(tags[0]);
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
						Tag category = GetBiggestCategory(tags);

						if (categoryKanims.TryGetValue(category, out KAnimFile kanim))
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

					if (animFiles == null)
					{
						animFiles = new KAnimFile[] { Assets.GetAnim("box_kanim") };
						animToPlay = "ui";
						offset = new Vector2(0, -0.2f);
						scale *= 1.5f;
					}

					if (animFiles != null && animFiles.Length > 0 && animFiles[0] != null)
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
				Debug.LogWarning("DumpingSign: Caught error in OnFilterChanged.");
				Debug.LogException(ex);
			}
		}

		private Tag GetBiggestCategory(Tag[] tags)
		{
			bool tied = false;
			Tag biggestCategory = Tag.Invalid;
			int biggestCategoryCount = 0;

			foreach (Tag category in storage.storageFilters)
			{
				int count = GetNumTagsFromCategory(category, tags);

				if (count > biggestCategoryCount)
				{
					biggestCategory = category;
					biggestCategoryCount = count;
					tied = false;
				}
				else if (count == biggestCategoryCount)
				{
					tied = true;
				}
			}

			if (tied)
				return Tag.Invalid;
			else
				return biggestCategory;
		}

		private int GetNumTagsFromCategory(Tag category, Tag[] tags)
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
	}
}
