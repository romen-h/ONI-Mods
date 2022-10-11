using RomenH.Common;

using TUNING;

using UnityEngine;

namespace RomenH.PipedDeodorizer
{
	public class PipedDeodorizerConfig : IBuildingConfig
	{
		public const string ID = "PipedDeodorizer";

		public static readonly LocString Name = StringUtils.BuildingName(ID, "Piped Deodorizer");

		public static readonly LocString Desc = StringUtils.BuildingDesc(ID, "");

		public static readonly LocString Effect = StringUtils.BuildingEffect(ID, "Filters " + STRINGS.UI.FormatAsKeyWord("Polluted Oxygen") + " from the air pumped through it.");

		private const float AIR_OUTPUT_RATIO = 0.9f;

		private const float FILTER_CONSUMPTION_RATIO = 1.2f;

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
			def.EnergyConsumptionWhenActive = ModSettings.Instance.Wattage;
			def.SelfHeatKilowattsWhenActive = ModSettings.Instance.HeatGenerated;
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

			ElementConverter elementConverter = go.AddOrGet<ElementConverter>();

			float intakeRate = ModSettings.Instance.IntakeRate;
			float airOutputRate = intakeRate * AIR_OUTPUT_RATIO;

			if (ModSettings.Instance.ClassicDeodorizer)
			{
				float filterConsumptionRate = intakeRate * FILTER_CONSUMPTION_RATIO;

				float clayOutputRate = filterConsumptionRate + (intakeRate - airOutputRate);

				elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
				{
					new ElementConverter.ConsumedElement(GameTags.Filter, filterConsumptionRate),
					new ElementConverter.ConsumedElement(SimHashes.ContaminatedOxygen.CreateTag(), intakeRate),
				};
				elementConverter.outputElements = new ElementConverter.OutputElement[]
				{
					new ElementConverter.OutputElement(airOutputRate, SimHashes.Oxygen, 0f, useEntityTemperature: false, storeOutput: true),
					new ElementConverter.OutputElement(clayOutputRate, SimHashes.Clay, 0f, useEntityTemperature: false, storeOutput: true)
				};

				ManualDeliveryKG manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
				manualDeliveryKG.SetStorage(storage);
				manualDeliveryKG.RequestedItemTag = GameTags.Filter;
				manualDeliveryKG.capacity = 1000f;
				manualDeliveryKG.refillMass = 50f;
				manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
			}
			else
			{
				float dirtOutputRate = intakeRate - airOutputRate;

				elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
				{
					new ElementConverter.ConsumedElement(SimHashes.ContaminatedOxygen.CreateTag(), intakeRate),
				};
				elementConverter.outputElements = new ElementConverter.OutputElement[]
				{
					new ElementConverter.OutputElement(airOutputRate, SimHashes.Oxygen, 0f, useEntityTemperature: false, storeOutput: true),
					new ElementConverter.OutputElement(dirtOutputRate, SimHashes.ToxicSand, 0f, useEntityTemperature: false, storeOutput: true)
				};
			}

			ElementDropper pollutedDirtDropper = go.AddComponent<ElementDropper>();
			pollutedDirtDropper.emitMass = 10f;
			pollutedDirtDropper.emitTag = SimHashes.ToxicSand.CreateTag();
			pollutedDirtDropper.emitOffset = new Vector3(1f, 1f, 0f);

			ElementDropper clayDropper = go.AddComponent<ElementDropper>();
			clayDropper.emitMass = 10f;
			clayDropper.emitTag = SimHashes.Clay.CreateTag();
			clayDropper.emitOffset = new Vector3(1f, 1f, 0f);

			ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
			conduitConsumer.conduitType = ConduitType.Gas;
			conduitConsumer.consumptionRate = intakeRate;
			conduitConsumer.capacityKG = 2 * intakeRate;
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

			go.AddOrGet<PipedDeodorizer>();

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
