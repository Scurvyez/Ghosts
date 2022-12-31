using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;

namespace Ghosts
{
    public class CompProperties_AbilityRaiseDead : CompProperties_AbilityEffect
    {
        public string successMessage;

        public CompProperties_AbilityRaiseDead() => compClass = typeof(Comp_AbilityRaiseDead);
    }
}
