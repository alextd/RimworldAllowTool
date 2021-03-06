﻿using RimWorld;
using Verse;

namespace AllowTool {
	/// <summary>
	/// Forbids all forbiddable things in the designated area
	/// </summary>
	public class Designator_Forbid : Designator_SelectableThings {
		public Designator_Forbid(ThingDesignatorDef def) : base(def) {
			inheritIcon = !AllowToolController.Instance.ReplaceIconsSetting;
		}

		public override AcceptanceReport CanDesignateThing(Thing thing) {
			if (thing.Position.Fogged(thing.Map)) return false;
			var comp = (thing as ThingWithComps)?.GetComp<CompForbiddable>();
			return comp != null && !comp.Forbidden;
		}


		public override void DesignateSingleCell(IntVec3 cell) {
			numThingsDesignated = AllowToolUtility.ToggleForbiddenInCell(cell, Find.CurrentMap, true);
		}
	}
}