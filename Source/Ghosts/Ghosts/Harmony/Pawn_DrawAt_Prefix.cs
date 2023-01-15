using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;
using UnityEngine;
using HarmonyLib;

namespace Ghosts
{
    [HarmonyPatch(typeof(Pawn), "DrawAt")]
    public static class DrawAt_Prefix
    {
        [HarmonyPrefix]
        public static bool DrawOnlyUnderCondition(Pawn __instance, Vector3 drawLoc, bool flip = false)
        {
            Comp_PawnTextureGeneration compTexGen = __instance.GetComp<Comp_PawnTextureGeneration>();

            if (compTexGen != null && __instance.Spawned)
            {
                return false;
            }
            return true;
        }
    }
}
