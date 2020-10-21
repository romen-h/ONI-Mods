using Harmony;

using PeterHan.PLib;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace RomenMods.FestiveDecorMod
{
	/// <summary>
	/// Atmo Suit
	/// </summary>
	[HarmonyPatch(typeof(AtmoSuitConfig))]
	[HarmonyPatch("CreateEquipmentDef")]
	public static class AtmoSuitConfig_CreateEquipmentDef_Patch
	{
		public static void Postfix(EquipmentDef __result)
		{
			if (Mod.Settings.EnableCustomHelmets)
			{
				if (FestivalManager.CurrentFestival != Festival.None)
				{
					KAnimFile itemAnim = Assets.GetAnim($"suit_oxygen_{FestivalManager.FestivalAnimAffix}_kanim");
					KAnimFile suitAnim = Assets.GetAnim("body_oxygen_nohelm_kanim");
					if (itemAnim != null && suitAnim != null)
					{
						__result.Anim = itemAnim;
						__result.BuildOverride = suitAnim;
					}
				}
			}
		}
	}

	/// <summary>
	/// Suit Docking Station
	/// </summary>
	[HarmonyPatch(typeof(SuitLockerConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class SuitLockerConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			if (Mod.Settings.EnableCustomHelmets)
			{
				if (FestivalManager.CurrentFestival != Festival.None)
				{
					KAnimFile anim = Assets.GetAnim($"changingarea_{FestivalManager.FestivalAnimAffix}_kanim");
					if (anim != null) __result.AnimFiles = new KAnimFile[1] { anim };
				}
			}
		}
	}

	/// <summary>
	/// Add a child gameobject to the minions to show a helmet in front of everything
	/// </summary>
	[HarmonyPatch(typeof(MinionConfig))]
	[HarmonyPatch("OnSpawn")]
	public static class MinionConfig_OnSpawn_Patch
	{
		public static void Postfix(GameObject __0)
		{
			if (Mod.Settings.EnableCustomHelmets)
			{
				if (FestivalManager.CurrentFestival == Festival.Halloween)
				{
					KAnimFile anim = Assets.GetAnim("helm_festive_kanim");
					if (anim != null)
					{
						var dupeAnim = __0.GetComponent<KBatchedAnimController>();
						if (dupeAnim == null)
						{
							Debug.Log("FestiveDecor: Unable to create festive helmet. Dupe does not have an anim controller!");
							return;
						}

						GameObject helm = new GameObject(__0.name + ".FestiveHelm");
						helm.transform.position = new Vector3(0, 0, Grid.GetLayerZ(Grid.SceneLayer.Front));
						helm.SetActive(false);
						helm.transform.parent = __0.transform;

						var ac = helm.AddComponent<KBatchedAnimController>();
						ac.isMovable = true;
						ac.sceneLayer = Grid.SceneLayer.Front;
						ac.AnimFiles = new KAnimFile[1]
						{
							anim
						};

						var cmp = helm.AddComponent<FestiveHelmet>();
						cmp.dupeAnim = dupeAnim;
						cmp.myAnim = ac;

						helm.SetActive(true);
						string front_anim = FestivalManager.FestivalAnimAffix + "_front";
						ac.Play(front_anim);
					}
				}
			}
		}
	}

	/// <summary>
	/// Unity component that implements a special KBatchedAnimTracker
	/// </summary>
	public class FestiveHelmet : MonoBehaviour
	{
		public KBatchedAnimController myAnim;
		public KBatchedAnimController dupeAnim;

		private string front_anim = FestivalManager.FestivalAnimAffix + "_front";
		private string back_anim = FestivalManager.FestivalAnimAffix + "_back";

		private float constantZ = Grid.GetLayerZ(Grid.SceneLayer.Front);

		static KAnimHashedString symbol = new KAnimHashedString("snapTo_neck");

		private Vector3 offset = Vector3.zero;

		private Matrix2x3 previousMatrix;

		private Vector3 previousPosition;

		private void LateUpdate()
		{
			bool symbolVisible = false;
			if (dupeAnim.CurrentAnim != null)
			{
				var batch = dupeAnim.GetBatch();
				var frame = batch.group.data.GetFrame(dupeAnim.GetCurrentFrameIndex());
				if (frame != KAnim.Anim.Frame.InvalidFrame)
				{
					for (int i = 0; i < frame.numElements; i++)
					{
						int num = frame.firstElementIdx + i;
						if (num < batch.group.data.frameElements.Count)
						{
							KAnim.Anim.FrameElement frameElement = batch.group.data.frameElements[num];
							if (frameElement.symbol == symbol)
							{
								if (frameElement.frame == 0)
								{
									myAnim.Play(front_anim);
								}
								else
								{
									myAnim.Play(back_anim);
								}
							}
						}
					}
				}

				symbolVisible = dupeAnim.GetSymbolVisiblity(symbol);
				bool unusedBool;
				Matrix2x3 symbolLocalTransform = dupeAnim.GetSymbolLocalTransform(symbol, out unusedBool);
				Vector3 position = dupeAnim.transform.GetPosition();
				if (symbolVisible && (previousMatrix != symbolLocalTransform || position != previousPosition))
				{
					previousMatrix = symbolLocalTransform;
					previousPosition = position;
					Matrix2x3 overrideTransformMatrix = dupeAnim.GetTransformMatrix() * symbolLocalTransform;
					float z = base.transform.GetPosition().z;
					base.transform.SetPosition(overrideTransformMatrix.MultiplyPoint(offset));

					Vector3 v = dupeAnim.FlipX ? Vector3.left : Vector3.right;
					Vector3 v2 = dupeAnim.FlipY ? Vector3.down : Vector3.up;
					base.transform.up = overrideTransformMatrix.MultiplyVector(v2);
					base.transform.right = overrideTransformMatrix.MultiplyVector(v);
					if (myAnim != null)
					{
						myAnim.GetBatchInstanceData()?.SetOverrideTransformMatrix(overrideTransformMatrix);
					}

					base.transform.SetPosition(new Vector3(base.transform.GetPosition().x, base.transform.GetPosition().y, z));

					myAnim.Offset = dupeAnim.Offset;
					myAnim.SetDirty();
				}
			}
			if (myAnim != null && symbolVisible != myAnim.enabled)
			{
				myAnim.enabled = symbolVisible;
			}
		}
	}
}
