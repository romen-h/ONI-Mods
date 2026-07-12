using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RomenH.Common
{
	public static class LightBuilder
	{
		public static Light2D SetStats(this Light2D light, int lux, float range)
		{
			light.Lux = lux;
			light.Range = range;
			return light;
		}

		public static Light2D SetNoGlow(this Light2D light, LightShape? raysShape = null)
		{
			light.Color = Color.clear;
			if (raysShape != null) light.shape = raysShape.Value;
			return light;
		}

		public static Light2D SetCircularGlow(this Light2D light, Color? color = null)
		{
			light.shape = LightShape.Circle;
			light.Angle = 0;
			light.Direction = Vector2.down;
			light.Color = color ?? Color.white;
			return light;
		}

		public static Light2D SetDirectedGlow(this Light2D light, float directionDegrees, float spreadDegrees, Color? color = null)
		{
			light.shape = LightShape.Cone;
			light.Angle = spreadDegrees * Mathf.Deg2Rad;
			light.Direction = new Vector2(Mathf.Cos(directionDegrees * Mathf.Deg2Rad), Mathf.Sin(directionDegrees * Mathf.Deg2Rad));
			light.Color = color ?? Color.white;
			return light;
		}

		public static Light2D SetRays(this Light2D light, bool enabled, Color? color = null)
		{
			light.drawOverlay = enabled;
			light.overlayColour = color ?? Color.white;
			return light;
		}
	}
}
