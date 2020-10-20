using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using static KAnim;

namespace RomenMods.FestiveDecorMod
{
	public class GlowInTheDark : KMonoBehaviour, ISim1000ms
	{
		[MyCmpGet]
		KBatchedAnimController myAnim;

		bool glowingEnabled = false;

		public KAnimFile noGlowAnim;
		public KAnimFile glowAnim;

		public void Sim1000ms(float dt)
		{
			if (GameClock.Instance.IsNighttime())
			{
				if (!glowingEnabled) EnableGlow();
			}
			else
			{
				if (glowingEnabled) DisableGlow();
			}
		}

		void EnableGlow()
		{
			if (glowAnim != null)
			{
				myAnim.AddAnimOverrides(glowAnim);
			}

			glowingEnabled = true;
		}

		void DisableGlow()
		{
			if (glowAnim != null)
			{
				myAnim.RemoveAnimOverrides(glowAnim);
			}

			glowingEnabled = false;
		}
	}
}
