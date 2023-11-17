using System.Collections.Generic;
using System.Linq;
using Verse;
using UnityEngine;

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

            if (gameComp != null && !gameComp.HumanGhosts.NullOrEmpty())
			{
                //Log.Message("Cached Ghosts: " + gameComp.HumanGhosts.Count().ToString().Colorize(debugColor1));
                foreach (Pawn pawn in gameComp.HumanGhosts)
                {
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
                }
            }
            return list;
		}
	}
}
