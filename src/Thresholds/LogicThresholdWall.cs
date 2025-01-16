using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace RomenH.Thresholds
{
	public class LogicThresholdWall : KMonoBehaviour, ISim200ms
	{
		public static readonly HashedString INPUT_PORT_ID = new HashedString("LogicThresholdWallInput");

		[MyCmpReq]
		public Building building;

		[MyCmpReq]
		public KBatchedAnimController kanim;

		private static Color redColor = new Color32(255, 0, 0, 255);
		private static Color orangeColor = new Color32(255, 121, 76, 255);
		private static Color GetColor0(bool foundation, bool blocking) => blocking ? redColor : foundation ? orangeColor : Color.clear;

		private static Color yellowColor = new Color32(255, 203, 76, 255);
		private static Color greenColor = new Color32(128, 255, 0, 255);
		private static Color GetColor90(bool foundation, bool blocking) => blocking ? yellowColor : foundation ? greenColor : Color.clear;

		private static Color cyanColor = new Color32(0, 255, 255, 255);
		private static Color blueColor = new Color32(0, 53, 255, 255);
		private static Color GetColor180(bool foundation, bool blocking) => blocking ? cyanColor : foundation ? blueColor : Color.clear;

		private static Color magentaColor = new Color32(127, 61, 94, 255);
		private static Color purpleColor = new Color32(96, 0, 255, 255);
		private static Color GetColor270(bool foundation, bool blocking) => blocking ? magentaColor : foundation ? purpleColor : Color.clear;

		private static readonly byte[] rot0LUT =
		{
			 0,  1,  2,  3,  4,  5,  6,  7,
			 8,  9, 10, 11, 12, 13, 14, 15,
			16, 17, 18, 19, 20, 21, 22, 23,
			24, 25, 26, 27, 28, 29, 30, 31,
			32, 33, 34, 35, 36, 37, 38, 39,
			40, 41, 42, 43, 44, 45, 46, 47,
			48, 49, 50, 51, 52, 53, 54, 55,
			56, 57, 58, 59, 60, 61, 62, 63,
		};

		private static readonly byte[] rot90LUT =
		{
			56, 48, 40, 32, 24, 16,  8,  0,
			57, 49, 41, 33, 25, 17,  9,  1,
			58, 50, 42, 34, 26, 18, 10,  2,
			59, 51, 43, 35, 27, 19, 11,  3,
			60, 52, 44, 36, 28, 20, 12,  4,
			61, 53, 45, 37, 29, 21, 13,  5,
			62, 54, 46, 38, 30, 22, 14,  6,
			63, 55, 47, 39, 31, 23, 15,  7,
		};

		private static readonly byte[] rot180LUT =
		{
			63, 62, 61, 60, 59, 58, 57, 56,
			55, 54, 53, 52, 51, 50, 49, 48,
			47, 46, 45, 44, 43, 42, 41, 40,
			39, 38, 37, 36, 35, 34, 33, 32,
			31, 30, 29, 28, 27, 26, 25, 24,
			23, 22, 21, 20, 19, 18, 17, 16,
			15, 14, 13, 12, 11, 10,  9,  8,
			 7,  6,  5,  4,  3,  2,  1,  0,
		};

		private static readonly byte[] rot270LUT =
		{
			 7, 15, 23, 31, 39, 47, 55, 63,
			 6, 14, 22, 30, 38, 46, 54, 62,
			 5, 13, 21, 29, 37, 45, 53, 61,
			 4, 12, 20, 28, 36, 44, 52, 60,
			 3, 11, 19, 27, 35, 43, 51, 59,
			 2, 10, 18, 26, 34, 42, 50, 58,
			 1,  9, 17, 25, 33, 41, 49, 57,
			 0,  8, 16, 24, 32, 40, 48, 56,
		};

		private static byte[] GetLUT(Orientation orientation)
		{
			switch (orientation)
			{
				case Orientation.Neutral:
				default:
					return rot0LUT;

				case Orientation.R90:
					return rot90LUT;

				case Orientation.R180:
					return rot180LUT;

				case Orientation.R270:
					return rot270LUT;
			}
		}

		private static readonly byte[] blankLEDs =
		{
			0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0,
		};

		private static readonly byte[] fullLEDs =
		{
			1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1,
		};

		private static readonly byte[] stripeLEDs =
		{
			0, 0, 0, 0, 1, 1, 1, 1,
			0, 0, 0, 1, 1, 1, 1, 0,
			0, 0, 1, 1, 1, 1, 0, 0,
			0, 1, 1, 1, 1, 0, 0, 0,
			1, 1, 1, 1, 0, 0, 0, 0,
			1, 1, 1, 0, 0, 0, 0, 1,
			1, 1, 0, 0, 0, 0, 1, 1,
			1, 0, 0, 0, 0, 1, 1, 1,
		};

		private static readonly byte[] crossLEDs =
		{
			1, 1, 0, 0, 0, 0, 1, 1,
			1, 1, 1, 0, 0, 1, 1, 1,
			0, 1, 1, 1, 1, 1, 1, 0,
			0, 0, 1, 1, 1, 1, 0, 0,
			0, 0, 1, 1, 1, 1, 0, 0,
			0, 1, 1, 1, 1, 1, 1, 0,
			1, 1, 1, 0, 0, 1, 1, 1,
			1, 1, 0, 0, 0, 0, 1, 1,
		};

		private static readonly byte[] checkerLEDs =
		{
			1, 1, 1, 1, 0, 0, 0, 0,
			1, 1, 1, 1, 0, 0, 0, 0,
			1, 1, 1, 1, 0, 0, 0, 0,
			1, 1, 1, 1, 0, 0, 0, 0,
			0, 0, 0, 0, 1, 1, 1, 1,
			0, 0, 0, 0, 1, 1, 1, 1,
			0, 0, 0, 0, 1, 1, 1, 1,
			0, 0, 0, 0, 1, 1, 1, 1,
		};

		private static readonly byte[] waveLEDs =
		{
			1, 0, 0, 0, 1, 0, 0, 0,
			1, 1, 0, 1, 1, 1, 0, 1,
			0, 1, 1, 1, 0, 1, 1, 1,
			0, 0, 1, 0, 0, 0, 1, 0,
			1, 0, 0, 0, 1, 0, 0, 0,
			1, 1, 0, 1, 1, 1, 0, 1,
			0, 1, 1, 1, 0, 1, 1, 1,
			0, 0, 1, 0, 0, 0, 1, 0,
		};

		private static readonly byte[] catLEDs =
		{
			1, 0, 0, 0, 0, 0, 0, 1,
			1, 1, 0, 0, 0, 0, 1, 1,
			1, 0, 1, 1, 1, 1, 0, 1,
			1, 0, 0, 0, 0, 0, 0, 1,
			1, 1, 0, 0, 1, 0, 1, 1,
			1, 0, 0, 1, 1, 1, 0, 1,
			1, 0, 0, 0, 0, 0, 0, 1,
			0, 1, 1, 1, 1, 1, 1, 0,
		};

		private static readonly byte[] heartLEDs =
		{
			0, 0, 0, 0, 0, 0, 0, 0,
			0, 1, 1, 0, 1, 1, 0, 0,
			1, 1, 1, 1, 1, 1, 1, 0,
			1, 1, 1, 1, 1, 1, 1, 0,
			0, 1, 1, 1, 1, 1, 0, 0,
			0, 0, 1, 1, 1, 0, 0, 0,
			0, 0, 0, 1, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0,
		};

		private static readonly byte[] faceLEDs =
		{
			0, 1, 1, 1, 1, 1, 0, 0,
			1, 0, 0, 0, 0, 0, 1, 0,
			1, 0, 1, 0, 1, 0, 1, 0,
			1, 0, 0, 0, 0, 0, 1, 0,
			1, 0, 1, 1, 1, 0, 1, 0,
			1, 0, 0, 0, 0, 0, 1, 0,
			0, 1, 1, 1, 1, 1, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0,
		};

		private static readonly byte[] amongusLEDs =
		{
			0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 1, 1, 1, 1, 1, 0,
			0, 1, 1, 1, 0, 0, 1, 0,
			0, 1, 1, 1, 1, 1, 1, 0,
			0, 1, 1, 1, 1, 1, 1, 0,
			0, 0, 1, 1, 1, 1, 1, 0,
			0, 0, 1, 1, 0, 1, 1, 0,
			0, 0, 0, 0, 0, 0, 0, 0,
		};

		private static byte[] GetPattern(int index)
		{
			switch (index)
			{
				case 0:
					return stripeLEDs;

				case 1:
					return crossLEDs;

				case 2:
					return checkerLEDs;

				case 3:
					return waveLEDs;

				case 4:
					return catLEDs;

				case 5:
					return heartLEDs;

				case 6:
					return faceLEDs;

				case 7:
					return amongusLEDs;

				default:
					return fullLEDs;
			}
		}

		private Color GetColor(bool foundation, bool blocking)
		{
			switch (building.Orientation)
			{
				case Orientation.Neutral:
					return GetColor0(foundation, blocking);

				case Orientation.R90:
					return GetColor90(foundation, blocking);

				case Orientation.R180:
					return GetColor180(foundation, blocking);

				case Orientation.R270:
					return GetColor270(foundation, blocking);

				default:
					return Color.clear;
			}
		}

		private string LEDSymbol(int i)
		{
			return $"led{i:D2}";
		}

		public void SetLED(int i, bool visible, Color color)
		{
			byte[] lut = GetLUT(building.Orientation);

			int j = lut[i];

			string symbol = LEDSymbol(j);
			kanim.SetSymbolVisiblity(symbol, visible);
			if (visible)
			{
				kanim.SetSymbolTint(symbol, color);
			}
		}

		private bool setFoundation = false;
		private bool setBlocking = false;
		private int currentPattern = 0;
		private bool doScroll = false;
		private bool altScroll = false;

		public void UpdateFunction(bool foundation, bool blocking)
		{
			int cell = Grid.PosToCell(this);

			// Toggle threshold
			if (setFoundation != foundation)
			{
				setFoundation = foundation;
				Grid.Foundation[cell] = setFoundation;
				Game.Instance.roomProber.SolidChangedEvent(cell, false);
			}

			// Toggle pathing
			if (setBlocking != blocking)
			{
				setBlocking = blocking;
				Grid.DupeImpassable[cell] = setBlocking;
				Pathfinding.Instance.AddDirtyNavGridCell(cell);
			}
		}

		public void UpdateLEDs()
		{
			byte[] pattern = GetPattern(currentPattern);

			Color color = GetColor(setFoundation, setBlocking);
			bool on = color.maxColorComponent > 0;

			for (int i = 0; i < 64; i++)
			{
				int j = i;
				if (doScroll)
				{
					int dy = 8 * counter;
					if (altScroll)
					{
						j = i - dy;
					}
					else
					{
						j = i + dy;
					}
				}

				if (j < 0) j += 64;
				if (j >= 64) j -= 64;

				bool enabled = on && pattern[j] > 0;
				SetLED(i, enabled, color);
			}
		}

		public override void OnSpawn()
		{
			base.Subscribe<LogicThresholdWall>(-801688580, OnLogicValueChangedDelegate);

			UpdateFunction(true, true);
			UpdateLEDs();
		}

		public void OnLogicValueChanged(object data)
		{
			LogicValueChanged logicValueChanged = (LogicValueChanged)data;
			if (logicValueChanged.portID != INPUT_PORT_ID) return;

			bool foundation = LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue); // 1 bit for foundation toggle
			bool blockPathing = LogicCircuitNetwork.IsBitActive(1, logicValueChanged.newValue); // 1 bit for pathing toggle
			UpdateFunction(foundation, blockPathing);

			currentPattern = (logicValueChanged.newValue >> 2) & 0x07; // last 2 bits + 1 hidden bit for patterns
			doScroll = (logicValueChanged.newValue & (1 << 5)) > 0; // 1 bit for scrolling effect
			altScroll = (logicValueChanged.newValue & (1 << 6)) > 0; // 1 bit for reverse scrolling effect

			UpdateLEDs();
		}

		private static int counter = 0;
		private static bool updatedThisFrame = false;

		public void Sim200ms(float dt)
		{
			if (!updatedThisFrame)
			{
				counter++;
				if (counter >= 8)
				{
					counter = 0;
				}
				updatedThisFrame = true;
			}
			
			UpdateLEDs();
		}

		private void LateUpdate()
		{
			updatedThisFrame = false;
		}

		private static readonly EventSystem.IntraObjectHandler<LogicThresholdWall> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicThresholdWall>(delegate (LogicThresholdWall component, object data)
		{
			component.OnLogicValueChanged(data);
		});
	}
}
