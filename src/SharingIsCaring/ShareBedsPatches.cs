using HarmonyLib;

namespace RomenH.SharingIsCaring
{
	[HarmonyPatch(typeof(SleepChoreMonitor.Instance))]
	[HarmonyPatch(nameof(SleepChoreMonitor.Instance.UpdateBed))]
	public static class SleepChoreMonitor_UpdateBed_Patch
	{
		public static bool Prefix(SleepChoreMonitor.Instance __instance)
		{
			if (ModSettings.Instance.ShareBeds)
			{
				var minion = __instance.sm.masterTarget.Get(__instance).GetComponent<MinionIdentity>();
				if (minion == null)
				{
					//Debug.LogWarning("SharingIsCaring.UpdateBed: MinionIdentity not found. [Error]");
					return true;
				}

				var soleOwner = minion.GetSoleOwner();
				if (soleOwner == null)
				{
					//Debug.LogWarning("SharingIsCaring.UpdateBed: SoleOwner not found. [Error]");
					return true;
				}

				var medicalCot = soleOwner.GetAssignable(Db.Get().AssignableSlots.MedicalBed);
				var bed = soleOwner.GetAssignable(Db.Get().AssignableSlots.Bed);

				if (!__instance.HasSleepUrge())
				{
					if (bed != null)
					{
						//Debug.Log($"SharingIsCaring.UpdateBed: Minion {minion.GetProperName()} has a bed. Unassigning...");
						bed.Unassign();
					}
					//Debug.Log($"SharingIsCaring.UpdateBed: Minion {minion.GetProperName()} doesn't need to sleep. [Skip]");
					return false;
				}

				if (medicalCot != null)
				{
					//Debug.LogWarning($"SharingIsCaring.UpdateBed: Minion {minion.GetProperName()} already has a medical cot. [Skip]");
					__instance.sm.bed.Set(medicalCot, __instance);
					return false;
				}

				
				if (bed != null)
				{
					//Debug.LogWarning($"SharingIsCaring.UpdateBed: Minion {minion.GetProperName()} already has a bed. [Skip]");
					__instance.sm.bed.Set(bed, __instance);
					return false;
				}

				//Debug.Log($"SharingIsCaring.UpdateBed: Minion {minion.GetProperName()} needs a bed. [Continue]");
			}

			return true;
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
				if (ModSettings.Instance.ShareBeds)
				{
					var minion = smi.sm.sleeper.Get(smi).GetComponent<MinionIdentity>();
					if (minion == null)
					{
						//Debug.LogWarning("SharingIsCaring.SleepChoreEnding: MinionIdentity not found. [Error]");
						return;
					}

					Ownables ownables = minion.GetSoleOwner();
					if (ownables == null)
					{
						//Debug.LogWarning("SharingIsCaring.SleepChoreEnding: SoleOwner not found. [Error]");
						return;
					}

					AssignableSlotInstance slot = ownables.GetSlot(Db.Get().AssignableSlots.Bed);

					//Debug.Log($"SharingIsCaring.SleepChoreEnding: Unassigning bed for {minion.GetProperName()}.");
					slot.Unassign(false);
				}
			});
		}
	}
}
