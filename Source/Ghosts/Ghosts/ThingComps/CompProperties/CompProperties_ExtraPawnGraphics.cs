using UnityEngine;
using Verse;

namespace Ghosts
{
    public class CompProperties_ExtraPawnGraphics : CompProperties
    {
        public Color _Color = new Color(2f, 2f, 2f, 0.75f);
        public float _FlowDetail = 0.06f;
        public float _FlowSpeed = 0.04f;
        public float _FlowMapScale = 0.5f;
        public float _TransparencySpeed = 0.009f;
        public float _TransparencyMapScale = 0.9f;
        public Color _TransparencyTint = new Color(1f, 1f, 1.5f, 1f);
        public float _Brightness = 0.75f;
        public float _BlackLevel = 0.0005f;
        
        public CompProperties_ExtraPawnGraphics()
        {
            compClass = typeof(Comp_ExtraPawnGraphics);
        }
    }
}
