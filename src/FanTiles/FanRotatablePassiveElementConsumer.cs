using KSerialization;
using UnityEngine;
using HarmonyLib;

namespace Fans
{
    [SkipSaveFileSerialization]
    [SerializationConfig(MemberSerialization.OptIn)]
    public class FanRotatablePassiveElementConsumer : PassiveElementConsumer
    {
        [SerializeField]
        public Vector3 rotatableCellOffset;
    }

    [HarmonyPatch(typeof(ElementConsumer))]
    [HarmonyPatch("GetSampleCell")]
    public static class ElementConsumer_GetSampleCell_Patch
    {
        [HarmonyPriority(-10000)] // Extremely low priority. We want this to happen last, since this will only overwrite FanRotatablePassiveElementConsumer variable
        public static void Prefix(ElementConsumer __instance)
        {
            if (__instance is FanRotatablePassiveElementConsumer)
            {
                Vector3 rotatableCellOffset = ((FanRotatablePassiveElementConsumer)__instance).rotatableCellOffset;
                Rotatable rotatable = __instance.GetComponent<Rotatable>();
                if (rotatable != null) __instance.sampleCellOffset = Rotatable.GetRotatedOffset(rotatableCellOffset, rotatable.GetOrientation());
            }
        }
    }
}
