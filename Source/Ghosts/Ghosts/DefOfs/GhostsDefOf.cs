using RimWorld;

namespace Ghosts
{
    [DefOf]
    public static class GhostsDefOf
    {
        public static ThoughtDef SZ_SawGhost_Soothed;
        public static ThoughtDef SZ_SawGhost_Scared;

        static GhostsDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(GhostsDefOf));
        }
    }
}
