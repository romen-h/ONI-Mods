using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using STRINGS;

using UnityEngine;

namespace RomenH.PlasticUtilities
{
	public class PlasticLiquidVentConfig : IBuildingConfig
	{
		public const string ID = "RomenH_PlasticLiquidVent";

		public const string Name = "Plastic Liquid Vent";

		public const string Desc = "Vents are an exit point for liquids from plumbing systems.";

		public static readonly string Effect = string.Concat(new string[]
		{
			"Releases ",
			UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
			" from ",
			UI.FormatAsLink("Liquid Pipes", "LIQUIDPIPING"),
			"."
		});

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 1,
				anim: "plastic_ventliquid_kanim",
				hitpoints:30,
				construction_time: 30f,
				construction_mass: TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
				construction_materials: TUNING.MATERIALS.PLASTICS,
				melting_point: 800f,
				build_location_rule: BuildLocationRule.Anywhere,
				decor: TUNING.BUILDINGS.DECOR.NONE,
				noise: TUNING.NOISE_POLLUTION.NOISY.TIER0
			);
			def.InputConduitType = ConduitType.Liquid;
			def.Floodable = false;
			def.Overheatable = false;
			def.ViewMode = OverlayModes.LiquidConduits.ID;
			def.AudioCategory = "Metal";
			def.UtilityInputOffset = new CellOffset(0, 0);
			def.UtilityOutputOffset = new CellOffset(0, 0);
			def.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "LiquidVent");
			SoundEventVolumeCache.instance.AddVolume("ventliquid_kanim", "LiquidVent_squirt", TUNING.NOISE_POLLUTION.NOISY.TIER0);
			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddOrGet<LoopingSounds>();

			go.AddOrGet<Exhaust>();

			go.AddOrGet<LogicOperationalController>();

			Vent vent = go.AddOrGet<Vent>();
			vent.conduitType = ConduitType.Liquid;
			vent.endpointType = Endpoint.Sink;
			vent.overpressureMass = 500f;

			ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
			conduitConsumer.conduitType = ConduitType.Liquid;
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
