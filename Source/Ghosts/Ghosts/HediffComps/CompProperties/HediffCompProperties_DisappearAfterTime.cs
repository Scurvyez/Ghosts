using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;

namespace Ghosts
{
    public class HediffCompProperties_DisappearAfterTime : HediffCompProperties
    {
        public int ticksTillGone = 4000;

        public HediffCompProperties_DisappearAfterTime()
        {
            compClass = typeof(HediffComp_DisappearAfterTime);
        }
    }
}
