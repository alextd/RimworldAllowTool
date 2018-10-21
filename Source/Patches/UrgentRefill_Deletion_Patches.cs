using System.Collections.Generic;
using Harmony;
using Verse;
using RimWorld;
using UnityEngine;

namespace AllowTool.Source.Patches
{
	[HarmonyPatch(typeof(HaulDestinationManager), "RemoveHaulDestination")]
	class UrgentRefill_Deletion_Patches
	{
		//public void RemoveHaulDestination(IHaulDestination haulDestination)
		public static void Postfix(IHaulDestination haulDestination)
		{
			if (haulDestination is ISlotGroupParent slotGroupParent &&
				slotGroupParent.GetSlotGroup() is SlotGroup slotGroup)
				AllowToolController.Instance.WorldSettings.MarkForRefill(haulDestination.Map, slotGroup, false);
		}
	}
}
