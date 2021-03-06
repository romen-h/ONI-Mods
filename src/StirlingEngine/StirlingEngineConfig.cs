﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TUNING;
using UnityEngine;

namespace RomenMods.StirlingEngineMod
{
	public class StirlingEngineConfig : IBuildingConfig
	{
		internal const float MAX_TEMP = TUNING.BUILDINGS.OVERHEAT_TEMPERATURES.HIGH_3;

		public const string ID = "StirlingEngine";

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 3,
				anim: "stirling_kanim",
				hitpoints: 30,
				construction_time: 60f,
				construction_mass: new float[] {
					BUILDINGS.CONSTRUCTION_MASS_KG.TIER4[0]
				},
				construction_materials: TUNING.MATERIALS.ALL_METALS,
				melting_point: 1600f,
				build_location_rule: BuildLocationRule.OnFloor,
				decor: DECOR.PENALTY.TIER3,
				noise: NOISE_POLLUTION.NOISY.TIER3
			);
			def.AudioCategory = TUNING.AUDIO.HOLLOW_METAL;
			def.AudioSize = "large";
			def.Floodable = false;
			def.Entombable = true;
			def.GeneratorWattageRating = Mod.Config.MaxWattOutput;
			def.GeneratorBaseCapacity = def.GeneratorWattageRating;
			def.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
			def.Overheatable = true;
			def.OverheatTemperature = MAX_TEMP;
			def.PermittedRotations = PermittedRotations.Unrotatable;
			def.PowerOutputOffset = new CellOffset(0, 0);
			def.ViewMode = OverlayModes.Power.ID;
			def.SelfHeatKilowattsWhenActive = 0f;
			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
			Prioritizable.AddRef(go);

#if ENABLE_GAS_DELIVERY
			Storage storage = go.AddOrGet<Storage>();
			storage.capacityKg = 10f;
			ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
			manualDeliveryKG.SetStorage(storage);
			manualDeliveryKG.requestedItemTag = GameTags.Hydrogen;
			manualDeliveryKG.capacity = 10f;
			manualDeliveryKG.refillMass = 0f;
			manualDeliveryKG.minimumMass = 10f;
			manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
#endif
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			// Extents are not modified
#if false
			go.GetComponent<KPrefabID>().prefabSpawnFn += delegate (GameObject game_object)
			{
				HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(game_object);
				StructureTemperaturePayload new_data = GameComps.StructureTemperatures.GetPayload(handle);
				Extents extents = game_object.GetComponent<Building>().GetExtents();
				Extents newExtents = new Extents(extents.x, extents.y, extents.width, extents.height);
				new_data.OverrideExtents(newExtents);
				GameComps.StructureTemperatures.SetPayload(handle, ref new_data);
			};
#endif

			go.AddOrGet<LogicOperationalController>();
			var engine = go.AddOrGet<StirlingEngine>();
			var tinkerable = Tinkerable.MakePowerTinkerable(go);
			tinkerable.SetWorkTime(120f);
		}
	}
}
