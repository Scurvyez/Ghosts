using RimWorld;
using Verse;

namespace Ghosts
{
    [DefOf]
    public static class GhostsDefOf
    {
        public static ThoughtDef SZ_SawGhost_Soothed;
        public static ThoughtDef SZ_SawGhost_Scared;

        public static PawnKindDef SZ_GhostFemaleBody;
        public static PawnKindDef SZ_GhostFatBody;

        static GhostsDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(GhostsDefOf));
        }
    }
}
