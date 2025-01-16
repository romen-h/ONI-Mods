using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RomenH.Common;

using STRINGS;

using UnityEngine;

namespace RomenH.PlasticUtilities
{
	public class PlasticGasVentConfig : IBuildingConfig
	{
		public const string ID = "RomenH_PlasticGasVent";
		public static readonly LocString Name = StringUtils.BuildingName(ID, "Plastic Gas Vent");
		public static readonly LocString Desc = StringUtils.BuildingDesc(ID, "Vents are an exit point for gases from ventilation systems.");
		public static readonly LocString Effect = StringUtils.BuildingEffect(ID, string.Concat(new string[]
		{
			"Releases ",
			UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
			" from ",
			UI.FormatAsLink("Gas Pipes", "GASPIPING"),
			"."
		}));

		public const float OVERPRESSURE_MASS = 1f;

		private const ConduitType CONDUIT_TYPE = ConduitType.Gas;

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 1,
				anim: "plastic_vent_kanim",
				hitpoints: 30,
				construction_time: 30f,
				construction_mass: TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1,
				construction_materials: TUNING.MATERIALS.PLASTICS,
				melting_point: 800f,
				build_location_rule: BuildLocationRule.Anywhere,
				decor: TUNING.BUILDINGS.DECOR.NONE,
				noise: TUNING.NOISE_POLLUTION.NONE
			);
			def.InputConduitType = CONDUIT_TYPE;
			def.Floodable = false;
			def.Overheatable = false;
			def.ViewMode = OverlayModes.GasConduits.ID;
			def.AudioCategory = "Metal";
			def.UtilityInputOffset = new CellOffset(0, 0);
			def.UtilityOutputOffset = new CellOffset(0, 0);
			def.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, "GasVent");
			SoundEventVolumeCache.instance.AddVolume("ventgas_kanim", "GasVent_clunk", TUNING.NOISE_POLLUTION.NONE);
			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddOrGet<LoopingSounds>();
			go.AddOrGet<Exhaust>();
			go.AddOrGet<LogicOperationalController>();

			Vent vent = go.AddOrGet<Vent>();
			vent.conduitType = CONDUIT_TYPE;
			vent.endpointType = Endpoint.Sink;
			vent.overpressureMass = OVERPRESSURE_MASS;

			ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
			conduitConsumer.conduitType = ConduitType.Gas;
			conduitConsumer.ignoreMinMassCheck = true;

			BuildingTemplates.CreateDefaultStorage(go, false).showInUI = true;
			go.AddOrGet<SimpleVent>();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGetDef<VentController.Def>();
			go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
		}
	}
}
