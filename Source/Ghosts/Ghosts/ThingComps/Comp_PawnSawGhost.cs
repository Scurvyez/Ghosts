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
    public class Comp_PawnSawGhost : ThingComp
    {
        public CompProperties_PawnSawGhost Props => (CompProperties_PawnSawGhost)props;

        public override void PostDraw()
        {
            base.PostDraw();
            Pawn parentPawn = parent as Pawn;

            List<Thought> thoughts = new List<Thought>();
            PawnNeedsUIUtility.GetThoughtGroupsInDisplayOrder(parentPawn.needs.mood, thoughts);

            if (parentPawn != null)
            {
                Vector3 drawPos = parent.DrawPos;
                Rot4 rotation = Rot4.North;

                for (int i = 0; i < thoughts.Count; i++)
                {
                    if (thoughts[i].ToString().Contains("SZ_SawGhost_"))
                    {
                        for (int j = 0; j < Props.graphicsSawGhost.Count; j++)
                        {
                            Props.graphicsSawGhost[j].Graphic.Draw(
                                (
                                drawPos + Props.graphicsSawGhost[j].drawOffset),
                                rotation,
                                parent
                                );
                        }
                    }
                }
            }
        }
    }
}
