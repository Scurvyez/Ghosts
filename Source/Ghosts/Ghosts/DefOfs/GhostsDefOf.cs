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
        public static PawnKindDef SZ_GhostFemaleBody;
        public static PawnKindDef SZ_GhostMaleBody;
        public static PawnKindDef SZ_GhostFatBody;
        public static PawnKindDef SZ_GhostHulkBody;
        public static PawnKindDef SZ_GhostThinBody;
        public static PawnKindDef SZ_GhostBabyBody;
        public static PawnKindDef SZ_GhostChildBody;

        static GhostsDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(GhostsDefOf));
        }
    }
}
