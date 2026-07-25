Shader "WanderSoul/UI/LogoGlow"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _GlowColor ("Glow Color", Color) = (0.12, 0.65, 1.0, 0.5)
        _GlowCenter ("Glow Center", Vector) = (0.5, 0.5, 0, 0)
        _GlowWidth ("Glow Width", Range(0.01, 1)) = 0.55
        _GlowHeight ("Glow Height", Range(0.01, 1)) = 0.22
        _GlowSoftness ("Glow Softness", Range(0.01, 1)) = 0.55
        _GlowStrength ("Glow Strength", Range(0, 3)) = 0.7
        _PulseSpeed ("Pulse Speed", Range(0, 5)) = 0.65
        _PulseAmount ("Pulse Amount", Range(0, 1)) = 0.18
        _FlameStrength ("Flame Strength", Range(0, 0.5)) = 0.12
        _FlameSpeed ("Flame Speed", Range(0, 3)) = 0.25
        _FlameScale ("Flame Scale", Range(0.5, 10)) = 3
        [HideInInspector] _EffectTime ("Effect Time", Float) = 0
        [HideInInspector] _EffectAlpha ("Effect Alpha", Range(0, 1)) = 0

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
        Blend SrcAlpha One
        ColorMask [_ColorMask]

        Pass
        {
            Name "LogoGlow"

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
            fixed4 _GlowColor;
            float4 _GlowCenter;
            float4 _ClipRect;
            float _GlowWidth;
            float _GlowHeight;
            float _GlowSoftness;
            float _GlowStrength;
            float _PulseSpeed;
            float _PulseAmount;
            float _FlameStrength;
            float _FlameSpeed;
            float _FlameScale;
            float _EffectTime;
            float _EffectAlpha;

            v2f vert(appdata_t input)
            {
                v2f output;
                output.worldPosition = input.vertex;
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.texcoord = input.texcoord;
                output.color = input.color;
                return output;
            }

            fixed4 frag(v2f input) : SV_Target
            {
                float2 centeredUv = input.texcoord - _GlowCenter.xy;
                float safeWidth = max(_GlowWidth, 0.001);
                float safeHeight = max(_GlowHeight, 0.001);

                // Apply the animated distortion only above the ellipse center.
                // Two slow waves keep the glow organic without making it look like a sharp fire sprite.
                float upperMask = smoothstep(0.0, safeHeight, centeredUv.y);
                float horizontalPosition = centeredUv.x / safeWidth;
                float flameTime = _EffectTime * _FlameSpeed * 6.2831853;
                float firstWave = sin(horizontalPosition * _FlameScale * 6.2831853 + flameTime);
                float secondWave = sin(horizontalPosition * (_FlameScale * 1.73) * 6.2831853 - flameTime * 0.63);
                float flameWave = firstWave * 0.65 + secondWave * 0.35;
                float flameOffset = flameWave * _FlameStrength * safeHeight * upperMask;

                float2 ellipsePosition = float2(
                    centeredUv.x / safeWidth,
                    (centeredUv.y - flameOffset) / safeHeight);

                float distanceFromCenter = length(ellipsePosition);
                float innerRadius = max(1.0 - _GlowSoftness, 0.0);
                float radialGlow = 1.0 - smoothstep(innerRadius, 1.0, distanceFromCenter);

                float pulse = 1.0 + sin(_EffectTime * _PulseSpeed * 6.2831853) * _PulseAmount;
                float intensity = radialGlow * _GlowStrength * pulse * saturate(_EffectAlpha);
                float alpha = saturate(_GlowColor.a * intensity * input.color.a);

                #ifdef UNITY_UI_CLIP_RECT
                alpha *= UnityGet2DClipping(input.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip(alpha - 0.001);
                #endif

                return fixed4(_GlowColor.rgb * input.color.rgb, alpha);
            }
            ENDCG
        }
    }
}
