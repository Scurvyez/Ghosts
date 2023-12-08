using Verse;
using HarmonyLib;
using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

namespace Ghosts
{
    [StaticConstructorOnStartup]
    [HarmonyPatch(typeof(Pawn), "Kill")]
    public static class Kill_PreFix
    {
        [HarmonyPrefix]
        public static void GenerateGhostWhenPawnDies(Pawn __instance, DamageInfo? dinfo, Hediff exactCulprit = null)
        {
            GameComponent_StoreGhostPawns gameComp = Current.Game.GetComponent<GameComponent_StoreGhostPawns>();

            if (__instance.Spawned && __instance.IsColonist) // change in the future to allow non-colonist ghosts?
            {
                DumpPawnTextures(__instance);
                gameComp.AvailableColonistGhosts.Add(__instance.Name);
            }
        }

        private static void DumpPawnTextures(Pawn pawn)
        {
            if (pawn.Spawned)
            {
                PawnTextureAtlasFrameSet frameSet;
                bool sourceFrameSetOut = GlobalTextureAtlasManager.TryGetPawnFrameSet(pawn, out frameSet, out var _);

                if (sourceFrameSetOut)
                {
                    Texture2D[] finalPawnTextures = new Texture2D[4];

                    for (int i = 0; i < 4; i++)
                    {
                        RenderTexture pawnTextureAtlas = frameSet.atlas;
                        finalPawnTextures[i] = ToTexture2D(pawnTextureAtlas, frameSet.uvRects[i]);
                        SaveCachedTextureToFile(finalPawnTextures[i], pawn, i);
                    }
                    SendTexturesToComp(pawn, finalPawnTextures);
                }
            }
        }

        public static Texture2D ToTexture2D(RenderTexture renderTexture, Rect uvRect)
        {
            RenderTexture.active = renderTexture;
            Texture2D pawnTexture = new Texture2D((int)(uvRect.width * renderTexture.width), (int)(uvRect.height * renderTexture.height), TextureFormat.ARGB32, false);
            pawnTexture.ReadPixels(new Rect(uvRect.x * renderTexture.width, ((1f - uvRect.y) - uvRect.height) * renderTexture.height, uvRect.width * renderTexture.width, uvRect.height * renderTexture.height), 0, 0);
            pawnTexture.Apply();

            return pawnTexture;
        }

        /// <summary>
        /// For debugging!
        /// Remember to disable this when all done. :)
        /// Exports generated textures to file on device.
        /// </summary>
        public static void SaveCachedTextureToFile(Texture2D texture, Pawn pawn, int frameIndex)
        {
            if (texture != null)
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string folderPath = Path.Combine(desktopPath, "GhostTextures");
                string fileName = $"{pawn.Name}_GhostTexture_{frameIndex}.png";
                string filePath = Path.Combine(folderPath, fileName);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                byte[] bytes = ImageConversion.EncodeToPNG(texture);
                File.WriteAllBytes(filePath, bytes);
            }
        }

        public static void SendTexturesToComp(Pawn pawn, Texture2D[] textures)
        {
            GameComponent_StoreGhostPawns gameComp = Current.Game.GetComponent<GameComponent_StoreGhostPawns>();

            if (gameComp != null)
            {
                if (!gameComp.GhostTextures.ContainsKey(pawn.Name))
                {
                    gameComp.GhostTextures.Add(pawn.Name, textures);
                }
                else
                {
                    gameComp.GhostTextures[pawn.Name] = textures;
                }
            }
        }
    }
}
