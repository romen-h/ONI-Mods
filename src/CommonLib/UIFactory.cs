using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.DefaultControls;

using Image = UnityEngine.UI.Image;

namespace RomenH.Common
{
	public static class UIFactory
	{
		public static readonly Color InactiveGray = new Color(0.8f, 0.8f, 0.8f);

		public static readonly Color Blue = new Color32(62, 67, 87, 255);
		public static readonly Color LightBlue = new Color32(128, 138, 178, 255);

		public static readonly Color Pink = new Color(0.617647052f, 0.331531137f, 0.4745891f);

		private static GameObject CreateUIElementRoot(string name, Vector2 size, params System.Type[] components)
		{
			GameObject gameObject = DefaultControls.factory.CreateGameObject(name, components);
			gameObject.GetComponent<RectTransform>().sizeDelta = size;
			return gameObject;
		}

		private static GameObject CreateUIObject(string name, GameObject parent, params System.Type[] components)
		{
			GameObject gameObject = DefaultControls.factory.CreateGameObject(name, components);
			UIExtensions.SetParentAndAlign(gameObject, parent);
			return gameObject;
		}

		private static void SetDefaultTextValues(Text lbl)
		{
			lbl.color = new Color(0.196078435f, 0.196078435f, 0.196078435f, 1f);
			lbl.font = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");
		}

		public static GameObject MakeLabel(string str)
		{
			var labelObj = DefaultControls.CreateText(new DefaultControls.Resources());
			var text = labelObj.GetComponent<Text>();
			text.text = str;

			labelObj.SetActive(true);

			return labelObj;
		}

		public static GameObject MakeButton(string text, UnityAction onClick)
		{
			return null;
		}

		public static GameObject MakeDropdown(UnityAction<int> onClick)
		{
			Font font = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");

			DefaultControls.Resources resources = new DefaultControls.Resources();
			GameObject uiElementRoot = CreateUIElementRoot("Dropdown", new Vector2(160f, 40f), typeof(Image), typeof(Dropdown));
			GameObject labelObj = CreateUIObject("Label", uiElementRoot, typeof(Text));
			GameObject arrowObj = CreateUIObject("Arrow", uiElementRoot, typeof(Image));
			GameObject templateObj = CreateUIObject("Template", uiElementRoot, typeof(Image), typeof(ScrollRect));
			GameObject viewportObj = CreateUIObject("Viewport", templateObj, typeof(Image), typeof(Mask));
			GameObject contentObj = CreateUIObject("Content", viewportObj, typeof(RectTransform));
			GameObject itemObj = CreateUIObject("Item", contentObj, typeof(Toggle));
			GameObject itemBackgroundObj = CreateUIObject("Item Background", itemObj, typeof(Image));
			//GameObject itemCheckmarkObj = CreateUIObject("Item Checkmark", itemObj, typeof(Image));
			GameObject itemLabelObj = CreateUIObject("Item Label", itemObj, typeof(Text));

			GameObject scrollbarObj = MakeScrollbar();
			scrollbarObj.name = "Scrollbar";
			UIExtensions.SetParentAndAlign(scrollbarObj, templateObj);
			Scrollbar scrollbar = scrollbarObj.GetComponent<Scrollbar>();
			scrollbar.SetDirection(Scrollbar.Direction.BottomToTop, true);
			RectTransform scrollbarTransform = scrollbarObj.GetComponent<RectTransform>();
			scrollbarTransform.anchorMin = Vector2.right;
			scrollbarTransform.anchorMax = Vector2.one;
			scrollbarTransform.pivot = Vector2.one;
			scrollbarTransform.sizeDelta = new Vector2(scrollbarTransform.sizeDelta.x, 0.0f);

			Text itemLabelText = itemLabelObj.GetComponent<Text>();
			itemLabelText.font = font;
			itemLabelText.fontSize = 14;
			itemLabelText.color = Color.white;
			itemLabelText.alignment = TextAnchor.MiddleLeft;

			Image itemBackgroundImage = itemBackgroundObj.GetComponent<Image>();
			itemBackgroundImage.color = Blue;

			//Image itemCheckmarkImage = itemCheckmarkObj.GetComponent<Image>();
			//itemCheckmarkImage.sprite = resources.checkmark;

			Toggle itemToggle = itemObj.GetComponent<Toggle>();
			itemToggle.targetGraphic = (Graphic)itemBackgroundImage;
			//itemToggle.graphic = (Graphic)itemCheckmarkImage;
			itemToggle.colors = itemToggle.colors with
			{
				highlightedColor = LightBlue
			};
			itemToggle.isOn = true;

			Image templateImage = templateObj.GetComponent<Image>();
			templateImage.sprite = resources.standard;
			templateImage.type = Image.Type.Sliced;

			ScrollRect templateScrollRect = templateObj.GetComponent<ScrollRect>();
			templateScrollRect.content = contentObj.GetComponent<RectTransform>();
			templateScrollRect.viewport = viewportObj.GetComponent<RectTransform>();
			templateScrollRect.horizontal = false;
			templateScrollRect.movementType = ScrollRect.MovementType.Clamped;
			templateScrollRect.verticalScrollbar = scrollbar;
			templateScrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
			templateScrollRect.verticalScrollbarSpacing = -3f;

			viewportObj.GetComponent<Mask>().showMaskGraphic = false;
			Image viewportImage = viewportObj.GetComponent<Image>();
			viewportImage.sprite = resources.mask;
			viewportImage.type = Image.Type.Sliced;

			Text labelText = labelObj.GetComponent<Text>();
			SetDefaultTextValues(labelText);
			labelText.fontSize = 16;
			labelText.alignment = TextAnchor.MiddleLeft;

			Image arrowImage = arrowObj.GetComponent<Image>();
			arrowImage.sprite = Assets.GetSprite("dash_arrow_down");
			arrowImage.color = Color.black;

			Image uiElementRootImage = uiElementRoot.GetComponent<Image>();
			uiElementRootImage.sprite = Assets.GetSprite("iconSquareSplice01");
			uiElementRootImage.color = Pink;
			uiElementRootImage.type = Image.Type.Sliced;

			var dropdownBehaviour = uiElementRoot.AddComponent<RomenHDropdownBehavior>();
			dropdownBehaviour.borderImage = uiElementRootImage;
			dropdownBehaviour.borderHoverColor = Pink;
			dropdownBehaviour.borderColor = InactiveGray;

			Dropdown uiElementRootDropdown = uiElementRoot.GetComponent<Dropdown>();
			uiElementRootDropdown.targetGraphic = (Graphic)uiElementRootImage;
			//DefaultControls.SetDefaultColorTransitionValues((Selectable)uiElementRootDropdown);
			uiElementRootDropdown.template = templateObj.GetComponent<RectTransform>();
			uiElementRootDropdown.captionText = labelText;
			uiElementRootDropdown.itemText = itemLabelText;
			itemLabelText.text = "Option A";
			uiElementRootDropdown.options.Add(new Dropdown.OptionData()
			{
				text = "Option A"
			});
			uiElementRootDropdown.options.Add(new Dropdown.OptionData()
			{
				text = "Option B"
			});
			uiElementRootDropdown.options.Add(new Dropdown.OptionData()
			{
				text = "Option C"
			});
			uiElementRootDropdown.RefreshShownValue();

			RectTransform labelTransform = labelObj.GetComponent<RectTransform>();
			labelTransform.anchorMin = Vector2.zero;
			labelTransform.anchorMax = Vector2.one;
			labelTransform.offsetMin = new Vector2(10f, 6f);
			labelTransform.offsetMax = new Vector2(-25f, -7f);

			RectTransform arrowTransform = arrowObj.GetComponent<RectTransform>();
			arrowTransform.anchorMin = new Vector2(1f, 0.5f);
			arrowTransform.anchorMax = new Vector2(1f, 0.5f);
			arrowTransform.sizeDelta = new Vector2(20f, 20f);
			arrowTransform.anchoredPosition = new Vector2(-15f, 0.0f);

			RectTransform templateTransform = templateObj.GetComponent<RectTransform>();
			templateTransform.anchorMin = new Vector2(0.0f, 0.0f);
			templateTransform.anchorMax = new Vector2(1f, 0.0f);
			templateTransform.pivot = new Vector2(0.5f, 1f);
			templateTransform.anchoredPosition = new Vector2(0.0f, 2f);
			templateTransform.sizeDelta = new Vector2(0.0f, 150f);

			RectTransform viewportTransform = viewportObj.GetComponent<RectTransform>();
			viewportTransform.anchorMin = new Vector2(0.0f, 0.0f);
			viewportTransform.anchorMax = new Vector2(1f, 1f);
			viewportTransform.sizeDelta = new Vector2(-18f, 0.0f);
			viewportTransform.pivot = new Vector2(0.0f, 1f);

			RectTransform contentTransform = contentObj.GetComponent<RectTransform>();
			contentTransform.anchorMin = new Vector2(0.0f, 1f);
			contentTransform.anchorMax = new Vector2(1f, 1f);
			contentTransform.pivot = new Vector2(0.5f, 1f);
			contentTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
			contentTransform.sizeDelta = new Vector2(0.0f, 28f);

			RectTransform itemTransform = itemObj.GetComponent<RectTransform>();
			itemTransform.anchorMin = new Vector2(0.0f, 0.5f);
			itemTransform.anchorMax = new Vector2(1f, 0.5f);
			itemTransform.sizeDelta = new Vector2(0.0f, 43f);

			RectTransform itemBackgroundTransform = itemBackgroundObj.GetComponent<RectTransform>();
			itemBackgroundTransform.anchorMin = Vector2.zero;
			itemBackgroundTransform.anchorMax = Vector2.one;
			itemBackgroundTransform.sizeDelta = Vector2.zero;

			//RectTransform itemCheckmarkTransform = itemCheckmarkObj.GetComponent<RectTransform>();
			//itemCheckmarkTransform.anchorMin = new Vector2(0.0f, 0.5f);
			//itemCheckmarkTransform.anchorMax = new Vector2(0.0f, 0.5f);
			//itemCheckmarkTransform.sizeDelta = new Vector2(20f, 20f);
			//itemCheckmarkTransform.anchoredPosition = new Vector2(10f, 0.0f);

			RectTransform itemLabelTransform = itemLabelObj.GetComponent<RectTransform>();
			itemLabelTransform.anchorMin = Vector2.zero;
			itemLabelTransform.anchorMax = Vector2.one;
			itemLabelTransform.offsetMin = new Vector2(4f, 1f);
			itemLabelTransform.offsetMax = new Vector2(-4f, -1f);
			templateObj.SetActive(false);

			return uiElementRoot;
		}

		public static GameObject MakeScrollbar()
		{
			DefaultControls.Resources resources = new DefaultControls.Resources();
			GameObject uiElementRoot = CreateUIElementRoot("Scrollbar", new Vector2(160f, 20f), typeof(Image), typeof(Scrollbar));
			GameObject uiObject1 = CreateUIObject("Sliding Area", uiElementRoot, typeof(RectTransform));
			GameObject uiObject2 = CreateUIObject("Handle", uiObject1, typeof(Image));
			Image component1 = uiElementRoot.GetComponent<Image>();
			component1.sprite = resources.background;
			component1.type = Image.Type.Sliced;
			component1.color = Color.white;
			Image component2 = uiObject2.GetComponent<Image>();
			component2.sprite = resources.standard;
			component2.type = Image.Type.Sliced;
			component2.color = Color.white;
			RectTransform component3 = uiObject1.GetComponent<RectTransform>();
			component3.sizeDelta = new Vector2(-20f, -20f);
			component3.anchorMin = Vector2.zero;
			component3.anchorMax = Vector2.one;
			RectTransform component4 = uiObject2.GetComponent<RectTransform>();
			component4.sizeDelta = new Vector2(20f, 20f);
			Scrollbar component5 = uiElementRoot.GetComponent<Scrollbar>();
			component5.handleRect = component4;
			component5.targetGraphic = (Graphic)component2;
			//DefaultControls.SetDefaultColorTransitionValues((Selectable)component5);
			return uiElementRoot;
		}
	}

	public class RomenHDropdownBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		public Image borderImage;
		public Color borderColor;
		public Color borderHoverColor;

		void Start()
		{
			if (borderImage != null)
			{
				borderImage.color = borderColor;
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (borderImage != null)
			{
				borderImage.color = borderHoverColor;
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (borderImage != null)
			{
				borderImage.color = borderColor;
			}
		}
	}

}
