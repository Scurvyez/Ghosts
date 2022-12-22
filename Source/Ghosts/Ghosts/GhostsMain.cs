using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;
using UnityEngine;
using HarmonyLib;

namespace Ghosts
{
    [StaticConstructorOnStartup]
    public static class GhostsMain
    {
        static GhostsMain()
        {
            Log.Message("<color=white>[</color>" + "<color=#4494E3FF>Steve</color>" + "<color=white>]</color>" +
                "<color=white>[</color>" + "<color=#4494E3FF>Ghosts</color>" + "<color=white>]</color>");

            var harmony = new Harmony("com.ghosts");
            harmony.PatchAll();
        }
    }
}
