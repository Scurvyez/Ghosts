using UnityEngine;
using Verse;

namespace Ghosts
{
    public class CompProperties_ExtraPawnGraphics : CompProperties
    {
        public float _FlowDetail = 0.06f;
        public float _FlowSpeed = 0.04f;
        public float _FlowMapScale = 0.5f;
        public float _TransparencySpeed = 0.009f;
        public float _TransparencyMapScale = 0.9f;
        public Color _Tint = new Color(1f, 1f, 1.5f, 1f);
        public float _Brightness = 0.75f;
        
        public CompProperties_ExtraPawnGraphics()
        {
            compClass = typeof(Comp_ExtraPawnGraphics);
        }
    }
}
