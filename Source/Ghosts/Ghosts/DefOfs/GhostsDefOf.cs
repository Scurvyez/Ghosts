using RimWorld;
using Verse;

namespace Ghosts
{
    [DefOf]
    public static class GhostsDefOf
    {
        // ThoughDefs
        public static ThoughtDef SZ_SawGhost_Soothed;
        public static ThoughtDef SZ_SawGhost_Scared;

        // PawnKindDefs
        public static PawnKindDef SZ_GhostBaseKind;

        // PreceptDefs
        public static PreceptDef SZ_RoleMedium;

        // StatDefs
        public static StatDef SZ_Clairvoyance;

        // HediffDefs
        public static HediffDef SZ_GhostsDisappearing;

        static GhostsDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(GhostsDefOf));
        }
    }
}
