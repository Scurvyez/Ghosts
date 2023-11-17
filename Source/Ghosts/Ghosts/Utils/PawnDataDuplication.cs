using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace Ghosts
{
    public class PawnDataDuplication
    {
        /// <summary>
        /// Duplicate a dead pawns ideo, relations, name, and graphics.
        /// </summary>
        public static void Duplicate(Pawn sourcePawn, Pawn destinationPawn)
        {
            try
            {
                if (ModsConfig.IdeologyActive)
                {
                    DuplicatePawnIdeology(ref sourcePawn, ref destinationPawn);
                }
                DuplicatePawnRelations(ref sourcePawn, ref destinationPawn);

                /*if (sourcePawn.Faction != destinationPawn.Faction)
                {
                    destinationPawn.SetFaction(sourcePawn.Faction);
                }*/

                NameTriple sourcePawnName = (NameTriple)sourcePawn.Name;
                destinationPawn.Name = new NameTriple(sourcePawnName.First, sourcePawnName.Nick, sourcePawnName.Last);

                // copy pawns' graphics
                /*
                destinationPawn.Drawer.renderer.graphics = new PawnGraphicSet(destinationPawn);
                destinationPawn.Drawer.renderer.graphics.nakedGraphic = sourcePawn.Drawer.renderer.graphics.nakedGraphic;
                destinationPawn.Drawer.renderer.graphics.headGraphic = sourcePawn.Drawer.renderer.graphics.headGraphic;
                destinationPawn.Drawer.renderer.graphics.hairGraphic = null;
                destinationPawn.Drawer.renderer.graphics.beardGraphic = null;
                */

                PawnTextureAtlasFrameSet pawnFrameSet;
                Material pawnMaterial;

                GlobalTextureAtlasManager.TryGetPawnFrameSet(sourcePawn, out pawnFrameSet, out var _);
                pawnMaterial = MaterialPool.MatFrom(new MaterialRequest(pawnFrameSet.atlas, ShaderDatabase.Transparent));

                destinationPawn.DefaultGraphic.MatSingle.CopyPropertiesFromMaterial(pawnMaterial);
                sourcePawn.Drawer.renderer.graphics.ResolveAllGraphics();
            }
            catch (Exception exception)
            {
                Log.Error("[<color=#4494E3FF>Ghosts</color>] PawnDataDuplication.Duplicate: Error occurred duplicating " + sourcePawn + " into " + destinationPawn + ". This will have severe consequences. " + exception.Message + exception.StackTrace);
            }
        }

        /// <summary>
        /// Duplicate a dead pawns ideology.
        /// </summary>
        public static void DuplicatePawnIdeology(ref Pawn sourcePawn, ref Pawn destinationPawn)
        {
            try
            {
                if (sourcePawn.ideo == null)
                {
                    destinationPawn.ideo = null;
                }
                else
                {
                    destinationPawn.ideo = sourcePawn.ideo;
                }
            }
            catch (Exception exception)
            {
                Log.Warning("[<color=#4494E3FF>Ghosts</color>] An unexpected error occurred during ideology duplication between " + sourcePawn + " " + destinationPawn + ". The destination IdeoTracker may be left unstable!" + exception.Message + exception.StackTrace);
            }
        }

        /// <summary>
        /// Duplicate a dead pawns relations.
        /// </summary>
        public static void DuplicatePawnRelations(ref Pawn sourcePawn, ref Pawn destinationPawn)
        {
            try
            {
                Pawn_RelationsTracker destinationPawnRelations = new Pawn_RelationsTracker(destinationPawn);
                List<Pawn> checkedOtherPawns = new List<Pawn>();

                foreach (DirectPawnRelation pawnRelation in sourcePawn.relations?.DirectRelations?.ToList())
                {
                    if (!checkedOtherPawns.Contains(pawnRelation.otherPawn))
                    {
                        foreach (DirectPawnRelation otherPawnRelation in pawnRelation.otherPawn.relations?.DirectRelations.ToList())
                        {
                            if (otherPawnRelation.otherPawn == sourcePawn)
                            {
                                pawnRelation.otherPawn.relations.AddDirectRelation(otherPawnRelation.def, destinationPawn);
                            }
                        }
                        checkedOtherPawns.Add(pawnRelation.otherPawn);
                    }
                    destinationPawnRelations.AddDirectRelation(pawnRelation.def, pawnRelation.otherPawn);
                }
                destinationPawnRelations.everSeenByPlayer = true;

                foreach (Map map in Find.Maps)
                {
                    foreach (Pawn animal in map.mapPawns.SpawnedColonyAnimals)
                    {
                        if (animal.playerSettings == null || animal == sourcePawn || animal == destinationPawn)
                            continue;

                        if (animal.playerSettings.Master != null && animal.playerSettings.Master == sourcePawn)
                            animal.playerSettings.Master = destinationPawn;
                    }
                }
                destinationPawn.relations = destinationPawnRelations;
            }
            catch (Exception exception)
            {
                Log.Warning("[<color=#4494E3FF>Ghosts</color>] An unexpected error occurred during relation duplication between " + sourcePawn + " " + destinationPawn + ". The destination RelationTracker may be left unstable!" + exception.Message + exception.StackTrace);
            }
        }

        public static Pawn SpawnCopy(Pawn pawn)
        {
            // Generate a new pawn.
            //PawnGenerationRequest request = new PawnGenerationRequest(pawn.kindDef, faction: null, context: PawnGenerationContext.NonPlayer, fixedBiologicalAge: pawn.ageTracker.AgeBiologicalYearsFloat, fixedChronologicalAge: pawn.ageTracker.AgeChronologicalYearsFloat, fixedGender: pawn.gender);
            //Pawn copy = PawnGenerator.GeneratePawn(request);

            // placeholder pawn object generation
            PawnGenerationRequest request = new PawnGenerationRequest(
                GhostsDefOf.SZ_GhostBaseKind, pawn.Faction, PawnGenerationContext.NonPlayer, forceGenerateNewPawn: true,
                canGeneratePawnRelations: false, allowFood: false, allowAddictions: false, fixedBiologicalAge: 0, fixedChronologicalAge: 0, fixedGender: pawn.gender,
                fixedIdeo: null, forceNoIdeo: true, forceBaselinerChance: 1f);

            Pawn ghost = PawnGenerator.GeneratePawn(request);

            // Melanin is controlled via genes. If the pawn has one, use it. Otherwise just take whatever skinColorBase the pawn has.
            /*
            if (copy.genes?.GetMelaninGene() != null)
            {
                copy.genes.GetMelaninGene().skinColorBase = pawn.genes.GetMelaninGene().skinColorBase;
            }
            else
            {
                copy.story.skinColorOverride = pawn.story?.skinColorOverride;
                copy.story.SkinColorBase = pawn.story.SkinColorBase;
            }
            */

            // Get rid of any items it may have spawned with.
            ghost?.equipment?.DestroyAllEquipment();
            ghost?.apparel?.DestroyAll();
            ghost?.inventory?.DestroyAll();

            // Copy the pawn's physical attributes.
            ghost.story.bodyType = pawn.story.bodyType;

            Duplicate(pawn, ghost);

            // Spawn the copy.
            GenSpawn.Spawn(ghost, pawn.Position, pawn.Map);

            // Draw the copy.
            ghost.Drawer.renderer.graphics.ResolveAllGraphics();
            return ghost;
        }
    }
}
