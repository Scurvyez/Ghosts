using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;

namespace Ghosts
{
    public class Comp_AbilityRaiseDead : CompAbilityEffect
    {
        public new CompProperties_AbilityRaiseDead Props => (CompProperties_AbilityRaiseDead)props;
        GameComponent_StoreGhostPawns GameComp = Current.Game.GetComponent<GameComponent_StoreGhostPawns>();
        
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);

            IEnumerable<Name> spawnedPawnNames = parent.pawn.Map.mapPawns.AllPawnsSpawned.Select(pawn => pawn.Name);
            if (GameComp != null && GameComp.AvailableColonistGhosts.Count > 0)
            {
                if (GameComp.GhostTextures != null && GameComp.GhostTextures.Count > 0)
                {
                    PawnGenerationRequest request = new PawnGenerationRequest(
                        GhostsDefOf.SZ_AnimalGhostBaseKind, parent.pawn.Faction, PawnGenerationContext.NonPlayer, forceGenerateNewPawn: true,
                        canGeneratePawnRelations: false, allowFood: false, allowAddictions: false, fixedBiologicalAge: 0, fixedChronologicalAge: 0, fixedGender: Gender.None,
                        fixedIdeo: null, forceNoIdeo: true, forceBaselinerChance: 0f);

                    Ghost ghostToSpawn = PawnGenerator.GeneratePawn(request) as Ghost;
                    var selectedGhost = GameComp.GhostTextures.RandomElement();
                    ghostToSpawn.Name = selectedGhost.Key;
                    ghostToSpawn.kindDef.label = selectedGhost.Key.ToString();
                    ghostToSpawn.GhostKeyValuePair = selectedGhost;

                    foreach (IntVec3 tile in GenRadial.RadialCellsAround(parent.pawn.Position, 2f, useCenter: true))
                    {
                        if (tile.IsValid && !ghostToSpawn.Spawned && parent.pawn.Map != null)
                        {
                            // Check if a ghost with the selected name is already spawned
                            if (!spawnedPawnNames.Contains(ghostToSpawn.Name))
                            {
                                // Add the ghost to the spawned and remove it from the available list
                                GameComp.SpawnedColonistGhosts.Add(ghostToSpawn.Name);
                                GameComp.AvailableColonistGhosts.Remove(ghostToSpawn.Name);

                                // Spawn the ghost
                                GenSpawn.Spawn(ghostToSpawn, tile.RandomAdjacentCell8Way(), parent.pawn.Map);

                                ghostToSpawn.health.RemoveAllHediffs();
                                ghostToSpawn.health.AddHediff(GhostsDefOf.SZ_GhostsDisappearing);
                            }
                        }
                    }
                }
                else
                {
                    // Handle the case when GhostTextures is null or empty
                    Log.Warning("GhostTextures is null or empty.");
                }
            }
        }

        public override bool GizmoDisabled(out string reason)
        {
            if (GameComp != null)
            {
                // Case where no colonists have died, so no ghosts available
                if (GameComp.AvailableColonistGhosts.Count <= 0 
                    && GameComp.SpawnedColonistGhosts.Count <= 0)
                {
                    reason = "SZ_AbilityRaiseDead_NoneDead".Translate(parent.pawn);
                    return true;
                }
                // Case where all available ghosts are currently spawned
                else if (GameComp.AvailableColonistGhosts.Count <= 0 
                    && GameComp.SpawnedColonistGhosts.Count > 0)
                {
                    reason = "SZ_AbilityRaiseDead_AllDeadSpawned".Translate(parent.pawn);
                    return true;
                }
            }

            reason = null;
            return false;
        }
    }
}
