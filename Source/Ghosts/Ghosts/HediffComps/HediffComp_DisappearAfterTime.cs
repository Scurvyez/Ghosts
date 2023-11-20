using RimWorld;
using Verse;

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
            GameComponent_StoreGhostPawns gameComp = Current.Game.GetComponent<GameComponent_StoreGhostPawns>();
            
            if (ticksRemaining <= 0)
            {
                Pawn.DeSpawn(DestroyMode.Vanish);
                gameComp.SpawnedColonistGhosts.Remove(Pawn.Name);
                gameComp.AvailableColonistGhosts.Add(Pawn.Name);
                Pawn.health.RemoveAllHediffs();
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
