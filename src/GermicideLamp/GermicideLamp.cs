using Klei.AI;

using KSerialization;

using STRINGS;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace RomenMods.GermicideLampMod
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class GermicideLamp : KMonoBehaviour, ISim200ms
	{
		protected static byte[] diseasesKilled = null;

		public bool FlashReachesCell(int cellX, int cellY)
		{
			return Grid.TestLineOfSight(cellX, cellY, lampXY.X, lampXY.Y + 1, Grid.VisibleBlockingCB, true);
		}

		private int GermsToKill(int count)
		{
			return (int)Mathf.Clamp(count * strength, 0, count);
		}

		[MyCmpReq]
		public Operational operational;

		private Vector2I lampXY;

		public int aoeLeft = 0;
		public int aoeWidth = 1;
		public int aoeBottom = 0;
		public int aoeHeight = 1;
		public bool applySunburn = false;
		public float strength = 0f;

		protected override void OnSpawn()
		{
			base.OnSpawn();

			if (diseasesKilled == null)
			{
				diseasesKilled = new byte[] {
					Db.Get().Diseases.GetIndex(Db.Get().Diseases.FoodGerms.id),
					Db.Get().Diseases.GetIndex(Db.Get().Diseases.SlimeGerms.id)
				};
			}

			lampXY = Grid.PosToXY(gameObject.transform.position);
		}

		public void Sim200ms(float dt)
		{
			if (operational.IsOperational)
			{
				HashSet<GameObject> buildingsAlreadySeen = new HashSet<GameObject>();

				// Delete germs in area
				for (int dy = aoeBottom; dy < aoeBottom + aoeHeight; dy++)
				{
					int y = lampXY.Y + dy;
					for (int dx = aoeLeft; dx < aoeLeft + aoeWidth; dx++)
					{
						int x = lampXY.X + dx;
						if (FlashReachesCell(x, y))
						{
							int cell = Grid.XYToCell(x, y);

							// Delete germs in the cell

							byte cellGermIndex = Grid.DiseaseIdx[cell];
							int cellGermCount = Grid.DiseaseCount[cell];
							int cellGermsToKill = GermsToKill(cellGermCount);
							SimMessages.ModifyDiseaseOnCell(cell, cellGermIndex, -cellGermsToKill);

							// Delete germs on pickupables in the cell

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
										if (pickupable.PrimaryElement.DiseaseIdx != byte.MaxValue)
										{
											int pickupableGermCount = pickupable.PrimaryElement.DiseaseCount;
											int pickupableGermsToKill = GermsToKill(pickupableGermCount);
											pickupable.PrimaryElement.ModifyDiseaseCount(-pickupableGermsToKill, GermicideLampConfig.ID);
										}

										if (applySunburn)
										{
											var minion = pickupable.GetComponent<MinionIdentity>();
											if (minion != null)
											{
												var sunburn = new SicknessExposureInfo(Db.Get().Sicknesses.Sunburn.Id, ModStrings.GERMICIDELAMP.NAME);
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

							// Delete germs on solid conduit items in the cell

							var conduit = Game.Instance.solidConduitFlow.GetConduit(cell);
							if (!conduit.Equals(SolidConduitFlow.Conduit.Invalid()))
							{
								var conduitContents = conduit.GetContents(Game.Instance.solidConduitFlow);
								var conduitPickupable = Game.Instance.solidConduitFlow.GetPickupable(conduitContents.pickupableHandle);
								if (conduitPickupable != null && conduitPickupable.PrimaryElement.DiseaseIdx != byte.MaxValue)
								{
									int cpuCount = conduitPickupable.PrimaryElement.DiseaseCount;
									int cpuGermsToKill = GermsToKill(cpuCount);
									conduitPickupable.PrimaryElement.ModifyDiseaseCount(-cpuGermsToKill, GermicideLampConfig.ID);
								}
							}

							// Delete germs on buildings in the cell

							var buildingInCell = Grid.Objects[cell, (int)ObjectLayer.Building];
							if (buildingInCell != null && !buildingsAlreadySeen.Contains(buildingInCell))
							{
								var buildingElement = buildingInCell.GetComponent<PrimaryElement>();
								if (buildingElement != null && buildingElement.DiseaseIdx != byte.MaxValue)
								{
									int buildingGermCount = buildingElement.DiseaseCount;
									int buildingGermsToKill = GermsToKill(buildingGermCount);
									buildingElement.ModifyDiseaseCount(-buildingGermsToKill, GermicideLampConfig.ID);
								}

								buildingsAlreadySeen.Add(buildingInCell);
							}
						}
					}
				}
			}
		}
	}
}
