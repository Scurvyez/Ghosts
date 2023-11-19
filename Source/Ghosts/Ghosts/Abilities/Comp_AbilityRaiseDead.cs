using System.Linq;
using RimWorld;
using Verse;
using UnityEngine;

namespace Ghosts
{
    public class Comp_AbilityRaiseDead : CompAbilityEffect
    {
        public new CompProperties_AbilityRaiseDead Props => (CompProperties_AbilityRaiseDead)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Color debugColor1 = new Color(0.545f, 0.388f, 0.645f, 1f);
            GameComponent_StoreGhostPawns gameComp = Current.Game.GetComponent<GameComponent_StoreGhostPawns>();

            if (gameComp.ColonistGhosts > 0)
            {
                //Pawn ghost = gameComp.ColonistGhosts.RandomElement();
                Pawn ghostToSpawn = new Pawn();
                foreach (IntVec3 tile in GenRadial.RadialCellsAround(parent.pawn.Position, 2f, useCenter: true))
                {
                    if (tile.IsValid && !ghostToSpawn.Spawned && parent.pawn.Map != null)
                    {
                        //PawnDataDuplication.SpawnCopy(ghost, target.Cell.RandomAdjacentCell8Way(), parent.pawn.Map);

                        //gameComp.ColonistGhosts.Remove(ghostToSpawn);
                        //gameComp.SpawnedColonistGhosts.Add(ghostToSpawn);
                        ghostToSpawn.health.RemoveAllHediffs();
                        ghostToSpawn.health.AddHediff(GhostsDefOf.SZ_GhostsDisappearing);

                        Log.Message("ColonistGhosts: " + gameComp.ColonistGhosts.ToString().Colorize(debugColor1));
                        Log.Message("SpawnedColonistGhosts: " + gameComp.SpawnedColonistGhosts.ToString().Colorize(debugColor1));
                    }
                }
            }
            return;
        }

        public override bool GizmoDisabled(out string reason)
        {
            GameComponent_StoreGhostPawns gameComp = Current.Game.GetComponent<GameComponent_StoreGhostPawns>();
            if (gameComp.ColonistGhosts < 0 && gameComp.SpawnedColonistGhosts < 0)
            {
                reason = "SZ_AbilityRaiseDead_NoneDead".Translate(parent.pawn);
                return true;
            }
            else if (gameComp.ColonistGhosts < 0 && gameComp.SpawnedColonistGhosts > 0)
            {
                reason = "SZ_AbilityRaiseDead_AllDeadSpawned".Translate(parent.pawn);
                return true;
            }
            reason = null;
            return false;
        }
    }
}
