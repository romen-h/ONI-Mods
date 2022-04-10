using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;

namespace RomenH.SharingIsCaring
{
	[HarmonyPatch(typeof(EatChore.States))]
	[HarmonyPatch(nameof(EatChore.States.InitializeStates))]
	public static class EatChore_Patch
	{
		public static void Postfix(EatChore.States __instance)
		{
			if (ModSettings.Instance.ShareTables)
			{
				__instance.eatatmessstation.Exit(delegate (EatChore.StatesInstance smi)
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
	}
}
