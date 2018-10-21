using System.Collections.Generic;
using Harmony;
using Verse;
using RimWorld;
using UnityEngine;

//Adds buttons to storage zones to refill them urgently 
namespace AllowTool.Patches
{
	[StaticConstructorOnStartup]
	internal static class SlotGroup_GetGizmos_Patch
	{
		public static Texture2D haulUrgentlyIcon = ContentFinder<Texture2D>.Get("haulUrgently");
		
		public static void InsertUrgentRefillGizmos(ref IEnumerable<Gizmo> __result, Map map, ISlotGroupParent parent)
		{
			SlotGroup group = parent.GetSlotGroup();
			__result = __result.Add(new Command_Toggle()
			{
				defaultLabel = "Urgent Refill",
				defaultDesc = "If any cell is unoccupied, it will be urgently refilled",
				icon = haulUrgentlyIcon,
				isActive = () => AllowToolController.Instance.WorldSettings.IsMarkedForRefill(map, group),
				toggleAction = delegate
				{
					AllowToolController.Instance.WorldSettings.MarkForRefill(map, group,
						!AllowToolController.Instance.WorldSettings.IsMarkedForRefill(map, group));
				}
			});
		}
	}

	[HarmonyPatch(typeof(Building_Storage), "GetGizmos")]
	internal static class BuildingStorage_GetGizmos_Patch
	{
		[HarmonyPostfix]
		public static void InsertUrgentRefillGizmos(ref IEnumerable<Gizmo> __result, Building_Storage __instance)
		{
			SlotGroup_GetGizmos_Patch.InsertUrgentRefillGizmos(ref __result, __instance.Map, __instance);
		}
	}

	[StaticConstructorOnStartup]
	[HarmonyPatch(typeof(Zone_Stockpile), "GetGizmos")]
	internal static class ZoneStockpile_GetGizmos_Patch
	{
		[HarmonyPostfix]
		public static void InsertUrgentRefillGizmos(ref IEnumerable<Gizmo> __result, Building_Storage __instance)
		{
			SlotGroup_GetGizmos_Patch.InsertUrgentRefillGizmos(ref __result, __instance.Map, __instance);
		}
	}
}
