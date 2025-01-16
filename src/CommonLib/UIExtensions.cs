using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static StateMachine;

using Object = System.Object;

namespace RomenH.Common
{
	public enum UISide
	{
		Left,
		Top,
		Right,
		Bottom
	}

	public static class UIAnchors
	{
		public static readonly Vector2 TopLeft = new Vector2(0f, 1f);
		public static readonly Vector2 TopCenter = new Vector2(0.5f, 1f);
		public static readonly Vector2 TopRight = new Vector2(1f, 1f);

		public static readonly Vector2 MiddleLeft = new Vector2(0f, 0.5f);
		public static readonly Vector2 MiddleCenter = new Vector2(0.5f, 0.5f);
		public static readonly Vector2 MiddleRight = new Vector2(1f, 0.5f);

		public static readonly Vector2 BottomLeft = new Vector2(0f, 0f);
		public static readonly Vector2 BottomCenter = new Vector2(0.5f, 0f);
		public static readonly Vector2 BottomRight = new Vector2(1f, 0f);
	}

	public static class UIExtensions
	{
		public static bool IsStretchingX(this RectTransform rectTransform)
		{
			if (rectTransform == null) return false;
			return (rectTransform.anchorMin.x != rectTransform.anchorMax.x);
		}

		public static bool IsStretchingY(this RectTransform rectTransform)
		{
			if (rectTransform == null) return false;
			return (rectTransform.anchorMin.y != rectTransform.anchorMax.y);
		}

		public static void SetPivot(this RectTransform rectTransform, Vector2 pivot)
		{
			if (rectTransform == null) return;
			rectTransform.pivot = pivot;

			LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
		}

		public static void SetAnchor(this RectTransform rectTransform, Vector2 anchor)
		{
			if (rectTransform == null) return;
			rectTransform.anchorMin = anchor;
			rectTransform.anchorMax = anchor;

			LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
		}

		public static void SetStretchHorizontal(this RectTransform rectTransform)
		{
			if (rectTransform == null) return;

			rectTransform.anchorMin = new Vector2(0f, rectTransform.anchorMin.y);
			rectTransform.anchorMax = new Vector2(1f, rectTransform.anchorMax.y);
			rectTransform.sizeDelta = new Vector2(0, rectTransform.sizeDelta.y);

			LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
		}

		public static void SetStretchVertical(this RectTransform rectTransform)
		{
			if (rectTransform == null) return;

			rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, 0f);
			rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, 1f);
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 0f);

			LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
		}

		public static void SetStretchToParent(this RectTransform rectTransform)
		{
			if (rectTransform == null) return;

			rectTransform.anchoredPosition = Vector2.zero;
			rectTransform.pivot = UIAnchors.MiddleCenter;
			rectTransform.anchorMin = UIAnchors.BottomLeft;
			rectTransform.anchorMax = UIAnchors.TopRight;
			rectTransform.sizeDelta = Vector2.zero;

			LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
		}

		public static void SetWidth(this RectTransform rectTransform, float width, UISide preferSide = UISide.Left)
		{
			if (rectTransform == null) return;
			switch (preferSide)
			{
				case UISide.Left:
					rectTransform.anchorMax = new Vector2(rectTransform.anchorMin.x, rectTransform.anchorMax.y);
					rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
					break;

				case UISide.Right:
					rectTransform.anchorMin = new Vector2(rectTransform.anchorMax.x, rectTransform.anchorMin.y);
					rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
					break;

				default:
					throw new ArgumentException("preferSide must be Left or Right", nameof(preferSide));
			}

			LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
		}

		public static void SetHeight(this RectTransform rectTransform, float height, UISide preferSide = UISide.Top)
		{
			if (rectTransform == null) return;
			switch (preferSide)
			{
				case UISide.Top:
					rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, rectTransform.anchorMin.y);
					rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
					break;

				case UISide.Bottom:
					rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, rectTransform.anchorMax.y);
					rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
					break;

				default:
					throw new ArgumentException("preferSide must be Left or Right", nameof(preferSide));
			}

			LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
		}

		public static void SetSize(this RectTransform rectTransform, float width, float height)
		{
			if (rectTransform == null) return;
			if (rectTransform.anchorMin == rectTransform.anchorMax)
			{
				rectTransform.sizeDelta = new Vector2(width, height);
			}
		}

		internal static void SetParentAndAlign(GameObject child, GameObject parent)
		{
			if ((UnityEngine.Object)parent == (UnityEngine.Object)null) return;
			child.transform.SetParent(parent.transform, false);
			SetLayerRecursively(child, parent.layer);
		}

		internal static void SetLayerRecursively(GameObject go, int layer)
		{
			go.layer = layer;
			Transform transform = go.transform;
			for (int index = 0; index < transform.childCount; ++index)
			{
				SetLayerRecursively(transform.GetChild(index).gameObject, layer);
			}
		}
	}
}
