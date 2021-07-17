using TUNING;
using UnityEngine;

namespace RomenH.PipedDeodorizer
{
	public class PipedDeodorizerConfig : IBuildingConfig
	{
		public const string ID = "PipedDeodorizer";

		private const float AIR_INPUT_RATE = 0.5f;

		private const float AIR_OUTPUT_RATE = 0.49f;

		private const float DIRT_OUTPUT_RATE = 0.01f;

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				ID,
				2,
				2,
				"piped_deodorizer_kanim",
				30,
				30f,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				MATERIALS.RAW_METALS,
				800f,
				BuildLocationRule.Anywhere,
				noise: NOISE_POLLUTION.NOISY.TIER3,
				decor: BUILDINGS.DECOR.PENALTY.TIER1
			);
			def.RequiresPowerInput = true;
			def.EnergyConsumptionWhenActive = 120f;
			def.SelfHeatKilowattsWhenActive = 1f;
			def.InputConduitType = ConduitType.Gas;
			def.OutputConduitType = ConduitType.Gas;
			def.ViewMode = OverlayModes.GasConduits.ID;
			def.AudioCategory = "Metal";
			def.UtilityInputOffset = new CellOffset(0, 0);
			def.UtilityOutputOffset = new CellOffset(1, 0);
			def.PowerInputOffset = new CellOffset(1, 0);
			def.PermittedRotations = PermittedRotations.FlipH;
			def.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 0));
			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, ID);
			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddOrGet<LoopingSounds>();
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

			Storage storage = BuildingTemplates.CreateDefaultStorage(go);
			storage.showInUI = true;
			storage.capacityKg = 30000f;
			storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);

			go.AddOrGet<PipedDeodorizer>();

			ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
			elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
			{
				new ElementConverter.ConsumedElement(SimHashes.ContaminatedOxygen.CreateTag(), AIR_INPUT_RATE),
			};
			elementConverter.outputElements = new ElementConverter.OutputElement[2]
			{
				new ElementConverter.OutputElement(AIR_OUTPUT_RATE, SimHashes.Oxygen, 0f, useEntityTemperature: false, storeOutput: true),
				new ElementConverter.OutputElement(DIRT_OUTPUT_RATE, SimHashes.ToxicSand, 0f, useEntityTemperature: false, storeOutput: true)
			};

			ElementDropper elementDropper = go.AddComponent<ElementDropper>();
			elementDropper.emitMass = 10f;
			elementDropper.emitTag = new Tag("ToxicSand");
			elementDropper.emitOffset = new Vector3(1f, 1f, 0f);

			ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
			conduitConsumer.conduitType = ConduitType.Gas;
			conduitConsumer.consumptionRate = AIR_INPUT_RATE;
			conduitConsumer.capacityKG = 2*AIR_INPUT_RATE;
			conduitConsumer.capacityTag = GameTags.Breathable;// ElementLoader.FindElementByHash(SimHashes.ContaminatedOxygen).tag;
			conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
			conduitConsumer.forceAlwaysSatisfied = true;

			ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
			conduitDispenser.conduitType = ConduitType.Gas;
			conduitDispenser.alwaysDispense = true;
			conduitDispenser.invertElementFilter = true;
			conduitDispenser.elementFilter = new SimHashes[]
			{
				SimHashes.ContaminatedOxygen
			};

			go.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<LogicOperationalController>();
			go.AddOrGetDef<PoweredActiveController.Def>();
			go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
		}
	}
}
