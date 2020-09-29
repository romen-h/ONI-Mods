

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TUNING;

using UnityEngine;

namespace RomenMods.GermicideLampMod
{
	public class CeilingGermicideLampConfig : IBuildingConfig
	{
		internal const int UV_LEFT = -1;
		internal const int UV_WIDTH = 4;
		internal const int UV_BOTTOM = -3;
		internal const int UV_HEIGHT = 4;

		public const string ID = "SmallGermicideLamp";

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 2,
				height: 1,
				anim: "uvlamp_small_kanim",
				hitpoints: 10,
				construction_time: 10f,
				construction_mass: new float[] {
					BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0],
					BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0]
				},
				construction_materials: new string[] {
					TUNING.MATERIALS.METAL,
					"Ceramic"
				},
				melting_point: 1600f,
				build_location_rule: BuildLocationRule.OnCeiling,
				decor: DECOR.PENALTY.TIER1,
				noise: NOISE_POLLUTION.NONE
			);
			def.RequiresPowerInput = true;
			def.EnergyConsumptionWhenActive = Mod.Settings.CeilingLampPowerCost;
			def.SelfHeatKilowattsWhenActive = 1f;
			def.AudioCategory = "Metal";
			def.Floodable = false;
			def.Entombable = true;
			def.Overheatable = false;
			def.ViewMode = OverlayModes.Disease.ID;
			def.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
			def.PermittedRotations = PermittedRotations.FlipH;
			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.DiseaseIDs, ID);

			return def;
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			AddVisualizer(go, movable: true);
			LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
			lightShapePreview.lux = Mod.Settings.CeilingLampLux;
			lightShapePreview.radius = 8f;
			lightShapePreview.shape = LightShape.Cone;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LightSource);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			AddVisualizer(go, movable: false);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<LoopingSounds>();
			GermicideLamp lamp = go.AddOrGet<GermicideLamp>();
			lamp.aoeLeft = UV_LEFT;
			lamp.aoeWidth = UV_WIDTH;
			lamp.aoeBottom = UV_BOTTOM;
			lamp.aoeHeight = UV_HEIGHT;
			lamp.applySunburn = Mod.Settings.CeilingLampGivesSunburn;
			lamp.strength = Mod.Settings.CeilingLampGermicidalStrength;
			go.AddOrGet<LogicOperationalController>();
			go.AddOrGetDef<PoweredActiveController.Def>();
			AddVisualizer(go, movable: false);

			GameObject leftLight = go;
			var leftLight2D = leftLight.AddOrGet<Light2D>();
			leftLight2D.overlayColour = LIGHT2D.CEILINGLIGHT_OVERLAYCOLOR;
			leftLight2D.Color = LIGHT2D.CEILINGLIGHT_COLOR;
			leftLight2D.Range = 6f;
			leftLight2D.Angle = 2.6f;
			leftLight2D.Direction = LIGHT2D.CEILINGLIGHT_DIRECTION;
			leftLight2D.Offset = LIGHT2D.CEILINGLIGHT_OFFSET;
			leftLight2D.shape = LightShape.Cone;
			leftLight2D.drawOverlay = true;
			leftLight2D.Lux = 600;
			leftLight.AddOrGetDef<LightController.Def>();

			// TODO: Add a right Light on child object?
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
