using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace AllowTool
{
	// Generates hauling jobs for zones designated need urgent refilling
	public class WorkGiver_HaulUrgentlyRefill : WorkGiver_HaulUrgently
	{
		private static bool NeedsRefill(IntVec3 c, Map map)
		{
			foreach(var thing in map.thingGrid.ThingsListAt(c))
			{
				if (thing.def.EverStorable(false))
				{
					return false;
				}
				if (thing.def.entityDefToBuild != null && thing.def.entityDefToBuild.passability != Traversability.Standable)
				{
					return false;
				}
				if (thing.def.surfaceType == SurfaceType.None && thing.def.passability != Traversability.Standable)
				{
					return false;
				}
			}
			return true;
		}

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			List<SlotGroup> needRefill = pawn.Map.haulDestinationManager.AllGroupsListForReading
				.FindAll(group => AllowToolController.Instance.WorldSettings.IsMarkedForRefill(pawn.Map, group) 
					&& group.CellsList.Any(c => NeedsRefill(c, pawn.Map)));

			return pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.HaulableEver)
				.FindAll(t => !t.IsInValidBestStorage()
				&& HaulAIUtility.PawnCanAutomaticallyHaulFast(pawn, t, false)
				&& needRefill.Any(g => g.Settings.AllowedToAccept(t)));
		}
	}
}