using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Klei.AI;

using KSerialization;

using UnityEngine;

namespace RomenH.GermicideLamp
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class GermicideLamp : KMonoBehaviour, ISim200ms
	{
		public bool alwaysOn = false;
		public bool isMobile = false;
		public bool applySunburn = false;
		public bool flicker = false;
		public int aoeLeft = 0;
		public int aoeWidth = 1;
		public int aoeBottom = 0;
		public int aoeHeight = 1;
		public float basePower = 1.0f;
		
		private static readonly double b = Math.Log(0.5);

		private static int GermsToKill(byte germIndex, int count, float uvPower)
		{
			if (DiseaseHalfLife.GetEnabled(germIndex))
			{
				if (count == 1) return 1;

				return count - GermsRemaining(count, germIndex, uvPower);
			}

			return 0;
		}

		private static int GermsRemaining(int initial, int germIndex, float uvPower)
		{
			float e = DiseaseHalfLife.GetExponent(germIndex);
			return (int)(initial * Math.Exp(b * e * uvPower));
		}

		public bool FlashReachesCell(int cellX, int cellY)
		{
			return Grid.TestLineOfSight(cellX, cellY, lampXY.X, lampXY.Y + 1, Grid.VisibleBlockingCB, true);
		}

		[MyCmpGet]
		public Operational operational;

		[MyCmpGet]
		public Light2D light;

		private Vector2I lampXY;

		protected override void OnSpawn()
		{
			base.OnSpawn();

			lampXY = Grid.PosToXY(gameObject.transform.position);
		}

		public void Update()
		{
			if (flicker && light != null)
			{
				float intensity = Mathf.Sin(100f * Time.time) * 0.025f + 0.975f;
				light.IntensityAnimation = intensity;
			}
		}

		public void LateUpdate()
		{
			DiseaseHalfLife.MarkExponentsDirty();
		}

		public void Sim200ms(float dt)
		{
			DiseaseHalfLife.PrecomputeExponents();

			if (isMobile)
			{
				lampXY = Grid.PosToXY(gameObject.transform.position);
			}

			if (alwaysOn || (operational != null && operational.IsOperational))
			{
				if (operational != null)
				{
					operational.SetActive(true);
				}

				HashSet<GameObject> buildingsAlreadySeen = new HashSet<GameObject>();

				// Delete germs in area
				for (int dy = aoeBottom; dy < aoeBottom + aoeHeight; dy++)
				{
					int y = lampXY.Y + dy;
					for (int dx = aoeLeft; dx < aoeLeft + aoeWidth; dx++)
					{
						int x = lampXY.X + dx;
						UpdateCell(dt, x, y, buildingsAlreadySeen);
					}
				}
			}
			else
			{
				if (operational != null)
				{
					operational.SetActive(false);
				}
			}
		}

		private void UpdateCell(float dt, int x, int y, HashSet<GameObject> buildingsAlreadySeen)
		{
			if (FlashReachesCell(x, y))
			{
				int cell = Grid.XYToCell(x, y);

				float uvPower = basePower;

				if (Grid.IsLiquid(cell))
					uvPower *= ModSettings.Instance.LiquidUVAttenuation;

				KillGermsInCell(cell, uvPower);
				KillGermsOnPickupable(cell, uvPower);
				KillGermsOnConduits(cell, uvPower);
				KillGermsOnBuildings(cell, buildingsAlreadySeen, uvPower);
			}
		}

		private void KillGermsInCell(int cell, float uvPower)
		{
			int cellGermCount = Grid.DiseaseCount[cell];
			if (cellGermCount > 0)
			{
				byte germID = Grid.DiseaseIdx[cell];
				int cellGermsToKill = GermsToKill(germID, cellGermCount, uvPower);
				SimMessages.ModifyDiseaseOnCell(cell, germID, -cellGermsToKill);
			}
		}

		private void KillGermsOnPickupable(int cell, float uvPower)
		{
			var pickupablesInCell = Grid.Objects[cell, (int)ObjectLayer.Pickupables];
			if (pickupablesInCell != null)
			{
				var currentPickupable = pickupablesInCell.GetComponent<Pickupable>().objectLayerListItem;
				while (currentPickupable != null)
				{
					var pickupable = currentPickupable.gameObject.GetComponent<Pickupable>();
					currentPickupable = currentPickupable.nextItem;

					if (pickupable != null)
					{
						byte germID = pickupable.PrimaryElement.DiseaseIdx;
						int pickupableGermCount = pickupable.PrimaryElement.DiseaseCount;
						int pickupableGermsToKill = GermsToKill(germID, pickupableGermCount, uvPower);
						pickupable.PrimaryElement.ModifyDiseaseCount(-pickupableGermsToKill, GermicideLampConfig.ID);

						if (applySunburn)
						{
							var minion = pickupable.GetComponent<MinionIdentity>();
							if (minion != null)
							{
								// Check if wearing suit
								bool noSuit = true;
								Navigator minionNav = minion.GetComponent<Navigator>();
								if (minionNav != null)
								{
									noSuit = (minionNav.flags & PathFinder.PotentialPath.Flags.HasAtmoSuit) == 0;
								}

								if (noSuit)
								{
									var sunburn = new SicknessExposureInfo(Db.Get().Sicknesses.Sunburn.Id, GermicideLampConfig.Name.ToString());
									var sicknesses = minion.GetSicknesses();

									bool hasSunburn = false;

									if (sicknesses.IsInfected())
									{
										foreach (SicknessInstance item in sicknesses)
										{
											if (item.ExposureInfo.sicknessID == Db.Get().Sicknesses.Sunburn.Id)
											{
												hasSunburn = true;
												break;
											}
										}
									}

									if (!hasSunburn) sicknesses.Infect(sunburn);
								}
							}
						}
					}
				}
			}
		}

		private void KillGermsOnConduits(int cell, float uvPower)
		{
			var conduit = Game.Instance.solidConduitFlow.GetConduit(cell);
			if (!conduit.Equals(SolidConduitFlow.Conduit.Invalid()))
			{
				var conduitContents = conduit.GetContents(Game.Instance.solidConduitFlow);
				var conduitPickupable = Game.Instance.solidConduitFlow.GetPickupable(conduitContents.pickupableHandle);
				if (conduitPickupable != null)
				{
					byte germID = conduitPickupable.PrimaryElement.DiseaseIdx;
					int conduitGermCount = conduitPickupable.PrimaryElement.DiseaseCount;
					int cpuGermsToKill = GermsToKill(germID, conduitGermCount, uvPower);
					conduitPickupable.PrimaryElement.ModifyDiseaseCount(-cpuGermsToKill, GermicideLampConfig.ID);
				}
			}
		}

		private void KillGermsOnBuildings(int cell, HashSet<GameObject> buildingsAlreadySeen, float uvPower)
		{
			var buildingInCell = Grid.Objects[cell, (int)ObjectLayer.Building];
			if (buildingInCell != null && !buildingsAlreadySeen.Contains(buildingInCell))
			{
				var buildingElement = buildingInCell.GetComponent<PrimaryElement>();
				if (buildingElement != null)
				{
					byte germID = buildingElement.DiseaseIdx;
					int buildingGermCount = buildingElement.DiseaseCount;
					int buildingGermsToKill = GermsToKill(germID, buildingGermCount, uvPower);
					buildingElement.ModifyDiseaseCount(-buildingGermsToKill, GermicideLampConfig.ID);
				}

				buildingsAlreadySeen.Add(buildingInCell);
			}
		}
	}
}
