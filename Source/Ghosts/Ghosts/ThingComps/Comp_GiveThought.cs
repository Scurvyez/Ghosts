using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace Ghosts
{
    public class Comp_GiveThought : ThingComp
    {
        public CompProperties_GiveThought Props => (CompProperties_GiveThought)props;
        public int tickCounter = 0;
        public List<Pawn> pawnList = new List<Pawn>();
        public Pawn thisPawn;

        public override void CompTick()
        {
            base.CompTick();
            Pawn parentPawn = parent as Pawn;
            Pawn_RelationsTracker relations = parentPawn.relations;

            tickCounter++;
            if (tickCounter <= Props.tickInterval)
            {
                return;
            }

            thisPawn = parent as Pawn;
            if (thisPawn !=null && thisPawn.Map != null && !thisPawn.Dead && !thisPawn.Downed)
            {
                foreach (Thing thing in GenRadial.RadialDistinctThingsAround(thisPawn.Position, thisPawn.Map, Props.radius, useCenter: true))
                {
                    if ((thing is Pawn pawn) && (pawn.IsColonist || !pawn.NonHumanlikeOrWildMan()))
                    {
                        if (PawnUtility.IsBiologicallyOrArtificiallyBlind(pawn))
                        {
                            // maybe still some awareness of ghosts, just less than usual, like a "gut-feeling"?
                            return;
                        }

                        if (Props.badThoughtDef != null)
                        {
                            pawn.needs.mood.thoughts.memories.TryGainMemory(Props.badThoughtDef);
                        }

                        if (Props.goodThoughtDef != null)
                        {
                            pawn.needs.mood.thoughts.memories.TryGainMemory(Props.goodThoughtDef);

                            if (relations.FamilyByBlood.Any())
                            {
                                pawn.needs.mood.thoughts.memories.TryGainMemory(Props.familialThoughtDef);
                            }
                        }

                        /*
                        for (int i = 0; i < thisPawn.Map.GetComponent<MapComponent_StoreGhostPawns>().HumanGhosts.Count; i++)
                        {
                            if (relations.DirectRelationExists(PawnRelationDefOf.Lover, thisPawn.Map.GetComponent<MapComponent_StoreGhostPawns>().HumanGhosts[i]))
                            {
                                pawn.needs.mood.thoughts.memories.TryGainMemory(Props.loverThoughtDef);
                            }
                        }
                        */
                    }
                }
            }
        }
    }
}
