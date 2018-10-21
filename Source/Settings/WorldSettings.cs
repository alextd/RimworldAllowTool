using System.Collections.Generic;
using HugsLib.Utils;
using Verse;
using RimWorld;

namespace AllowTool.Settings {
	/// <summary>
	/// Store settings for a specific world save file
	/// </summary>
	public class WorldSettings : UtilityWorldObject {
		private HashSet<int> partyHuntingPawns = new HashSet<int>();
		private HashSet<Building_Storage> markedForRefillBuilding = new HashSet<Building_Storage>();
		private HashSet<Zone_Stockpile> markedForRefillStockpile = new HashSet<Zone_Stockpile>();

		public override void ExposeData() {
			base.ExposeData();
			// convert to list for serialization
			var partyHuntingList = new List<int>(partyHuntingPawns);
			Scribe_Collections.Look(ref partyHuntingList, "partyHuntingPawns");
			partyHuntingPawns = new HashSet<int>(partyHuntingList);

			Scribe_Collections.Look(ref markedForRefillBuilding, "markedForRefillBuilding", LookMode.Reference);
			if (markedForRefillBuilding == null) markedForRefillBuilding = new HashSet<Building_Storage>();

			Scribe_Collections.Look(ref markedForRefillStockpile, "markedForRefillStockpile", LookMode.Reference);
			if (markedForRefillStockpile == null) markedForRefillStockpile = new HashSet<Zone_Stockpile>();
		}

		public bool PawnIsPartyHunting(Pawn pawn) {
			return partyHuntingPawns.Contains(pawn.thingIDNumber);
		}

		public void TogglePawnPartyHunting(Pawn pawn, bool enable) {
			var id = pawn.thingIDNumber;
			if (enable) {
				partyHuntingPawns.Add(id);
			} else {
				partyHuntingPawns.Remove(id);
			}
		}

		public bool IsMarkedForRefill(Map map, SlotGroup group)
		{
			return markedForRefillBuilding.Contains(group.parent as Building_Storage) 
				|| markedForRefillStockpile.Contains(group.parent as Zone_Stockpile);
		}

		public void MarkForRefill(Map map, SlotGroup group, bool enable)
		{
			if (group.parent is Building_Storage building)
			{
				if (enable)
					markedForRefillBuilding.Add(building);
				else
					markedForRefillBuilding.Remove(building);
			}
			else if (group.parent is Zone_Stockpile zone)
			{
				if (enable)
					markedForRefillStockpile.Add(zone);
				else
					markedForRefillStockpile.Remove(zone);
			}
		}

	}
}