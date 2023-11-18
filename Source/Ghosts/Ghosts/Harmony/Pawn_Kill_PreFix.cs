using Verse;
using HarmonyLib;
using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ghosts
{
    [StaticConstructorOnStartup]
    [HarmonyPatch(typeof(Pawn), "Kill")]
    public static class Kill_PreFix
    {
        [HarmonyPrefix]
        public static void GenerateGhostWhenPawnDies(Pawn __instance, DamageInfo? dinfo, Hediff exactCulprit = null)
        {
            if (__instance.Spawned && __instance.IsColonist) // change in the future to allow non-colonist ghosts?
            {
                DumpPawnTextures(__instance);
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
                    for (int i = 0; i < 4; i++)
                    {
                        RenderTexture pawnTextureAtlas = frameSet.atlas;
                        Texture2D finalPawnTexture = ToTexture2D(pawnTextureAtlas, frameSet.uvRects[i]);

                        //SaveCachedTextureToFile(finalPawnTexture, pawn, i);
                    }
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

        /*
        public static void SaveCachedTextureToFile(Texture2D texture, Pawn pawn, int frameIndex)
        {
            if (texture != null)
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string folderPath = Path.Combine(desktopPath, "GhostTextures");
                string fileName = $"{pawn.Name.ToStringShort}_GhostTexture_{frameIndex}.png"; // change to a unique ID instead of Name
                string filePath = Path.Combine(folderPath, fileName);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                byte[] bytes = ImageConversion.EncodeToPNG(texture);
                File.WriteAllBytes(filePath, bytes);
            }
        }
        */
    }
}
