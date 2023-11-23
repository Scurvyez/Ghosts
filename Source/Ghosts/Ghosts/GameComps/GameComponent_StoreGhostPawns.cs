using System.Collections.Generic;
using Verse;
using UnityEngine;

namespace Ghosts
{
    /// <summary>
    /// This class simply stores ghosts for later use in numerous methods.
    /// </summary>
    public class GameComponent_StoreGhostPawns : GameComponent
    {
        public List<Name> AvailableColonistGhosts = new List<Name>();
        public List<Name> SpawnedColonistGhosts = new List<Name>();

        public List<Name> AvailableHosilteGhosts = new List<Name>();
        public List<Name> SpawnedHosilteGhosts = new List<Name>();

        public Dictionary<Name, Texture2D[]> GhostTextures = new Dictionary<Name, Texture2D[]>();

        public GameComponent_StoreGhostPawns(Game game)
        {

        }

        public override void GameComponentTick()
        {
            base.GameComponentTick();
        }

        public override void FinalizeInit()
        {
            base.FinalizeInit();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            // SAVE STUFF! Come back to this later
        }
    }
}
