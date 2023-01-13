using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;

namespace Ghosts
{
    public class Thought_MemorySawGhost : Thought_MemoryObservation
    {
        public override float MoodOffset()
        {
            if (ThoughtUtility.ThoughtNullified(pawn, def))
            {
                return 0f;
            }

            float num = base.MoodOffset();
            List<Precept> precepts = pawn.Ideo.PreceptsListForReading;
            for (int i = 0; i < precepts.Count; i++)
            {
                if (precepts[i].def == GhostsDefOf.SZ_RoleMedium)
                {
                    num += pawn.GetStatValue(GhostsDefOf.SZ_Clairvoyance);
                }
            }
            return num;
        }
    }
}
