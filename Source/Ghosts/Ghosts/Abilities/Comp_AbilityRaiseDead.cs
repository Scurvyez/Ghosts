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
    public class Comp_AbilityRaiseDead : CompAbilityEffect
    {
        public new CompProperties_AbilityRaiseDead Props => (CompProperties_AbilityRaiseDead)props;
        public PawnDataDuplication PawnDataDuplication;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Color debugColor1 = new Color(0.545f, 0.388f, 0.645f, 1f);
            GameComponent_StoreGhostPawns gameComp = Current.Game.GetComponent<GameComponent_StoreGhostPawns>();

            if (!gameComp.HumanGhosts.NullOrEmpty())
            {
                Pawn ghost = gameComp.HumanGhosts.RandomElement();
                foreach (IntVec3 tile in GenRadial.RadialCellsAround(parent.pawn.Position, 2f, useCenter: true))
                {
                    if (tile.IsValid && !ghost.Spawned && parent.pawn.Map != null)
                    {
                        GenSpawn.Spawn(ghost, target.Cell.RandomAdjacentCell8Way(), parent.pawn.Map);
                        gameComp.HumanGhosts.Remove(ghost);
                        gameComp.SpawnedHumanGhosts.Add(ghost);
                        ghost.health.RemoveAllHediffs();
                        ghost.health.AddHediff(GhostsDefOf.SZ_GhostsDisappearing);

                        Log.Message("HumanGhosts: " + gameComp.HumanGhosts.Count().ToString().Colorize(debugColor1));
                        Log.Message("SpawnedHumanGhosts: " + gameComp.SpawnedHumanGhosts.Count().ToString().Colorize(debugColor1));
                    }
                }
            }
            return;
        }

        public override bool GizmoDisabled(out string reason)
        {
            GameComponent_StoreGhostPawns gameComp = Current.Game.GetComponent<GameComponent_StoreGhostPawns>();
            if (gameComp.HumanGhosts.NullOrEmpty() || gameComp.HumanGhosts.RandomElement().Spawned)
            {
                reason = "SZ_AbilityRaiseDead_EmptyGhostCache".Translate(parent.pawn);
                return true;
            }
            reason = null;
            return false;
        }
    }
}
