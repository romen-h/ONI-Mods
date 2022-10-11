using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomenH.Common
{
	public static class Probability
	{
		/// <summary>
		/// Returns the probability value for "x in 100" odds.
		/// </summary>
		public static float PercentChance(float percent) => percent / 100f;

		/// <summary>
		/// Returns the probability for "1 in X" odds.
		/// </summary>
		public static float OneInXChance(int outcomes) => 1f / (float)outcomes;

		/// <summary>
		/// Returns the probability for "x in y" odds. (Rolling a dice)
		/// </summary>
		public static float DiceChance(int winningSides, int totalSides) => (float)winningSides / (float)totalSides;

		/// <summary>
		/// Returnes the aggregate probability of either probability succeeding.
		/// </summary>
		public static float Either(float chance1, float chance2) => chance1 + chance2;

		/// <summary>
		/// Returns the aggregate probability of both probabilities succeeding.
		/// </summary>
		public static float Both(float chance1, float chance2) => chance1 * chance2;

		/// <summary>
		/// Returns true or false with 50/50 odds.
		/// </summary>
		public static bool FlipCoin()
		{
			return UnityEngine.Random.value < 0.5f;
		}

		/// <summary>
		/// Returns the value of an N-sided dice roll.
		/// </summary>
		public static int RollDice(int sides)
		{
			return (int)UnityEngine.Random.Range(1, sides+1);
		}

		/// <summary>
		/// Rolls an N-sided die and returns true if the result was at least the given value.
		/// </summary>
		public static bool TestDice(int atLeast, int sides)
		{
			if (atLeast <= 0) return true;
			if (atLeast > sides) return false;
			return RollDice(sides) >= atLeast;
		}

		/// <summary>
		/// Gets a random number and returns true if the given probability is greater.
		/// </summary>
		public static bool Test(float chance)
		{
			float roll = UnityEngine.Random.value;
			return (roll <= chance);
		}

		/// <summary>
		/// Tests the given probabilities and returns true if any of them succeeded.
		/// </summary>
		public static bool TestAny(params float[] chances)
		{
			for (int i = 0; i < chances.Length; i++)
			{
				bool r = Test(chances[i]);
				if (r) return true;
			}

			return false;
		}

		/// <summary>
		/// Tests the given probabilities and returns true if all of them succeeded.
		/// </summary>
		public static bool TestAll(params float[] chances)
		{
			for (int i = 0; i < chances.Length; i++)
			{
				bool r = Test(chances[i]);
				if (!r) return false;
			}

			return true;
		}
	}
}
