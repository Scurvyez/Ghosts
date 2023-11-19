using System.Collections.Generic;
using Verse;
using UnityEngine;
using System;

namespace Ghosts
{
    /// <summary>
    /// This class simply stores ghosts for later use in numerous methods.
    /// </summary>
    public class GameComponent_StoreGhostPawns : GameComponent
    {
        //public List<Pawn> ColonistGhosts = new List<Pawn>();
        //public List<Pawn> SpawnedColonistGhosts = new List<Pawn>();
        public int ColonistGhosts = 0;
        public int SpawnedColonistGhosts = 0;
        public Dictionary<string, Texture2D[]> GhostTextures = new Dictionary<string, Texture2D[]>();
        
        public Color debugColor1 = new Color(0.145f, 0.588f, 0.745f, 1f);
        public Color debugColor2 = new Color(0.945f, 0.288f, 0.145f, 1f);

        public GameComponent_StoreGhostPawns(Game game)
        {

        }

        public override void GameComponentTick()
        {
            base.GameComponentTick();
            // ensure the number of available ghosts is = to the number of key/value pairs in our Dictionary
            ColonistGhosts = GhostTextures.Count;
        }

        public override void FinalizeInit()
        {
            base.FinalizeInit();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            //Scribe_Collections.Look(ref ColonistGhosts, "HumanGhosts", LookMode.Deep);

            // Serialize and deserialize the GhostTextures dictionary
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                List<string> textureKeys = new List<string>(GhostTextures.Keys);
                List<string> textureValuesBase64 = new List<string>();

                foreach (var kvp in GhostTextures)
                {
                    List<string> textureBase64List = new List<string>();

                    foreach (Texture2D texture in kvp.Value)
                    {
                        string textureBase64 = Convert.ToBase64String(texture.EncodeToPNG());
                        textureBase64List.Add(textureBase64);
                    }

                    textureValuesBase64.Add(string.Join(",", textureBase64List.ToArray()));
                }

                Scribe_Collections.Look(ref textureKeys, "textureKeys", LookMode.Value);
                Scribe_Collections.Look(ref textureValuesBase64, "textureValuesBase64", LookMode.Value);
            }
            else if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                List<string> textureKeys = null;
                List<string> textureValuesBase64 = null;

                Scribe_Collections.Look(ref textureKeys, "textureKeys", LookMode.Value);
                Scribe_Collections.Look(ref textureValuesBase64, "textureValuesBase64", LookMode.Value);

                if (textureKeys != null && textureValuesBase64 != null)
                {
                    GhostTextures.Clear();

                    for (int i = 0; i < textureKeys.Count; i++)
                    {
                        string[] textureBase64Array = textureValuesBase64[i].Split(',');
                        Texture2D[] textures = new Texture2D[textureBase64Array.Length];

                        for (int j = 0; j < textureBase64Array.Length; j++)
                        {
                            byte[] textureBytes = Convert.FromBase64String(textureBase64Array[j]);
                            Texture2D texture = new Texture2D(2, 2); // Replace with the actual dimensions of your textures
                            texture.LoadImage(textureBytes);
                            textures[j] = texture;
                        }

                        GhostTextures.Add(textureKeys[i], textures);
                    }
                }
            }
        }
    }
}
