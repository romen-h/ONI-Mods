using RomenH.Common;

using TUNING;

using UnityEngine;

namespace RomenH.GermicideLamp
{
	public class CeilingGermicideLampConfig : IBuildingConfig
	{
		public const string ID = "SmallGermicideLamp";

		public static readonly LocString Name = StringUtils.BuildingName(ID, "Germicidal Ceiling Light");

		public static readonly LocString Desc = StringUtils.BuildingDesc(ID, "");

		public static readonly LocString Effect = StringUtils.BuildingEffect(ID, "Provides a small amount of light while killing germs beneath them with UVC radiation.");

		internal const int UV_LEFT = -1;
		internal const int UV_WIDTH = 4;
		internal const int UV_BOTTOM = -3;
		internal const int UV_HEIGHT = 4;

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
			def.EnergyConsumptionWhenActive = ModSettings.Instance.CeilingLampPowerCost;
			def.SelfHeatKilowattsWhenActive = ModSettings.Instance.CeilingLampHeat;
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
			LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
			lightShapePreview.lux = ModSettings.Instance.CeilingLampLux;
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
			ExtentsHelpers.CeilingUVExtents(ModSettings.Instance.CeilingLampRangeWidth, ModSettings.Instance.CeilingLampRangeHeight, out int left, out int width, out int bottom, out int height);

			go.AddOrGet<LoopingSounds>();
			GermicideLamp lamp = go.AddOrGet<GermicideLamp>();
			lamp.aoeLeft = left;
			lamp.aoeWidth = width;
			lamp.aoeBottom = bottom;
			lamp.aoeHeight = height;
			lamp.applySunburn = ModSettings.Instance.CeilingLampGivesSunburn;
			lamp.basePower = ModSettings.Instance.CeilingLampGermicidalStrength;
			lamp.flicker = true;
			go.AddOrGet<LogicOperationalController>();
			go.AddOrGetDef<PoweredActiveController.Def>();
			AddVisualizer(go, movable: false);

			var light2D = go.AddOrGet<Light2D>();
			light2D.overlayColour = LIGHT2D.CEILINGLIGHT_OVERLAYCOLOR;
			light2D.Color = ModSettings.Instance.LightColor;
			light2D.Range = 6f;
			light2D.Angle = 2.6f;
			light2D.Direction = LIGHT2D.CEILINGLIGHT_DIRECTION;
			light2D.Offset = new Vector2(0.55f, 0.65f);
			light2D.shape = LightShape.Cone;
			light2D.drawOverlay = true;
			light2D.Lux = ModSettings.Instance.CeilingLampLux;
			go.AddOrGetDef<LightController.Def>();

			// TODO: Add a right Light on child object?
		}

		private static void AddVisualizer(GameObject prefab, bool movable)
		{
			ExtentsHelpers.CeilingUVExtents(ModSettings.Instance.CeilingLampRangeWidth, ModSettings.Instance.CeilingLampRangeHeight, out int left, out int width, out int bottom, out int height);

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
