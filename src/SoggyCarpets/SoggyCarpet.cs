using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Klei;

using UnityEngine;

namespace SoggyCarpets
{
	public class SoggyCarpet : KMonoBehaviour, ISim200ms
	{
		internal static readonly HashSet<int> SoggyCells = new HashSet<int>();

		public float dripRate = 0.5f;

		private int myCell;
		private int cellBelow;

		[MyCmpGet]
		private Building building;

		[MyCmpGet]
		private Storage storage;

		[MyCmpGet]
		private ElementConsumer consumer;

		protected override void OnSpawn()
		{
			myCell = building.GetCell();
			cellBelow = Grid.GetCellInDirection(myCell, Direction.Down);
			consumer.EnableConsumption(true);
		}

		protected override void OnCleanUp()
		{
			SoggyCells.Remove(building.GetCell());
		}

		public void Sim200ms(float dt)
		{
			Emit(dt);

			if (storage.MassStored() > 0f)
			{
				SoggyCells.Add(myCell);
			}
			else
			{
				SoggyCells.Remove(myCell);
			}
		}

		private void Emit(float dt)
		{
			SoggyCarpet carpetBelow = null;

			bool doDrip = true;

			if (Grid.Solid[cellBelow])
			{
				// Do not drip if solid tile below
				doDrip = false;

				// But...
				Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].TryGetValue(cellBelow, out GameObject go);
				if (go != null)
				{
					KPrefabID prefabID = go.GetComponent<KPrefabID>();

					// Drip if there's a mesh tile below
					if (prefabID.PrefabTag == MeshTileConfig.ID) doDrip = true;

					// Drip if there's another tile below, mass transfer implemented below
					carpetBelow = go.GetComponent<SoggyCarpet>();
					if (carpetBelow != null) doDrip = true;
				}
			}

			if (!doDrip) return;

			PrimaryElement firstPrimaryElement = storage.FindFirstWithMass(GameTags.Liquid);
			if (firstPrimaryElement == null) return;

			Element element = firstPrimaryElement.Element;
			if (element == null) return;
			if (!element.IsLiquid) return;
			byte elementIndex = element.idx;

			float massToDrip = Mathf.Min(firstPrimaryElement.Mass, dripRate * dt);
			if (massToDrip <= 0f) return;

			Tag prefabTag = firstPrimaryElement.GetComponent<KPrefabID>().PrefabTag;
			storage.ConsumeAndGetDisease(prefabTag, massToDrip, out float mass, out SimUtil.DiseaseInfo diseaseInfo, out float temperature);

			if (carpetBelow != null)
			{
				carpetBelow.storage.AddLiquid(element.id, mass, temperature, diseaseInfo.idx, diseaseInfo.count);
			}
			else
			{
				FallingWater.instance.AddParticle(myCell, elementIndex, mass, temperature, diseaseInfo.idx, diseaseInfo.count, true, false, false, false);
			}
		}
	}
}
