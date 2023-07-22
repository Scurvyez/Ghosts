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
        protected PawnTextureAtlasFrameSet frameSet;

        public Color debugColor1 = new Color(0.545f, 0.388f, 0.645f, 1f);

        /// <summary>
        /// Grabs the frame set of a Pawn which is then used to pull the specific Texture used in that frame. 
        /// 3 copies of the Texture are then made and stored.
        /// </summary>
        private void GetPawnMaterialAndMakeCopies()
        {
            Pawn parentPawn = parent as Pawn;
            if (parentPawn != null && !parentPawn.Downed && !parentPawn.Dead)
            {
                GlobalTextureAtlasManager.TryGetPawnFrameSet(parentPawn, out frameSet, out var _);
            }
        }

        /// <summary>
        /// Takes the Pawns' texture frame set and gets the pixels of the texture for the given frame in-game.
        /// WIP. Makes all the pixels white.
        /// WIP. Blurs those pixels.
        /// </summary>
        private void DrawPawnMaterial()
        {
            Pawn parentPawn = parent as Pawn;
            if (parentPawn != null && !parentPawn.Downed && !parentPawn.Dead)
            {
                if (pawnMaterial == null)
                {
                    Shader shader = ShaderDatabase.MoteGlow;
                    //Color color = new Color(0, 0, 255);
                    pawnMaterial = MaterialPool.MatFrom(new MaterialRequest(frameSet.atlas, shader));
                }

                int index = frameSet.GetIndex(parentPawn.Rotation, PawnDrawMode.BodyAndHead);
                if (frameSet.isDirty[index])
                {
                    Find.PawnCacheCamera.rect = frameSet.uvRects[index];
                    Find.PawnCacheRenderer.RenderPawn(parentPawn, frameSet.atlas, cameraOffset: Vector3.zero, cameraZoom: 1f, angle: 0f, parentPawn.Rotation, renderHead: true, renderBody: true, portrait: true);
                    Find.PawnCacheCamera.rect = new Rect(0f, 1f, 1f, 1f);
                    frameSet.isDirty[index] = false;
                }

                //pawnMaterial.color = new Color(pawnMaterial.color.r, pawnMaterial.color.g, pawnMaterial.color.b, 1f);
                pawnMaterial.color = new Color(255, 255, 255, 1f);

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
            DrawPawnMaterial();
        }
    }
}
