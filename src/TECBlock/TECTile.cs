using KSerialization;

using UnityEngine;

namespace RomenH.TECBlock
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class TECTile : KMonoBehaviour, ISim200ms, ISim33ms
	{
		private static readonly Color32 coldColor = new Color32(153, 238, 255, 255);
		private static readonly Color32 hotColor = new Color32(255, 160, 160, 255);
		private static readonly Color32 whiteColor = new Color32(255, 255, 255, 255);

		[SerializeField]
		public float minColdTemp = 50f;

		[SerializeField]
		public float maxTempDiff = 70f;

		[SerializeField]
		public float kDTUsPerWatt = 0.1f;

		[SerializeField]
		public float efficiency = 0.9f;

		[SerializeField]
		public float wasteHeat = 0f;

		[MyCmpGet]
		private Building building;

		[MyCmpGet]
		private Operational operational;

		[MyCmpGet]
		private EnergyConsumer energyConsumer;

		[MyCmpGet]
		private KBatchedAnimController anim;

		private bool selfHeating = false;
		private int myCell;
		private int coldCell;
		private int hotCell;
		private float heatPowerkDTU;
		private float maxHeatMovedkDTU;
		private float heatMovedPerDegree;
		private bool isMovingHeat = false;

		public override void OnSpawn()
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

			myCell = building.GetCell();

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
				float heatMoved = 0f;

				float coldMass = Grid.Mass[coldCell];
				float hotMass = Grid.Mass[hotCell];

				if (coldMass > 0f)
				{
					isMovingHeat = true;

					if (anim.currentAnim != "on")
					{
						anim.Play("on", KAnim.PlayMode.Paused);
					}

					float coldTemp = Grid.Temperature[coldCell];
					float hotTemp = Grid.Temperature[hotCell];
					float dTemp = Mathf.Clamp(hotTemp - coldTemp, 0f, maxTempDiff);

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

					SimMessages.ModifyEnergy(coldCell, -heatMoved, 9000f, SimMessages.EnergySourceID.StructureTemperature);

					float heatProduced = heatMoved + wasteHeat;
					if (hotMass == 0)
					{
						// If the hot side is vacuum then we dump the heat into the building
						selfHeating = true;
						SimMessages.ModifyEnergy(myCell, heatProduced, 9000f, SimMessages.EnergySourceID.StructureTemperature);
					}
					else
					{
						selfHeating = false;
						SimMessages.ModifyEnergy(hotCell, heatProduced, 9000f, SimMessages.EnergySourceID.StructureTemperature);
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
				if (selfHeating)
				{
					anim.SetSymbolTint("base", hotGlow);
				}
				else
				{
					anim.SetSymbolTint("base", whiteColor);
				}
			}
			else
			{
				anim.SetSymbolTint("plate", whiteColor);
				anim.SetSymbolTint("fins", whiteColor);
				anim.SetSymbolTint("base", whiteColor);
			}
		}
	}
}
