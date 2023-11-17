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
            Color debugColor1 = new Color(0.145f, 0.588f, 0.745f, 1f);
            Color debugColor2 = new Color(0.545f, 0.388f, 0.645f, 1f);

            // If the map exists then generate a placeholder pawn object, dupe the dead pawns data, transfer to the placeholder pawn, and save for later use
            if (__instance.IsColonist && __instance.MapHeld != null)
            {
                // Placeholder pawn object generation

                PawnGenerationRequest request = new PawnGenerationRequest(
                    GhostsDefOf.SZ_GhostBaseKind, __instance.Faction, PawnGenerationContext.NonPlayer, forceGenerateNewPawn: true, 
                    canGeneratePawnRelations: false, allowFood: false, allowAddictions: false, fixedBiologicalAge: 0, fixedChronologicalAge: 0, fixedGender: __instance.gender, 
                    fixedIdeo: null, forceNoIdeo: true, forceBaselinerChance: 1f);

                // Generate pawn and store in GameComp, ensure no apparel spawwns too
                Pawn ghost = PawnGenerator.GeneratePawn(request);

                // Duplicate all pertinent data
                PawnDataDuplication.Duplicate(__instance, ghost);

                Current.Game.GetComponent<GameComponent_StoreGhostPawns>().HumanGhosts.Add(ghost);

                // Let's check some stuff...
                Log.Message("__instance name: " + __instance.Name.ToString().Colorize(debugColor1));
                Log.Message("ghost name: " + ghost.Name.ToString().Colorize(debugColor1));
                Log.Message("__instance bodyType: " + __instance.story.bodyType.ToString().Colorize(debugColor1));
                Log.Message("ghost bodyType: " + ghost.story.bodyType.ToString().Colorize(debugColor1));
                Log.Message("__instance Ideo: " + __instance.Ideo.ToString().Colorize(debugColor1));
                Log.Message("ghost Ideo: " + ghost.Ideo.ToString().Colorize(debugColor1));
                Log.Message("__instance Faction: " + __instance.Faction.ToString().Colorize(debugColor1));
                Log.Message("ghost Faction: " + ghost.Faction.ToString().Colorize(debugColor1));
                Log.Message("__instance gender: " + __instance.gender.ToString().Colorize(debugColor1));
                Log.Message("ghost gender: " + ghost.gender.ToString().Colorize(debugColor1));

                Log.Message("All pertinent data for " + ghost.Name.ToString().Colorize(debugColor2) + " duplicated successfully.");
            }
        }
    }
}
