using System.Collections.Generic;
using System.Linq;
using Verse;
using UnityEngine;

namespace Ghosts
{
    public static class DebugToolsGhosts
    {
		[DebugAction("Ghost Utils", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> SpawnGhost()
		{
            Color debugColor1 = new Color(0.145f, 0.588f, 0.745f, 1f);
            List<DebugActionNode> list = new List<DebugActionNode>();
			GameComponent_StoreGhostPawns gameComp = Current.Game.GetComponent<GameComponent_StoreGhostPawns>();

            if (gameComp != null)
			{
                List<Pawn> savedGhosts = gameComp.HumanGhosts;
                if (!savedGhosts.NullOrEmpty())
                {
                    Log.Message("Cached Ghosts: " + savedGhosts.Count().ToString().Colorize(debugColor1));
                    foreach (Pawn pawn in savedGhosts)
                    {
                        if (pawn != null && pawn.kindDef != null && pawn.Faction != null)
                        {
                            Pawn ghosty = pawn;
                            list.Add(new DebugActionNode(ghosty.Name.ToString(), DebugActionType.ToolMap)
                            {
                                action = delegate
                                {
                                    Pawn ghostToSpawn = PawnGenerator.GeneratePawn(ghosty.kindDef, ghosty.Faction);
                                    GenSpawn.Spawn(ghostToSpawn, UI.MouseCell(), Find.CurrentMap);
                                    //PawnDataDuplication.SpawnCopy(ghostToSpawn);
                                }
                            });
                        }
                    }
                }
            }
            return list;
		}
	}
}
