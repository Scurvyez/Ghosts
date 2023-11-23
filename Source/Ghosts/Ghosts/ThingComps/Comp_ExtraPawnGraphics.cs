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
                    ghostMat.SetColor("_Color", Props._Color);
                    ghostMat.SetTexture("_FlowMap", FlowMap);
                    ghostMat.SetFloat("_FlowDetail", Props._FlowDetail);
                    ghostMat.SetFloat("_FlowSpeed", Props._FlowSpeed);
                    ghostMat.SetFloat("_FlowMapScale", Props._FlowMapScale);
                    ghostMat.SetTexture("_TransparencyMap", FlowMap);
                    ghostMat.SetFloat("_TransparencySpeed", Props._TransparencySpeed);
                    ghostMat.SetFloat("_TransparencyMapScale", Props._TransparencyMapScale);
                    ghostMat.SetColor("_TransparencyTint", Props._TransparencyTint);
                    ghostMat.SetFloat("_Brightness", Props._Brightness);
                    ghostMat.SetFloat("_BlackLevel", Props._BlackLevel);

                    // Calculate the rotation angle based on the pawn's rotation
                    float rotationAngle = 0f;
                    switch (parent.Rotation.AsInt)
                    {
                        case 1: rotationAngle = 0f; break; // East
                        case 2: rotationAngle = 0f; break; // South
                        case 3: rotationAngle = 0f; break; // West
                    }

                    Vector3 drawPos = parent.DrawPos;
                    int texWidth = Textures[index].width;
                    float meshScalingFactor = texWidth / 60f;
                    // 60 instead of 64 for a little padding since ghost texture edges get shrunk by the shader
                    drawPos.y = AltitudeLayer.VisEffects.AltitudeFor();
                    Matrix4x4 matrix = Matrix4x4.TRS(drawPos, Quaternion.Euler(0f, rotationAngle, 0f), new Vector3(1f, 1f, 1f));
                    Graphics.DrawMesh(MeshPool.GetMeshSetForWidth(meshScalingFactor).MeshAt(parent.Rotation), matrix, ghostMat, 0);
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
                case 3: return 1; // West, mismatch here because of the MeshAt() above
                default: return 0;
            }
        }
    }
}
