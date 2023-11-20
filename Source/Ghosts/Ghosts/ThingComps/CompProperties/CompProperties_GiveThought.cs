using RimWorld;
using Verse;

namespace Ghosts
{
    public class CompProperties_GiveThought : CompProperties
    {
        public int radius = 1;
        public int tickInterval = 1200;

        public ThoughtDef goodThoughtDef = null;
        public ThoughtDef badThoughtDef = null;
        public ThoughtDef familialThoughtDef = null;
        public ThoughtDef loverThoughtDef = null;

        public CompProperties_GiveThought() => compClass = typeof(Comp_GiveThought);
    }
}
