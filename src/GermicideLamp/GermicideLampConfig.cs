using TUNING;

using UnityEngine;

namespace RomenH.GermicideLamp
{
	public class GermicideLampConfig : IBuildingConfig
	{
		internal const int UV_LEFT = -3;
		internal const int UV_WIDTH = 7;
		internal const int UV_BOTTOM = -3;
		internal const int UV_HEIGHT = 9;

		public const string ID = "GermicideLamp";

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 3,
				anim: "uvlamp_kanim",
				hitpoints: 10,
				construction_time: 10f,
				construction_mass: new float[] {
					BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0],
					BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0]
				},
				construction_materials: new string[] {
					TUNING.MATERIALS.METAL,
					"Ceramic"
				},
				melting_point: 1600f,
				build_location_rule: BuildLocationRule.OnFloor,
				decor: DECOR.PENALTY.TIER1,
				noise: NOISE_POLLUTION.NONE
			);
			def.RequiresPowerInput = true;
			def.EnergyConsumptionWhenActive = ModSettings.Instance.BigLampPowerCost;
			def.SelfHeatKilowattsWhenActive = ModSettings.Instance.BigLampHeat;
			def.AudioCategory = "Metal";
			def.Floodable = false;
			def.Entombable = true;
			def.Overheatable = false;
			def.ViewMode = OverlayModes.Disease.ID;
			def.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
			def.PermittedRotations = PermittedRotations.Unrotatable;
			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.DiseaseIDs, ID);

			return def;
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			AddVisualizer(go, movable: true);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			AddVisualizer(go, movable: false);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			ExtentsHelpers.CenteredUVExtents(ModSettings.Instance.BigLampRange, 1, 3, out int left, out int width, out int bottom, out int height);

			go.AddOrGet<LoopingSounds>();
			go.AddOrGet<EnergyConsumer>();
			var lamp = go.AddOrGet<GermicideLamp>();
			lamp.aoeLeft = left;
			lamp.aoeWidth = width;
			lamp.aoeBottom = bottom;
			lamp.aoeHeight = height;
			lamp.applySunburn = ModSettings.Instance.BigLampGivesSunburn;
			lamp.strength = ModSettings.Instance.BigLampGermicidalStrength;
			go.AddOrGet<LogicOperationalController>();
			go.AddOrGetDef<PoweredActiveController.Def>();
			AddVisualizer(go, movable: false);
		}

		private static void AddVisualizer(GameObject prefab, bool movable)
		{
			ExtentsHelpers.CenteredUVExtents(ModSettings.Instance.BigLampRange, 1, 3, out int left, out int width, out int bottom, out int height);

			StationaryChoreRangeVisualizer stationaryChoreRangeVisualizer = prefab.AddOrGet<StationaryChoreRangeVisualizer>();
			stationaryChoreRangeVisualizer.x = left;
			stationaryChoreRangeVisualizer.y = bottom;
			stationaryChoreRangeVisualizer.width = width;
			stationaryChoreRangeVisualizer.height = height;
			stationaryChoreRangeVisualizer.movable = movable;
			stationaryChoreRangeVisualizer.blocking_tile_visible = true;

			prefab.GetComponent<KPrefabID>().instantiateFn += delegate (GameObject go)
			{
				go.GetComponent<StationaryChoreRangeVisualizer>().blocking_cb = (cell) =>
				{
					return Grid.VisibleBlockingCB(cell);
				};
			};
		}


	}
}
