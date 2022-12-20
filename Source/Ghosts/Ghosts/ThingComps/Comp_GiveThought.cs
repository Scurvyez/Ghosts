using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;
using UnityEngine;

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
                        pawn.needs.mood.thoughts.memories.TryGainMemory(Props.badThoughtDef);
                    }
                }
            }
        }
    }
}
