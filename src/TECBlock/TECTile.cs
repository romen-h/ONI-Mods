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

		public float minColdTemp = 50f;
		public float maxTempDiff = 70f;
		public float kDTUsPerWatt = 0.1f;
		public float efficiency = 0.25f;

		private float heatPowerkDTU;
		private float maxHeatMovedkDTU;

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
				float heatMoved = 0f;

				// Only move heat if over min temp and under max temp difference
				if (coldTemp > minColdTemp && dTemp < maxTempDiff)
				{
					// Heat to move is negatively proportional to the temp difference
					heatMoved = maxHeatMovedkDTU - (heatMovedPerDegree * dTemp);
					heatMoved = Mathf.Clamp(heatMoved, 0, maxHeatMovedkDTU) * dt;

					// Clamp heat to move if it's more energy than the cold side has
					float heatInColdCell = (coldTemp * Grid.Mass[coldCell] * Grid.Element[coldCell].specificHeatCapacity);
					if (heatMoved > heatInColdCell)
					{
						heatMoved = heatInColdCell - 1;
					}
				}

				if (heatMoved > 0)
				{
					isMovingHeat = true;
					SimMessages.ModifyEnergy(coldCell, -heatMoved * 1000f, 5000f, SimMessages.EnergySourceID.StructureTemperature);
					SimMessages.ModifyEnergy(hotCell, heatMoved * 1000f, 5000f, SimMessages.EnergySourceID.StructureTemperature);
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
				Color hotGlow = Color.Lerp(whiteColor, hotColor, t);
				Color coldGlow = Color.Lerp(coldColor, whiteColor, t);

				anim.SetSymbolTint("plate", coldGlow * 2);
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
