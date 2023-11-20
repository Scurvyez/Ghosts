using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using Verse;

namespace Ghosts
{
    [StaticConstructorOnStartup]
    public class Comp_ExtraPawnGraphics : ThingComp
    {
        public CompProperties_ExtraPawnGraphics Props => (CompProperties_ExtraPawnGraphics)props;
        public GameComponent_StoreGhostPawns GameComp = Current.Game.GetComponent<GameComponent_StoreGhostPawns>();
        private Texture2D[] Textures = new Texture2D[4];
        private Texture2D FlowMap = ContentFinder<Texture2D>.Get("Other/Ripples", true);
        
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            Ghost parentPawn = parent as Ghost;

            if (parentPawn != null)
            {
                KeyValuePair<Name, Texture2D[]> ghostKeyValuePair = parentPawn.GhostKeyValuePair;
                Textures = ghostKeyValuePair.Value;
            }
        }

        public override void PostDraw()
        {
            base.PostDraw();

            if (parent is Ghost parentPawn)
            {
                //Vector3 drawPos = parent.DrawPos;

                if (Textures != null && Textures.Length > 0)
                {
                    // Determine the index based on the pawn's rotation
                    int index = GetIndex(parent.Rotation);

                    // Setup the material + shader properties
                    Material ghostMat = new Material(GhostsContentDatabase.GhostEffect);
                    ghostMat.SetTexture("_MainTex", Textures[index]);
                    ghostMat.SetColor("_Color", new Color(2f, 2f, 2f, 0.75f));
                    ghostMat.SetTexture("_FlowMap", FlowMap);
                    ghostMat.SetFloat("_FlowDetail", 0.06f);
                    ghostMat.SetFloat("_FlowSpeed", 0.04f);
                    ghostMat.SetFloat("_FlowMapScale", 0.5f);
                    ghostMat.SetTexture("_TransparencyMap", FlowMap);
                    ghostMat.SetFloat("_TransparencySpeed", 0.009f);
                    ghostMat.SetFloat("_TransparencyMapScale", 0.9f);
                    ghostMat.SetColor("_TransparencyTint", new Color(1f, 1f, 1.5f, 1f));
                    ghostMat.SetFloat("_Brightness", 0.75f);
                    ghostMat.SetFloat("_BlackLevel", 0.0005f);

                    // Calculate the rotation angle based on the pawn's rotation
                    float rotationAngle = 0f;
                    switch (parent.Rotation.AsInt)
                    {
                        case 1: rotationAngle = 0f; break;  // East
                        case 2: rotationAngle = 0f; break; // South
                        case 3: rotationAngle = 0f; break; // West
                    }

                    Vector3 drawPos = parent.DrawPos;
                    drawPos.y = AltitudeLayer.VisEffects.AltitudeFor();
                    Matrix4x4 matrix = Matrix4x4.TRS(drawPos, Quaternion.Euler(0f, rotationAngle, 0f), new Vector3(1f, 1f, 1f));
                    Graphics.DrawMesh(MeshPool.plane20, matrix, ghostMat, 0);
                }
            }
        }

        public int GetIndex(Rot4 rotation)
        {
            // Assign the index based on the rotation (assuming textures are ordered as north, east, south, west)
            switch (rotation.AsInt)
            {
                case 0: return 0; // North
                case 1: return 1; // East
                case 2: return 2; // South
                case 3: return 3; // West
                default: return 0;
            }
        }
    }
}
