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
        public List<Pawn> HumanGhosts = new List<Pawn>();
        public Dictionary<string, Texture2D[]> GhostTextures = new Dictionary<string, Texture2D[]>();

        public List<Pawn> SpawnedHumanGhosts = new List<Pawn>();

        public List<Pawn> AnimalSpirits = new List<Pawn>();

        public Color debugColor1 = new Color(0.145f, 0.588f, 0.745f, 1f);
        public Color debugColor2 = new Color(0.945f, 0.288f, 0.145f, 1f);

        public GameComponent_StoreGhostPawns(Game game)
        {

        }

        public override void GameComponentTick()
        {
            base.GameComponentTick();

            if (GhostTextures.Count > 0)
            {
                for (int i = 0; i < GhostTextures.Count; i++)
                {

                }
            }

            /*if (!HumanGhosts.NullOrEmpty())
            {
                //Log.Message("Cached Ghosts: " + HumanGhosts.Count().ToString().Colorize(debugColor1));
                for (int i = 0; i < HumanGhosts.Count; i++)
                {
                    Log.Message(HumanGhosts[i].Name.ToString().Colorize(debugColor1) + " BodyTypeDef equates to: " + HumanGhosts[i].kindDef.defName.ToString().Colorize(debugColor2));
                }
            }*/
        }

        public override void FinalizeInit()
        {
            base.FinalizeInit();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref HumanGhosts, "HumanGhosts", LookMode.Deep);


            Scribe_Collections.Look(ref AnimalSpirits, "AnimalSpirits", LookMode.Deep);
        }
    }
}
