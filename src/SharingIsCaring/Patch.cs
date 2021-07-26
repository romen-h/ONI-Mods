using HarmonyLib;

using UnityEngine;

namespace SharingIsCaring
{
	[HarmonyPatch(typeof(EatChore.States))]
	[HarmonyPatch(nameof(EatChore.States.InitializeStates))]
	public static class EatChore_Patch
	{
		public static void Postfix(EatChore.States __instance)
		{
			__instance.eatatmessstation.Exit(delegate(EatChore.StatesInstance smi)
			{
				var minion = smi.sm.eater.Get(smi).GetComponent<MinionIdentity>();
				if (minion == null) return;
				Ownables ownables = minion.GetSoleOwner();
				if (ownables == null) return;
				AssignableSlotInstance slot = ownables.GetSlot(Db.Get().AssignableSlots.MessStation);
				if (slot == null) return;
				slot.Unassign(true);
			});
		}
	}

	[HarmonyPatch(typeof(SleepChore.States))]
	[HarmonyPatch(nameof(SleepChore.States.InitializeStates))]
	public static class SleepChore_Patch
	{
		public static void Postfix(SleepChore.States __instance)
		{
			__instance.success.Exit(delegate (SleepChore.StatesInstance smi)
			{
				var minion = smi.sm.sleeper.Get(smi).GetComponent<MinionIdentity>();
				if (minion == null) return;
				Ownables ownables = minion.GetSoleOwner();
				if (ownables == null) return;
				AssignableSlotInstance slot = ownables.GetSlot(Db.Get().AssignableSlots.Bed);
				if (slot == null) return;
				slot.Unassign(false);
			});
		}
	}
}
