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

                if (Props.graphicsExtra != null)
                {
                    for (int i = 0; i < Props.graphicsExtra.Count; i++)
                    {
                        Rot4 rotation = parent.Rotation;
                        Props.graphicsExtra[i].Graphic.drawSize = drawSize;

                        Props.graphicsExtra[i].Graphic.Draw(
                            (
                            drawPos + Props.graphicsExtra[i].drawOffset),
                            rotation,
                            parent
                            );
                    }
                }

                if (Props.graphicsChristmas != null)
                {
                    if (GenLocalDate.Season(parentPawn.Map) == Season.Winter &&
                        (GenLocalDate.DayOfSeason(parentPawn.Map) < 15 && GenLocalDate.DayOfSeason(parentPawn.Map) > 10))
                    {
                        for (int i = 0; i < Props.graphicsChristmas.Count; i++)
                        {
                            Rot4 rotation = Rot4.North;
                            Props.graphicsChristmas[i].Graphic.drawSize = drawSize;

                            Props.graphicsChristmas[i].Graphic.Draw(
                                (
                                drawPos + Props.graphicsChristmas[i].drawOffset),
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
