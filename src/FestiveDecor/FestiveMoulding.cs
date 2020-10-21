using Harmony;

using JetBrains.Annotations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace RomenMods.FestiveDecorMod
{
	/// <summary>
	/// Corner Moulding
	/// </summary>
	[HarmonyPatch(typeof(CornerMouldingConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class CornerMouldingConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			if (FestivalManager.CurrentFestival != Festival.None)
			{
				KAnimFile anim = Assets.GetAnim($"corner_tile_{FestivalManager.FestivalAnimAffix}_kanim");
				if (anim != null) __result.AnimFiles = new KAnimFile[1] { anim };
			}
		}
	}

	/// <summary>
	/// Ceiling Crawler Spawner
	/// </summary>
	[HarmonyPatch(typeof(CornerMouldingConfig))]
	[HarmonyPatch("DoPostConfigureComplete")]
	public static class CornerMouldingConfig_DoPostConfigureComplete_Patch
	{
		public static void Postfix(GameObject __0)
		{
			if (Mod.Settings.EnableSpiders && FestivalManager.CurrentFestival == Festival.Halloween)
			{
				__0.AddComponent<MouldingSpiderSpawner>();
			}
		}
	}

	[HarmonyPatch(typeof(CrownMouldingConfig))]
	[HarmonyPatch("CreateBuildingDef")]
	public static class CrownMouldingConfig_CreateBuildingDef_Patch
	{
		public static void Postfix(BuildingDef __result)
		{
			if (FestivalManager.CurrentFestival != Festival.None)
			{
				KAnimFile anim = Assets.GetAnim($"crown_moulding_{FestivalManager.FestivalAnimAffix}_kanim");
				if (anim != null) __result.AnimFiles = new KAnimFile[1] { anim };
			}
		}
	}

	public class MouldingSpiderSpawner : KMonoBehaviour
	{
		GameObject mySpider;

		protected override void OnSpawn()
		{
			base.OnSpawn();

			mySpider = new GameObject();
			mySpider.transform.position = new Vector3(transform.position.x, transform.position.y + 0.7f, Grid.GetLayerZ(Grid.SceneLayer.Creatures));
			mySpider.SetActive(false);

			var anim = mySpider.AddComponent<KBatchedAnimController>();
			anim.isMovable = true;
			anim.sceneLayer = Grid.SceneLayer.Creatures;
			anim.FlipY = true;
			anim.AnimFiles = new KAnimFile[1]
			{
				Assets.GetAnim("decorations_halloween_kanim")
			};

			mySpider.AddComponent<CeilingCrawler>();
			mySpider.SetActive(true);
		}

		protected override void OnCleanUp()
		{
			base.OnCleanUp();

			mySpider.DeleteObject();
		}
	}

	public class CeilingCrawler : MonoBehaviour
	{
		private enum MotionState
		{
			Idle,
			WalkingRight,
			WalkingLeft
		}

		KBatchedAnimController anim;

		int currentCell;

		private MotionState currentMotion = MotionState.Idle;
		private MotionState currentAnimState = MotionState.Idle;

		float timeSinceLastDecision = 0f;

		Vector3 fromPos;
		Vector3 toPos;

		private Vector3 GetCellPoint(int cell)
		{
			var cellXY = Grid.CellToPos(cell);
			return new Vector3(cellXY.x + 0.5f, cellXY.y + 0.7f, transform.position.z);
		}

		void Start()
		{
			//Debug.Log("Spider spawned.");
			anim = gameObject.GetComponent<KBatchedAnimController>();
			anim.Play("spider");

			currentCell = Grid.PosToCell(gameObject);
			fromPos = GetCellPoint(currentCell);
			toPos = GetCellPoint(currentCell);
		}

		private void MakeDecision()
		{
			

			bool isInWeb = false;
			var building = Grid.Objects[currentCell, (int)ObjectLayer.Building];
			if (building != null)
			{
				isInWeb = building.HasTag(CornerMouldingConfig.ID.ToTag());
			}

			int cellToRight = Grid.CellRight(currentCell);
			var rightBuilding = Grid.Objects[cellToRight, (int)ObjectLayer.Building];
			bool canGoRight = false;
			if (rightBuilding != null)
			{
				canGoRight = rightBuilding.HasTag(CrownMouldingConfig.ID.ToTag()) || rightBuilding.HasTag(CornerMouldingConfig.ID.ToTag());
			}

			int cellToLeft = Grid.CellLeft(currentCell);
			var leftBuilding = Grid.Objects[cellToLeft, (int)ObjectLayer.Building];
			bool canGoLeft = false;
			if (leftBuilding != null)
			{
				canGoLeft = leftBuilding.HasTag(CrownMouldingConfig.ID.ToTag()) || leftBuilding.HasTag(CornerMouldingConfig.ID.ToTag());
			}

			List<MotionState> options = new List<MotionState>();
			options.Add(MotionState.Idle);
			options.Add(MotionState.Idle);

			if (isInWeb)
			{
				options.Add(MotionState.Idle);
				options.Add(MotionState.Idle);
			}

			if (canGoRight && currentMotion != MotionState.WalkingLeft)
			{
				options.Add(MotionState.WalkingRight);
				if (currentMotion == MotionState.WalkingRight)
				{
					options.Add(MotionState.WalkingRight);
				}
			}

			if (canGoLeft && currentMotion != MotionState.WalkingRight)
			{
				options.Add(MotionState.WalkingLeft);
				if (currentMotion == MotionState.WalkingLeft)
				{
					options.Add(MotionState.WalkingLeft);
				}
			}

			currentMotion = options.GetRandom();

			//Debug.Log($"FestiveDecor: Spider decided on {currentMotion}");

			int toCell = currentCell;
			switch (currentMotion)
			{
				case MotionState.WalkingRight:
					toCell = cellToRight;
					break;
				case MotionState.WalkingLeft:
					toCell = cellToLeft;
					break;

				default:
					break;
			}

			fromPos = GetCellPoint(currentCell);
			toPos = GetCellPoint(toCell);
			timeSinceLastDecision = 0f;
		}

		void Update()
		{
			currentCell = Grid.PosToCell(gameObject);
			timeSinceLastDecision += Time.deltaTime;

			switch (currentMotion)
			{
				case MotionState.Idle:
					Idle();
					break;

				case MotionState.WalkingRight:
					WalkRight();
					break;

				case MotionState.WalkingLeft:
					WalkLeft();
					break;
			}

			transform.position = Vector3.Lerp(fromPos, toPos, Mathf.Clamp(timeSinceLastDecision, 0f, 1f));

			if (timeSinceLastDecision >= 1.0f)
			{
				MakeDecision();
			}
		}

		private void Idle()
		{
			if (currentAnimState != MotionState.Idle)
			{
				//Debug.Log("Spider idle");
				anim.Play("spider");
				currentAnimState = MotionState.Idle;
			}
		}

		private void WalkRight()
		{
			if (currentAnimState != MotionState.WalkingRight)
			{
				//Debug.Log("Spider walk right");
				anim.Play("spider_walk", KAnim.PlayMode.Loop);
				anim.FlipX = false;
				currentAnimState = MotionState.WalkingRight;
			}
		}

		private void WalkLeft()
		{
			if (currentAnimState != MotionState.WalkingLeft)
			{
				//Debug.Log("Spider walk left");
				anim.Play("spider_walk", KAnim.PlayMode.Loop);
				anim.FlipX = true;
				currentAnimState = MotionState.WalkingLeft;
			}
		}
	}
}
