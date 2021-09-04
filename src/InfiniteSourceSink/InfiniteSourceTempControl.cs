using System;
using KSerialization;
using STRINGS;

namespace InfiniteSourceSink
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class InfiniteSourceTempControl : KMonoBehaviour, ISingleSliderControl
	{
		public const string Title = "Temperature";
		public const string Tooltip = "Temperature";

		public string SliderTitleKey => "STRINGS.UI.UISIDESCREENS.INFINITESOURCE.TEMP.TITLE";

		public string SliderUnits => UI.UNITSUFFIXES.TEMPERATURE.KELVIN;

		[MyCmpGet]
		public InfiniteSource source;

		[MyCmpGet]
		public Filterable filterable;

		public string GetSliderTooltip() => Tooltip;

		public string GetSliderTooltipKey(int index) => "STRINGS.UI.UISIDESCREENS.INFINITESOURCE.TEMP.TOOLTIP";

		public int SliderDecimalPlaces(int index) => 1;

		public float GetSliderMin(int index)
		{
			Element element = ElementLoader.GetElement(filterable.SelectedTag);
			return element?.lowTemp ?? 0f;
		}

		public float GetSliderMax(int index)
		{
			Element element = ElementLoader.GetElement(filterable.SelectedTag);
			return Math.Min(element?.highTemp ?? 100f, 7000f);
		}

		public float GetSliderValue(int index)
		{
			return source.Temp;
		}

		public void SetSliderValue(float percent, int index)
		{
			source.Temp = percent;
		}
	}
}
