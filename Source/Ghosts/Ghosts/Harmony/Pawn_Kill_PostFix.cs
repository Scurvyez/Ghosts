using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;

namespace Ghosts
{
    [HarmonyPatch(typeof(Pawn), "Kill")]
    public static class Kill_PostFix
    {
        /// <summary>
        /// Checks whenever a pawn dies on the map and creates a new, placeholder "ghost" pawn.
        /// Certain information from the dead pawn is duplicated over to the placeholder pawn.
        /// Once that is done, the placeholder pawn becomes an actual ghost of the dead pawn with the same name.
        /// This ghost is then stored for later use inside a MapComponent.
        /// </summary>
        [HarmonyPostfix]
        public static void GenerateGhostWhenPawnDies(Pawn __instance, DamageInfo? dinfo, Hediff exactCulprit = null)
        {
            Color debugColor1 = new Color(0.545f, 0.388f, 0.645f, 1f);
            Color debugColor2 = new Color(0.845f, 0.388f, 0.245f, 1f);

            // if the map exists then generate a placeholder pawn object, dupe the dead pawns data, transfer to the placeholder pawn, and save for later use
            if (__instance.IsColonist && __instance.MapHeld != null)
            {
                // placeholder pawn object generation
                PawnGenerationRequest request = new PawnGenerationRequest(
                    GhostsDefOf.SZ_GhostBaseKind, __instance.Faction, PawnGenerationContext.NonPlayer, forceGenerateNewPawn: true, 
                    canGeneratePawnRelations: false, allowFood: false, allowAddictions: false, fixedBiologicalAge: 0, fixedChronologicalAge: 0, fixedGender: __instance.gender, 
                    fixedIdeo: null, forceNoIdeo: true, forceBaselinerChance: 1f);

                // set the ghost pawnKind to that of the matching dead pawns' bodyType
                /*if (__instance.story.bodyType == BodyTypeDefOf.Female)
                {
                    request.KindDef = GhostsDefOf.SZ_GhostFemaleBody;
                }
                else if (__instance.story.bodyType == BodyTypeDefOf.Fat)
                {
                    request.KindDef = GhostsDefOf.SZ_GhostFatBody;
                }
                else
                {
                    request.KindDef = GhostsDefOf.SZ_GhostHulkBody;
                }*/

                Pawn ghost = PawnGenerator.GeneratePawn(request);

                // this ensures the placeholder pawn doesn't spawn with apparel as it's not needed here
                ghost.apparel?.DestroyAll();

                // name of temp pawn before duping
                Log.Message("Successful generation of: " + ghost.Name.ToString().Colorize(debugColor1) + " , as a placeholder pawn");

                PawnDataDuplication.Duplicate(__instance, ghost);

                // name of finalized pawn after duping for storage in MapComp
                Log.Message("All pertinent data for " + ghost.Name.ToString().Colorize(debugColor2) + " duplicated successfully.");

                // storage in MapComp
                // DEPRECATED __instance.MapHeld.GetComponent<MapComponent_StoreGhostPawns>().HumanGhosts.Add(ghost);
                Current.Game.GetComponent<GameComponent_StoreGhostPawns>().HumanGhosts.Add(ghost);
            }
        }
    }
}
