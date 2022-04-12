using RomenH.Common;

using TUNING;

using UnityEngine;

namespace InfiniteSourceSink
{
	public class InfiniteLiquidSinkConfig : IBuildingConfig
	{
		public const string ID = "RomenH_InfiniteLiquidSink";
		public const string DisplayName = "Infinite Liquid Sink";
		public const string Description = "Voids all liquid sent into it.";
		public const string Effect = "Where does all the liquid go?";

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 1,
				anim: "sink_liquid_kanim",
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
			buildingDef.InputConduitType = ConduitType.Liquid;
			buildingDef.Floodable = false;
			buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
			buildingDef.UtilityInputOffset = new CellOffset(0, 0);
			buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, ID);
			return buildingDef;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);

			go.AddOrGet<LogicOperationalController>();

			var sink = go.AddOrGet<InfiniteSink>();
			sink.Type = ConduitType.Liquid;

			Object.DestroyImmediate(go.GetComponent<RequireInputs>(), true);
			Object.DestroyImmediate(go.GetComponent<ConduitConsumer>(), true);

			go.AddOrGetDef<OperationalController.Def>();
			go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
		}
	}
}
