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
    public class CompProperties_GiveThought : CompProperties
    {
        public int radius = 1;
        public int tickInterval = 1200;

        public ThoughtDef goodThoughtDef = null;
        public ThoughtDef badThoughtDef = null;

        public CompProperties_GiveThought() => compClass = typeof(Comp_GiveThought);
    }
}
