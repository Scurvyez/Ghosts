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
    public class HediffComp_DisappearAfterTime : HediffComp
    {
        public HediffCompProperties_DisappearAfterTime Props => (HediffCompProperties_DisappearAfterTime)props;
        private int ticksRemaining;

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            ticksRemaining = Props.ticksTillGone;
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            ticksRemaining--;
            Color debugColor1 = new Color(0.545f, 0.388f, 0.645f, 1f);
            GameComponent_StoreGhostPawns gameComp = Current.Game.GetComponent<GameComponent_StoreGhostPawns>();
            
            if (ticksRemaining <= 0)
            {
                Pawn.DeSpawn(DestroyMode.Vanish);
                gameComp.HumanGhosts.Add(Pawn);
                Pawn.health.RemoveAllHediffs();
                gameComp.SpawnedHumanGhosts.Remove(Pawn);

                if (ticksRemaining == 0)
                {
                    Log.Message("HumanGhosts: " + gameComp.HumanGhosts.Count().ToString().Colorize(debugColor1));
                    Log.Message("SpawnedHumanGhosts: " + gameComp.SpawnedHumanGhosts.Count().ToString().Colorize(debugColor1));
                }
            }
        }

        public override string CompTipStringExtra
        {
            get
            {
                if (ticksRemaining <= 0)
                {
                    return null;
                }
                return Pawn.Name + "SZ_DisappearAfterTime_TimeRemaining".Translate(ticksRemaining.ToStringTicksToPeriod().Colorize(ColoredText.DateTimeColor)).Resolve().CapitalizeFirst();
            }
        }

        public override void CompExposeData()
        {
            Scribe_Values.Look(ref ticksRemaining, "ticksLeft", 0);
        }
    }
}
