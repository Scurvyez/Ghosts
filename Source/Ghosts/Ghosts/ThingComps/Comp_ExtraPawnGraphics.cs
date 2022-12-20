using System;
using System.Collections.Generic;
using System.Linq;

using Verse;
using UnityEngine;
using RimWorld;

namespace Ghosts
{
    public class Comp_ExtraPawnGraphics : ThingComp
    {
        public CompProperties_ExtraPawnGraphics Props => (CompProperties_ExtraPawnGraphics)props;

        public override void PostDraw()
        {
            base.PostDraw();
            Pawn parentPawn = parent as Pawn;

            if (parentPawn != null)
            {
                PawnKindDef pawnKind = parentPawn.kindDef;

                Vector2 drawSize = pawnKind.lifeStages[parentPawn.ageTracker.CurLifeStageIndex].bodyGraphicData.Graphic.drawSize;
                Vector3 drawPos = parent.DrawPos;
                Rot4 rotation = parent.Rotation;

                if (Props.graphicsExtra != null)
                {
                    for (int i = 0; i < Props.graphicsExtra.Count; i++)
                    {
                        Props.graphicsExtra[i].Graphic.drawSize = drawSize;

                        Props.graphicsExtra[i].Graphic.Draw(
                            (
                            drawPos + Props.graphicsExtra[i].drawOffset),
                            rotation,
                            parent
                            );
                    }
                }
            }
        }
    }
}
