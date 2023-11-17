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
        public static void Duplicate(Pawn sourcePawn, Pawn destinationPawn)
        {
            try
            {
                // Copy the pawn's name
                NameTriple sourcePawnName = (NameTriple)sourcePawn.Name;
                destinationPawn.Name = sourcePawnName;

                DuplicatePawnIdeology(ref sourcePawn, ref destinationPawn);
                DuplicatePawnRelations(ref sourcePawn, ref destinationPawn);
                DuplicatePawnAppearance(ref sourcePawn, ref destinationPawn);

                sourcePawn.Drawer.renderer.graphics.ResolveAllGraphics();
            }
            catch (Exception exception)
            {
                Log.Error("[<color=#4494E3FF>Ghosts</color>] PawnDataDuplication.Duplicate: Error occurred duplicating " 
                    + sourcePawn + " into " + destinationPawn + ". This will have severe consequences. " 
                    + exception.Message + exception.StackTrace);
            }
        }

        public static void DuplicatePawnIdeology(ref Pawn sourcePawn, ref Pawn destinationPawn)
        {
            try
            {
                if (ModsConfig.IdeologyActive)
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
            }
            catch (Exception exception)
            {
                Log.Warning("[<color=#4494E3FF>Ghosts</color>] An unexpected error occurred during ideology duplication between " 
                    + sourcePawn + " " + destinationPawn + ". The destination IdeoTracker may be left unstable!" 
                    + exception.Message + exception.StackTrace);
            }
        }

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
                Log.Warning("[<color=#4494E3FF>Ghosts</color>] An unexpected error occurred during relation duplication between " 
                    + sourcePawn + " " + destinationPawn + ". The destination RelationTracker may be left unstable!" 
                    + exception.Message + exception.StackTrace);
            }
        }

        public static void DuplicatePawnAppearance(ref Pawn sourcePawn, ref Pawn destinationPawn)
        {
            try
            {
                if (sourcePawn.story != null)
                {
                    destinationPawn.story.bodyType = sourcePawn.story.bodyType;
                    destinationPawn.story.headType = sourcePawn.story.headType;
                    destinationPawn.story.skinColorOverride = sourcePawn.story.skinColorOverride;
                    destinationPawn.story.hairDef = sourcePawn.story.hairDef;
                    destinationPawn.story.HairColor = sourcePawn.story.HairColor;
                    destinationPawn.story.furDef = sourcePawn.story.furDef;
                }
                if (sourcePawn.apparel != null)
                {
                    for (int i = 0; i < sourcePawn.apparel.WornApparel.Count; i++)
                    {
                        Apparel apparel = sourcePawn.apparel.WornApparel[i];
                        destinationPawn.apparel.WornApparel.Add(apparel);
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Warning("[<color=#4494E3FF>Ghosts</color>] An unexpected error occurred during appearance duplication between " 
                    + sourcePawn + " " + destinationPawn + ". The destination story and or apparel trackers may be left unstable!" 
                    + exception.Message + exception.StackTrace);
            }
        }

        public static Pawn SpawnCopy(Pawn pawn, IntVec3 position, Map map)
        {
            // Placeholder pawn object generation
            PawnGenerationRequest request = new PawnGenerationRequest(
                GhostsDefOf.SZ_GhostBaseKind, pawn.Faction, PawnGenerationContext.NonPlayer, forceGenerateNewPawn: true,
                canGeneratePawnRelations: false, allowFood: false, allowAddictions: false, fixedBiologicalAge: 0, fixedChronologicalAge: 0, fixedGender: pawn.gender,
                fixedIdeo: null, forceNoIdeo: true, forceBaselinerChance: 1f);

            Pawn ghost = new Pawn();

            // Dupe the pawn, contains PawnAtlas for graphics, ideo, relations, etc
            Duplicate(pawn, ghost);

            // Spawn the copy.
            GenSpawn.Spawn(ghost, position, map);

            return ghost;
        }
    }
}
