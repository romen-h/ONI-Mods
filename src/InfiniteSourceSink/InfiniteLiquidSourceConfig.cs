using RomenH.Common;

using TUNING;

using UnityEngine;

namespace InfiniteSourceSink
{
	public class InfiniteLiquidSourceConfig : IBuildingConfig
	{
		public const string ID = "RomenH_InfiniteLiquidSource";
		public const string DisplayName = "Infinite Liquid Source";
		public const string Description = "Materializes liquid from the void.";
		public const string Effect = "Where is all the liquid coming from?";

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 1,
				anim: "source_liquid_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER2,
				construction_time: ModSettings.Instance.BuildTimeSeconds,
				construction_mass: ModSettings.Instance.BuildMassKg,
				construction_materials: ModSettings.Instance.SandboxOnly ? GameStrings.MaterialLists.Neutronium : MATERIALS.REFINED_METALS,
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER4,
				build_location_rule: BuildLocationRule.Anywhere,
				decor: BUILDINGS.DECOR.PENALTY.TIER1,
				noise: NOISE_POLLUTION.NOISY.TIER4,
				0.2f
			);
			buildingDef.Overheatable = false;
			buildingDef.Entombable = false;
			buildingDef.Floodable = false;
			buildingDef.OutputConduitType = ConduitType.Liquid;
			buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
			buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
			buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, ID);
			return buildingDef;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<LogicOperationalController>();
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);

			var src = go.AddOrGet<InfiniteSource>();
			src.conduitType = ConduitType.Liquid;

			go.AddOrGet<InfiniteSourceFlowControl>();
			go.AddOrGet<InfiniteSourceTempControl>();
			var filterable = go.AddOrGet<Filterable>();
			filterable.filterElementState = Filterable.ElementState.Liquid;

			go.AddOrGetDef<OperationalController.Def>();
			go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
		}
	}
}
