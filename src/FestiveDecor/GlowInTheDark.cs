namespace RomenH.FestiveDecor
{
	public class GlowInTheDark : KMonoBehaviour, ISim1000ms
	{
		[MyCmpReq]
		KBatchedAnimController myAnim;

		bool glowingEnabled = false;

		public KAnimFile noGlowAnim;
		public KAnimFile glowAnim;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			myAnim.usingNewSymbolOverrideSystem = true;
			gameObject.AddComponent<SymbolOverrideController>();
		}

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
