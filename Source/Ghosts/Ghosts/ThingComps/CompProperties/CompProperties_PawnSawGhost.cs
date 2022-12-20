using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;
using UnityEngine;

namespace Ghosts
{
    public class CompProperties_PawnSawGhost : CompProperties
    {
        public List<GraphicData> graphicsSawGhost = null;

        public CompProperties_PawnSawGhost() => compClass = typeof(Comp_PawnSawGhost);
    }
}
