using Verse;

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
                GameComponent_StoreGhostPawns gameComp = Current.Game.GetComponent<GameComponent_StoreGhostPawns>();


            }
        }

        public int GetIndex(Rot4 rotation)
        {
            return rotation.AsInt;
        }
    }
}
