using HarmonyLib;

using UnityEngine;

namespace RomenH.FestiveDecor
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
			if (ModSettings.Instance.EnableHQOverlay)
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
				else
				{
					Debug.Log("FestiveDecor: Could not find festive HQ overlay kanim.");
				}
			}
		}
	}
}
