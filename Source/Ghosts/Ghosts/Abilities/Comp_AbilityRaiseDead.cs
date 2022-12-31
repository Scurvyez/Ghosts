using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;

namespace Ghosts
{
    public class Comp_AbilityRaiseDead : CompAbilityEffect
    {
        public new CompProperties_AbilityRaiseDead Props => (CompProperties_AbilityRaiseDead)props;
        public MapComponent_StoreGhostPawns MapComp = Find.CurrentMap.GetComponent<MapComponent_StoreGhostPawns>();

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            if (MapComp.HumanGhosts != null)
            {
                if (target.Cell.RandomAdjacentCell8Way().IsValid)
                {
                    Pawn ghost = MapComp.HumanGhosts.RandomElement();
                    GenSpawn.Spawn(ghost, target.Cell.RandomAdjacentCell8Way(), parent.pawn.Map);
                }
                if (MapComp.HumanGhosts.RandomElement().Spawned)
                {
                    return;
                }
            }
            return;
        }

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            if (MapComp.HumanGhosts == null)
            {
                if (throwMessages)
                {
                    Messages.Message("Cannot Use Ability".Translate(parent.def.label) + ": " + "There are no colonist souls present.", target.ToTargetInfo(parent.pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            return true;
        }
    }
}
