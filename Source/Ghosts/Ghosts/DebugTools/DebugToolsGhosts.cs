using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;
using UnityEngine;
using Verse.AI.Group;
using RimWorld.Planet;

namespace Ghosts
{
    public static class DebugToolsGhosts
    {
		[DebugAction("Ghost Spawning", null, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static List<DebugActionNode> SpawnGhost()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			MapComponent_StoreGhostPawns mapComp = Find.CurrentMap.GetComponent<MapComponent_StoreGhostPawns>();
			List<Pawn> savedGhosts = mapComp.Ghosts;

			if (savedGhosts != null)
            {
				foreach (Pawn pawn in savedGhosts)
				{
					Pawn ghosty = pawn;
					list.Add(new DebugActionNode(ghosty.Name.ToString(), DebugActionType.ToolMap)
					{
						action = delegate
						{
							Pawn ghostToSpawn = PawnGenerator.GeneratePawn(ghosty.kindDef, ghosty.Faction);
							GenSpawn.Spawn(ghostToSpawn, UI.MouseCell(), Find.CurrentMap);
						}
					});
				}
			}
			return list;
		}
	}
}
