using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;

namespace Ghosts
{
    public class StatWorker_Clairvoyance : StatWorker
    {
        public override bool ShouldShowFor(StatRequest req)
        {
            if (!base.ShouldShowFor(req))
            {
                return false;
            }
            if (!(req.Thing is Pawn pawn))
            {
                return false;
            }

            PreceptDef precept = GhostsDefOf.SZ_RoleMedium;
            if (pawn.Ideo.GetRole(pawn).def == precept)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
