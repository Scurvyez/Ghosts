using System.Linq;
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
                gameComp.ColonistGhosts += 1;
                Pawn.health.RemoveAllHediffs();
                gameComp.SpawnedColonistGhosts =+ 1;

                if (ticksRemaining == 0)
                {
                    Log.Message("ColonistGhosts: " + gameComp.ColonistGhosts.ToString().Colorize(debugColor1));
                    Log.Message("SpawnedColonistGhosts: " + gameComp.SpawnedColonistGhosts.ToString().Colorize(debugColor1));
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
