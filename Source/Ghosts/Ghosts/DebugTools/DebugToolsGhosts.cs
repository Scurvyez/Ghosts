using System.Collections.Generic;
using Verse;
using UnityEngine;
using System.IO;
using System;
using System.Linq;

namespace Ghosts
{
    public static class DebugToolsGhosts
    {
        [DebugAction("Ghost Utils", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> SpawnGhost()
		{
            //Color debugColor1 = new Color(0.145f, 0.588f, 0.745f, 1f);
            List<DebugActionNode> list = new List<DebugActionNode>();
			GameComponent_StoreGhostPawns gameComp = Current.Game.GetComponent<GameComponent_StoreGhostPawns>();

            if (gameComp != null && gameComp.AvailableColonistGhosts.Count > 0)
			{
                //Log.Message("Cached Ghosts: " + gameComp.HumanGhosts.Count().ToString().Colorize(debugColor1));
                foreach (var kvp in gameComp.GhostTextures)
                {
                    Name pawnName = kvp.Key;
                    Texture2D[] textures = kvp.Value;

                    list.Add(new DebugActionNode(pawnName.ToString(), DebugActionType.ToolMap)
                    {
                        action = delegate
                        {
                            // You might want to use the textures in some way when spawning the ghost
                            // For now, just print a log message
                            Log.Message($"Spawned ghost for {pawnName} at {UI.MouseCell()}");
                        }
                    });

                    /*
                    if (pawn != null && pawn.kindDef != null && pawn.Faction != null)
                    {
                        Pawn ghost = pawn;
                        list.Add(new DebugActionNode(ghost.Name.ToString(), DebugActionType.ToolMap)
                        {
                            action = delegate
                            {
                                //Pawn ghostToSpawn = PawnGenerator.GeneratePawn(ghost.kindDef, ghost.Faction);
                                //PawnDataDuplication.SpawnCopy(ghostToSpawn, UI.MouseCell(), Find.CurrentMap);
                                GenSpawn.Spawn(ghost, UI.MouseCell(), Find.CurrentMap);
                            }
                        });
                    }
                    */
                }
            }
            return list;
		}

        [DebugAction("Ghost Utils", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
        private static void DumpPawnTexturesForGhosts()
        {
            // Get all colonist pawns spawned on the map
            List<Pawn> colonists = Find.CurrentMap.mapPawns.FreeColonistsSpawned.ToList();

            for ( int i = 0; i < colonists.Count; i++)
            {
                DumpPawnTextures(colonists[i]);
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
                        SaveCachedTextureToFile(finalPawnTexture, pawn, i);
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
    }
}
