Shader "Unlit/GhostEffect"
{
    Properties
    {
        [Header(Main)]
        _MainTex ("Main texture", 2D) = "white" {}
        _Tint ("Tint", Vector) = (1,1,1,1)

        [Header(Flow Mapping)]
        _FlowMap ("Flow map", 2D) = "white" {}
        _FlowDetail ("Flow detail", Range(0, 1)) = 0.085
        _FlowSpeed ("Flow speed", Range(0, 1)) = 0.085
        _FlowMapScale ("Flow map scale", float) = 0.5

        [Header(Transparency Mapping)]
        _TransparencyMap ("Transparency map", 2D) = "white" {}
        _TransparencySpeed ("Transparency speed", Range(0, 1)) = 0.085
        _TransparencyMapScale ("Transparency map scale", float) = 0.5

        [Header(Exposure)]
        _Brightness ("Brightness", Range(0.0, 100.0)) = 4
    }

    SubShader
    {
        Tags
        { 
            "Queue" = "Transparent+150" 
            "IgnoreProjector" = "True" 
            "RenderType" = "Transparent"
        }

        Pass
        {
            Tags 
		    { 
                "Queue" = "Transparent+150"
			    "IgnoreProjector" = "true" 
			    "RenderType" = "Transparent"
		    }

            ZWrite Off
            BlendOp Add
			Blend SrcAlpha One

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Tint;

            sampler2D _FlowMap;
            float4 _FlowMap_ST;
            float _FlowDetail;
            float _FlowSpeed;
            float _FlowMapScale;

            sampler2D _TransparencyMap;
            float4 _TransparencyMap_ST;
            float _TransparencySpeed;
            float _TransparencyMapScale;

            float _Brightness;

            float _GameSeconds;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 mainUV : TEXCOORD0; // UV coordinates for the main texture
                float2 flowUV : TEXCOORD1; // UV coordinates for the flow map texture
                float2 transUV : TEXCOORD2; // UV coordinates for the transparency map texture
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata IN)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(IN.vertex);

                // UV coordinates for the main texture
                o.mainUV = TRANSFORM_TEX(IN.uv, _MainTex);

                // Apply flow map scaling only to UV coordinates used for sampling the flow map
                float2 flowUV = (IN.uv - 0.5) / _FlowMapScale + 0.5 + (_Time.y * _FlowSpeed);
                o.flowUV = TRANSFORM_TEX(flowUV, _FlowMap);

                // Apply flow map scaling only to UV coordinates used for sampling the flow map
                float2 transUV = (IN.uv - 0.5) / _TransparencyMapScale + 0.5 + (_Time.y * _TransparencySpeed);
                o.transUV = TRANSFORM_TEX(transUV, _TransparencyMap);

                return o;
            }

            fixed4 frag (v2f IN) : SV_Target
            {
                // sample the flow map so we can get the flow direction (update later to account for direction from all angles)
                float3 flowDir = tex2D(_FlowMap, IN.flowUV).rgb * 2.0f - 1.0f; 
                flowDir *= _FlowDetail; // scale the flow direction based on speed

                // calculate our two phases for the flow animation
                float phase0 = frac(_Time.y * 0.5f + 0.5f);
                float phase1 = frac(_Time.y * 0.5f + 1.0f);

                // sample the main texture at two different phases for the animation
                half3 tex0 = tex2D(_MainTex, IN.mainUV + flowDir.xy * phase0);
                half3 tex1 = tex2D(_MainTex, IN.mainUV + flowDir.xy * phase1);

                // interpolate between the two textures based on the phase, we moving now!
                float flowLerp = abs((0.5f - phase0) / 0.5f);
                half3 tempColor = lerp(tex0, tex1, flowLerp);

                // use the flow map alpha as a mask on the main texture alpha
                float mainTexValue = tex2D(_MainTex, IN.mainUV).a;
                float flowMapValue = tex2D(_FlowMap, IN.flowUV);
                float transparencyMapValue = tex2D(_TransparencyMap, IN.transUV);
                float finalValue = mainTexValue * flowMapValue * transparencyMapValue;

                // convert the color to grayscale
                half luminance = dot(tempColor.rgb, float3(0.299, 0.587, 0.114));
                half3 grayscaleColor = half3(luminance, luminance, luminance);

                // combine the final color with our brightness factor and tint color
                fixed4 c = float4(grayscaleColor, finalValue);
                c *= _Brightness * _Tint;
                return c; // return the final color :)
            }
            ENDCG
        }
    }
}
