using Harmony;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace RomenMods.FestiveDecorMod
{
	/// <summary>
	/// Base Headquarters
	/// </summary>
	[HarmonyPatch(typeof(Telepad))]
	[HarmonyPatch("OnSpawn")]
	public static class Telepad_OnSpawn_Patch
	{
		public static void Postfix(Telepad __instance)
		{
			KAnimFile anim = Assets.GetAnim("hqbase_festive_overlay_kanim");
			if (anim != null)
			{
				GameObject go = __instance.gameObject;
				GameObject overlay = new GameObject(go.name + ".Decorations");
				overlay.transform.position = new Vector3(0, 0, Grid.GetLayerZ(Grid.SceneLayer.BuildingFront));
				overlay.SetActive(false);
				overlay.transform.parent = go.transform;
				overlay.transform.localPosition = new Vector3(0.5f, 0, 0);

				var ac = overlay.AddComponent<KBatchedAnimController>();
				ac.isMovable = true;
				ac.sceneLayer = Grid.SceneLayer.BuildingFront;
				ac.AnimFiles = new KAnimFile[1] { anim };

				overlay.SetActive(true);

				try
				{
					ac.Play(FestivalManager.FestivalAnimAffix);
				}
				catch
				{ }
			}
		}
	}

	/// <summary>
	/// Ration Box
	/// </summary>
	[HarmonyPatch(typeof(RationBoxConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class RationBoxConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			if (FestivalManager.CurrentFestival != Festival.None)
			{
				KAnimFile anim = Assets.GetAnim($"rationbox_{FestivalManager.FestivalAnimAffix}_kanim");
				if (anim != null) __result.AnimFiles = new KAnimFile[1] { anim };
			}
		}
	}
}
