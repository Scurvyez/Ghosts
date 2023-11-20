using UnityEngine;
using Verse;

namespace Ghosts
{
    public class Comp_GhostGraphics : ThingComp
    {
        public CompProperties_GhostGraphics Props => (CompProperties_GhostGraphics)props;
        public GameComponent_StoreGhostPawns GameComp;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            GameComp = Current.Game.GetComponent<GameComponent_StoreGhostPawns>();

            if (GameComp != null)
            {
                Pawn parentPawn = parent as Pawn;

            }
        }

        public override void PostDraw()
        {
            base.PostDraw();
            Pawn parentPawn = parent as Pawn;

            if (parentPawn != null)
            {
                if (GameComp != null)
                {
                    Texture2D[] ghostTextures = GameComp.GhostTextures[parentPawn.Name];

                    if (ghostTextures.Length > 0)
                    {
                        Rot4 rotation = parentPawn.Rotation;
                        int index = GetIndex(rotation);

                        if (index >= 0 && index < ghostTextures.Length)
                        {
                            // make new shader! :)
                            Material material = MaterialPool.MatFrom(ghostTextures[index], ShaderDatabase.TransparentPostLight, parentPawn.DrawColor);
                            Vector3 drawPos = parentPawn.DrawPos;
                            Graphics.DrawMesh(MeshPool.plane10, drawPos, rotation.AsQuat, material, 0);
                        }
                    }
                }
            }
        }

        public int GetIndex(Rot4 rotation)
        {
            return rotation.AsInt;
        }
    }
}
