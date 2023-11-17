using RimWorld;

namespace Ghosts
{
    public class CompProperties_AbilityRaiseDead : CompProperties_AbilityEffect
    {
        public string successMessage; // turn this into a keyed value and just use .Translate()

        public CompProperties_AbilityRaiseDead() => compClass = typeof(Comp_AbilityRaiseDead);
    }
}
