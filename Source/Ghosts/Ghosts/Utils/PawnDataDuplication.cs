﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;
using UnityEngine;

namespace Ghosts
{
    public class PawnDataDuplication
    {
        /// <summary>
        /// Duplicate a dead pawns ideo, relations, and name.
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

                if (sourcePawn.Faction != destinationPawn.Faction)
                {
                    destinationPawn.SetFaction(sourcePawn.Faction);
                }

                NameTriple sourcePawnName = (NameTriple)sourcePawn.Name;
                destinationPawn.Name = new NameTriple(sourcePawnName.First, sourcePawnName.Nick, sourcePawnName.Last);
                destinationPawn.Drawer.renderer.graphics.ResolveAllGraphics();
            }
            catch(Exception exception)
            {
                Log.Error("[Ghosts] PawnDataDuplication.Duplicate: Error occurred duplicating " + sourcePawn + " into " + destinationPawn + ". This will have severe consequences. " + exception.Message + exception.StackTrace);
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
                Log.Warning("[Ghosts] An unexpected error occurred during ideology duplication between " + sourcePawn + " " + destinationPawn + ". The destination IdeoTracker may be left unstable!" + exception.Message + exception.StackTrace);
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
                Log.Warning("[Ghosts] An unexpected error occurred during relation duplication between " + sourcePawn + " " + destinationPawn + ". The destination RelationTracker may be left unstable!" + exception.Message + exception.StackTrace);
            }
        }
    }
}