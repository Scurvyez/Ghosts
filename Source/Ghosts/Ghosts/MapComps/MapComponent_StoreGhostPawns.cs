using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;
using UnityEngine;

namespace Ghosts
{
    /// <summary>
    /// This class simply stores ghosts for later use in numerous methods.
    /// </summary>
    public class MapComponent_StoreGhostPawns : MapComponent
    {
        public List<Pawn> Ghosts = new List<Pawn>();
        public Color debugColor1 = new Color(0.145f, 0.588f, 0.745f, 1f);

        public MapComponent_StoreGhostPawns(Map map) : base(map) { }

        public override void MapComponentTick()
        {
            base.MapComponentTick();

            Log.Message("Cached Ghosts: " + Ghosts.Count().ToString().Colorize(debugColor1));
        }

        public override void FinalizeInit()
        {
            base.FinalizeInit();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref Ghosts, "Ghosts", LookMode.Deep);
        }
    }
}
