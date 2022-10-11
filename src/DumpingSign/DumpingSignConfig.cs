
using RomenH.Common;

using UnityEngine;

namespace RomenH.DumpingSign
{
	public class DumpingSignConfig : IBuildingConfig
	{
		public const string ID = "RomenH_DumpingSign";

		public static readonly LocString Name = StringUtils.BuildingName(ID, "Dumping Sign");

		public static readonly LocString Desc = StringUtils.BuildingDesc(ID, "");

		public static readonly LocString Effect = StringUtils.BuildingEffect(ID, "Allows Duplicants to deliver materials to this sign which drops them on the ground immediately.");

		public static readonly Tag CopyGroupTag = TagManager.Create(ID);

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 1,
				anim: "dumping_sign_kanim",
				hitpoints: 20,
				construction_time: 5f,
				construction_mass: TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0,
				construction_materials: TUNING.MATERIALS.RAW_MINERALS,
				melting_point: 1600f,
				build_location_rule: BuildLocationRule.Anywhere,
				decor: TUNING.DECOR.NONE,
				noise: TUNING.NOISE_POLLUTION.NONE
			);
			def.Floodable = false;
			def.Entombable = true;
			def.Overheatable = false;
			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			Prioritizable.AddRef(go);

			Storage storage = go.AddComponent<Storage>();
			storage.showInUI = true;
			storage.allowItemRemoval = false;
			storage.storageFilters = TUNING.STORAGEFILTERS.NOT_EDIBLE_SOLIDS;
			storage.fetchCategory = Storage.FetchCategory.StorageSweepOnly;
			storage.showCapacityStatusItem = false;
			storage.showCapacityAsMainStatus = false;
			storage.capacityKg = 10000;

			go.AddOrGet<TreeFilterable>();

			go.AddOrGet<CopyBuildingSettings>().copyGroupTag = CopyGroupTag;

			go.AddOrGet<UserNameable>();

			go.AddOrGet<DumpingSign>();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			//SymbolOverrideControllerUtil.AddToPrefab(go);
		}
	}
}
