using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using UnityEngine;
using Verse;

namespace Ghosts
{
    public class CompProperties_PawnTextureGeneration : CompProperties
    {
        public CompProperties_PawnTextureGeneration() => compClass = typeof(Comp_PawnTextureGeneration);
    }
}
