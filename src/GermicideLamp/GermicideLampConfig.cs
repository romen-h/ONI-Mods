using TUNING;

using UnityEngine;

namespace RomenMods.GermicideLampMod
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
			def.EnergyConsumptionWhenActive = Mod.Settings.BigLampPowerCost;
			def.SelfHeatKilowattsWhenActive = Mod.Settings.BigLampHeat;
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
			go.AddOrGet<LoopingSounds>();
			var lamp = go.AddOrGet<GermicideLamp>();
			lamp.aoeLeft = UV_LEFT;
			lamp.aoeWidth = UV_WIDTH;
			lamp.aoeBottom = UV_BOTTOM;
			lamp.aoeHeight = UV_HEIGHT;
			lamp.applySunburn = Mod.Settings.BigLampGivesSunburn;
			lamp.strength = Mod.Settings.BigLampGermicidalStrength;
			go.AddOrGet<LogicOperationalController>();
			go.AddOrGetDef<PoweredActiveController.Def>();
			AddVisualizer(go, movable: false);
		}

		private static void AddVisualizer(GameObject prefab, bool movable)
		{
			StationaryChoreRangeVisualizer stationaryChoreRangeVisualizer = prefab.AddOrGet<StationaryChoreRangeVisualizer>();
			stationaryChoreRangeVisualizer.x = UV_LEFT;
			stationaryChoreRangeVisualizer.y = UV_BOTTOM;
			stationaryChoreRangeVisualizer.width = UV_WIDTH;
			stationaryChoreRangeVisualizer.height = UV_HEIGHT;
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
