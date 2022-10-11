
using RomenH.Common;

using STRINGS;

using TUNING;

using UnityEngine;

namespace RomenH.StirlingEngine
{
	public class StirlingEngineConfig : IBuildingConfig
	{
		internal const float MAX_TEMP = TUNING.BUILDINGS.OVERHEAT_TEMPERATURES.HIGH_3;

		public const string ID = "StirlingEngine";

		public static readonly LocString Name = StringUtils.BuildingName(ID, "Stirling Engine");

		public static readonly LocString Desc = StringUtils.BuildingDesc(ID, "Draws heat from the cell below the floor and converts it to power. The amount of heat drawn is based on the ratio of building temperature vs temperature below the floor tile.");

		public static readonly LocString Effect = StringUtils.BuildingEffect(ID, "Stirling Engines draw " + UI.FormatAsLink("Heat", "HEAT") + " from the room below and harness that heat to generate " + UI.FormatAsLink("Power", "POWER") + ".");

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 3,
				anim: "stirling_kanim",
				hitpoints: 30,
				construction_time: 60f,
				construction_mass: new float[] {
					TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4[0]
				},
				construction_materials: TUNING.MATERIALS.ALL_METALS,
				melting_point: 1600f,
				build_location_rule: BuildLocationRule.OnFloor,
				decor: DECOR.PENALTY.TIER3,
				noise: NOISE_POLLUTION.NOISY.TIER3
			);
			def.AudioCategory = TUNING.AUDIO.CATEGORY.HOLLOW_METAL;
			def.AudioSize = "large";
			def.Floodable = false;
			def.Entombable = true;
			def.GeneratorWattageRating = ModSettings.Instance.MaxWattOutput;
			def.GeneratorBaseCapacity = def.GeneratorWattageRating;
			def.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
			def.Overheatable = true;
			def.OverheatTemperature = MAX_TEMP;
			def.PermittedRotations = PermittedRotations.Unrotatable;
			def.RequiresPowerOutput = true;
			def.PowerOutputOffset = new CellOffset(0, 0);
			def.ViewMode = OverlayModes.Power.ID;
			def.SelfHeatKilowattsWhenActive = 0f;
			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
			Prioritizable.AddRef(go);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<LogicOperationalController>();
			var engine = go.AddOrGet<StirlingEngine>();
			var tinkerable = Tinkerable.MakePowerTinkerable(go);
			tinkerable.SetWorkTime(120f);

			go.AddOrGetDef<PoweredActiveController.Def>();
		}
	}
}
