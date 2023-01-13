using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using UnityEngine;
using UnityEngine.Rendering;
using Verse;
using System.IO;

namespace Ghosts
{
    public class Comp_PawnTextureGeneration : ThingComp
    {
        public CompProperties_PawnTextureGeneration Props => (CompProperties_PawnTextureGeneration)props;

        protected Material pawnMaterial;
        protected Material[] pawnMaterialCopies = new Material[3];
        protected PawnTextureAtlasFrameSet frameSet;

        public Color debugColor1 = new Color(0.545f, 0.388f, 0.645f, 1f);

        /// <summary>
        /// Grabs the frame set of a Pawn which is then used to pull the specific Texture used in that frame. 
        /// 3 copies of the Texture are then made and stored.
        /// </summary>
        private void GetPawnMaterialAndMakeCopies()
        {
            Pawn parentPawn = parent as Pawn;
            if (parentPawn != null && parentPawn.Map != null)
            {
                GlobalTextureAtlasManager.TryGetPawnFrameSet(parentPawn, out frameSet, out var _);
                pawnMaterial = MaterialPool.MatFrom(new MaterialRequest(frameSet.atlas, ShaderDatabase.MoteGlowDistorted));
            }
        }

        private void DrawPawnMaterial()
        {
            Pawn parentPawn = parent as Pawn;
            if (parentPawn != null && parentPawn.Map != null && !parentPawn.Dead && !parentPawn.Downed)
            {
                int index = frameSet.GetIndex(parentPawn.Rotation, PawnDrawMode.BodyAndHead);
                if (frameSet.isDirty[index])
                {
                    Find.PawnCacheCamera.rect = frameSet.uvRects[index];
                    Find.PawnCacheRenderer.RenderPawn(parentPawn, frameSet.atlas, cameraOffset: Vector3.zero, cameraZoom: 1f, angle: 0f, parentPawn.Rotation, renderHead: true, renderBody: true, portrait: false);
                    Find.PawnCacheCamera.rect = new Rect(0f, 1f, 1f, 1f);
                    frameSet.isDirty[index] = false;
                }

                for (int i = 0; i < 3; i++)
                {
                    pawnMaterialCopies[i] = new Material(pawnMaterial);
                    pawnMaterialCopies[i].color = Color.white;
                }

                GenDraw.DrawMeshNowOrLater(
                        frameSet.meshes[index],
                        parentPawn.DrawPos,
                        Quaternion.AngleAxis(0f, Vector3.up),
                        pawnMaterial,
                        drawNow: false
                    );
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            GetPawnMaterialAndMakeCopies();
        }

        public override void PostDraw()
        {
            base.PostDraw();
            DrawPawnMaterial();
        }
    }
}
