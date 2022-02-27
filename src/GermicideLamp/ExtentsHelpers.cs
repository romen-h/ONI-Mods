namespace RomenH.GermicideLamp
{
	public static class ExtentsHelpers
	{
		public static void CenteredUVExtents(int range, int sourceWidth, int sourceHeight, out int left, out int width, out int bottom, out int height)
		{
			width = 2 * range + sourceWidth;
			height = 2 * range + sourceHeight;
			left = (sourceWidth / 2) - (width / 2);
			bottom = (sourceHeight / 2) - (height / 2);
		}

		public static void FloorUVExtents(int rangeWidth, int rangeHeight, out int left, out int width, out int bottom, out int height)
		{
			width = rangeWidth;
			height = rangeHeight;
			left = 1 - (width / 2);
			bottom = 0;
		}

		public static void CeilingUVExtents(int rangeWidth, int rangeHeight, out int left, out int width, out int bottom, out int height)
		{
			width = rangeWidth;
			height = rangeHeight;
			left = 1 - (width / 2);
			bottom = 1 - height;
		}
	}
}
