using HarmonyLib;
using KSerialization;
using STRINGS;

namespace InfiniteSourceSink
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class InfiniteSourceControl : KMonoBehaviour, IIntSliderControl
    {
        public const string Title = "Flow Rate";
		public static readonly string Tooltip = $"Flow Rate";

		public string SliderTitleKey => "STRINGS.UI.UISIDESCREENS.INFINITESOURCE.FLOW.TITLE";

		public string SliderUnits => UI.UNITSUFFIXES.MASS.GRAM + "/" + UI.UNITSUFFIXES.SECOND;

		[MyCmpGet]
		public InfiniteSource source;

		[MyCmpGet]
		public Filterable filterable;

		public string GetSliderTooltip() => Title;

		public string GetSliderTooltipKey(int index)
		{
			return "STRINGS.UI.UISIDESCREENS.INFINITESOURCE.FLOW.TOOLTIP";
		}

		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}

		public float GetSliderMin(int index)
		{
			return 0;
		}

		public float GetSliderMax(int index)
        {
			var flowManager = Conduit.GetFlowManager(source.conduitType);
			return Traverse.Create(flowManager).Field("MaxMass").GetValue<float>() * 1000f;
        }

        public float GetSliderValue(int index)
        {
			return source.Flow;
        }

        public void SetSliderValue(float percent, int index)
        {
			source.Flow = percent;
        }
    }
}
