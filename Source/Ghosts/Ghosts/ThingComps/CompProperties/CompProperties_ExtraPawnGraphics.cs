using System.Collections.Generic;

using Verse;

namespace Ghosts
{
    public class CompProperties_ExtraPawnGraphics : CompProperties
    {
        public List<GraphicData> graphicsExtra = null;
        public List<GraphicData> graphicsChristmas = null;

        public CompProperties_ExtraPawnGraphics()
        {
            compClass = typeof(Comp_ExtraPawnGraphics);
        }
    }
}
