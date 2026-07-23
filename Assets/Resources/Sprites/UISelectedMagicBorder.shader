Shader "WanderSoul/UI/SelectedMagicBorder"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (0.02, 0.22, 0.35, 0.75)
        _FlowColor ("Flow Color", Color) = (0.10, 0.85, 1.00, 1.00)
        _BorderWidth ("Border Width", Range(0.005, 0.2)) = 0.035
        _GlowWidth ("Glow Width", Range(0.005, 0.25)) = 0.08
        _FlowSpeed ("Flow Speed", Range(0, 2)) = 0.55
        _FlowLength ("Flow Length", Range(0.02, 0.8)) = 0.24
        _GlowStrength ("Glow Strength", Range(0, 5)) = 2.0
        _PulseSpeed ("Pulse Speed", Range(0, 5)) = 1.3
        _NoiseStrength ("Noise Strength", Range(0, 1)) = 0.18

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "SelectedMagicBorder"

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
            };

            sampler2D _MainTex;
            fixed4 _TextureSampleAdd;
            fixed4 _BaseColor;
            fixed4 _FlowColor;
            float4 _ClipRect;
            float _BorderWidth;
            float _GlowWidth;
            float _FlowSpeed;
            float _FlowLength;
            float _GlowStrength;
            float _PulseSpeed;
            float _NoiseStrength;

            v2f vert(appdata_t input)
            {
                v2f output;
                output.worldPosition = input.vertex;
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.texcoord = input.texcoord;
                output.color = input.color;
                return output;
            }

            float GetClockwisePerimeter(float2 uv, float4 edgeDistance)
            {
                float nearest = min(min(edgeDistance.x, edgeDistance.y), min(edgeDistance.z, edgeDistance.w));

                if (nearest == edgeDistance.w) return uv.x * 0.25;
                if (nearest == edgeDistance.z) return 0.25 + (1.0 - uv.y) * 0.25;
                if (nearest == edgeDistance.y) return 0.50 + (1.0 - uv.x) * 0.25;
                return 0.75 + uv.y * 0.25;
            }

            fixed4 frag(v2f input) : SV_Target
            {
                float2 uv = saturate(input.texcoord);
                float4 edgeDistance = float4(uv.x, uv.y, 1.0 - uv.x, 1.0 - uv.y);
                float distanceToEdge = min(min(edgeDistance.x, edgeDistance.y), min(edgeDistance.z, edgeDistance.w));

                float border = 1.0 - smoothstep(_BorderWidth * 0.65, _BorderWidth, distanceToEdge);
                float glow = 1.0 - smoothstep(_BorderWidth, _BorderWidth + _GlowWidth, distanceToEdge);

                float perimeter = GetClockwisePerimeter(uv, edgeDistance);
                float headPosition = frac(_Time.y * _FlowSpeed);
                float trailDistance = frac(headPosition - perimeter + 1.0);
                float trail = 1.0 - smoothstep(0.0, max(_FlowLength, 0.001), trailDistance);
                float head = 1.0 - smoothstep(0.0, 0.035, min(trailDistance, 1.0 - trailDistance));

                float noise = sin(perimeter * 93.0 + _Time.y * 7.0) * 0.5 + 0.5;
                float noiseFactor = lerp(1.0, noise, _NoiseStrength);
                float pulse = 0.82 + sin(_Time.y * _PulseSpeed * 6.28318) * 0.18;

                fixed4 sprite = tex2D(_MainTex, uv) + _TextureSampleAdd;
                float baseIntensity = border * pulse;
                float flowIntensity = saturate(trail * border + head * glow) * noiseFactor * _GlowStrength;

                fixed3 color = _BaseColor.rgb * baseIntensity + _FlowColor.rgb * flowIntensity;
                float alpha = saturate(_BaseColor.a * border + _FlowColor.a * (trail * border + head * glow));
                alpha *= sprite.a * input.color.a;

                #ifdef UNITY_UI_CLIP_RECT
                alpha *= UnityGet2DClipping(input.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip(alpha - 0.001);
                #endif

                return fixed4(color * input.color.rgb, alpha);
            }
            ENDCG
        }
    }
}
