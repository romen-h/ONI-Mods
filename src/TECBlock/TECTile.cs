using UnityEngine;

namespace RomenH.TECBlock
{
	public class TECTile : KMonoBehaviour, ISim200ms, ISim33ms
	{
		[MyCmpGet]
		public Building building;

		[MyCmpGet]
		public Operational operational;

		[MyCmpGet]
		public EnergyConsumer energyConsumer;

		[MyCmpGet]
		public KBatchedAnimController anim;

		private int coldCell;
		private int hotCell;

		public float maxTempDiff = 70f;
		public float kDTUsPerWatt = 0.1f;
		public float efficiency = 0.25f;

		private float heatPowerkDTU;
		private float maxHeatMovedkDTU;
		private float wasteHeatkDTU;

		private float heatMovedPerDegree;

		private static readonly Color32 coldColor = new Color32(153, 238, 255, 255);
		private static readonly Color32 hotColor = new Color32(255, 160, 160, 255);
		private static readonly Color32 whiteColor = new Color32(255, 255, 255, 255);

		private bool isMovingHeat = false;

		protected override void OnSpawn()
		{
			base.OnSpawn();

			heatPowerkDTU = energyConsumer.BaseWattageRating * kDTUsPerWatt;
			if (efficiency < 1f)
			{
				maxHeatMovedkDTU = heatPowerkDTU * efficiency;
			}
			else
			{
				maxHeatMovedkDTU = heatPowerkDTU;
			}
			wasteHeatkDTU = 0f;

			heatMovedPerDegree = maxHeatMovedkDTU / maxTempDiff;

			var hotOffset = building.GetRotatedOffset(new CellOffset(0, 1));
			hotCell = Grid.OffsetCell(building.GetCell(), hotOffset);

			var coldOffset = building.GetRotatedOffset(new CellOffset(0, -1));
			coldCell = Grid.OffsetCell(building.GetCell(), coldOffset);
		}

		public void Sim200ms(float dt)
		{
			operational.SetActive(operational.IsOperational);

			if (operational.IsActive)
			{
				if (anim.currentAnim != "on")
				{
					anim.Play("on", KAnim.PlayMode.Paused);
				}

				float coldTemp = Grid.Temperature[coldCell];
				float hotTemp = Grid.Temperature[hotCell];

				float dTemp = Mathf.Clamp(hotTemp - coldTemp, 0f, maxTempDiff);

				float heatMoved = maxHeatMovedkDTU - (heatMovedPerDegree * dTemp);
				heatMoved = Mathf.Clamp(heatMoved, 0, maxHeatMovedkDTU);

				float heatInColdCell = (coldTemp * Grid.Mass[coldCell] * Grid.Element[coldCell].specificHeatCapacity);
				if (heatMoved > heatInColdCell)
				{
					heatMoved = heatInColdCell - 1;
				}

				if (heatMoved > 0)
				{
					isMovingHeat = true;
					float heatProduced = heatMoved + wasteHeatkDTU;
					SimMessages.ModifyEnergy(coldCell, -heatMoved * 1000f, 5000f, SimMessages.EnergySourceID.StructureTemperature);
					SimMessages.ModifyEnergy(hotCell, heatProduced * 1000f, 5000f, SimMessages.EnergySourceID.StructureTemperature);
				}
				else
				{
					isMovingHeat = false;
				}
			}
			else
			{
				isMovingHeat = false;

				if (anim.currentAnim != "off")
				{
					anim.Play("off");
				}
			}
		}

		public void Sim33ms(float dt)
		{
			if (operational.IsActive && isMovingHeat)
			{
				float t = 0.5f + 0.5f * Mathf.Sin(Time.time * Mathf.PI / 2f);
				Color32 hotGlow = Color32.Lerp(whiteColor, hotColor, t);
				Color32 coldGlow = Color32.Lerp(coldColor, whiteColor, t);

				anim.SetSymbolTint("plate", coldGlow);
				anim.SetSymbolTint("fins", hotGlow);
			}
			else
			{
				anim.SetSymbolTint("plate", whiteColor);
				anim.SetSymbolTint("fins", whiteColor);
			}
		}
	}
}
