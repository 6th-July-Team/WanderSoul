Shader "WanderSoul/UI/BarrierShimmer"
{
    Properties
    {
        [PerRendererData] _MainTex ("Barrier Mask", 2D) = "white" {}
        _BaseColor ("Base Glow Color", Color) = (0.08, 0.65, 0.85, 0.12)
        _ShimmerColor ("Shimmer Color", Color) = (0.55, 0.95, 1.00, 0.85)
        _BaseStrength ("Base Glow Strength", Range(0, 1)) = 0.12
        _ShimmerStrength ("Shimmer Strength", Range(0, 5)) = 1.5
        _BandWidth ("Band Width", Range(0.005, 0.3)) = 0.12
        _BandSoftness ("Band Softness", Range(0.001, 0.3)) = 0.08
        _Diagonal ("Flow Lean", Range(-2, 2)) = 0
        _CurveStrength ("Dome Curve Strength", Range(0, 1)) = 0.75
        _TopWidth ("Top Width", Range(0.05, 1)) = 0.25
        _BottomFadeStart ("Bottom Fade Start", Range(0, 1)) = 0.55
        _BottomFadeEnd ("Bottom Fade End", Range(0, 1)) = 0.05
        _BottomFadePower ("Bottom Fade Power", Range(0.1, 5)) = 1.5
        _BarrierUvRect ("Barrier UV Rect", Vector) = (0.6465, 0.6886, 0.8971, 0.8990)
        _SweepDuration ("Sweep Duration", Range(0.1, 10)) = 2.2
        _CycleDuration ("Cycle Duration", Range(0.2, 20)) = 5.5
        [HideInInspector] _EffectTime ("Effect Time", Float) = 0
        [HideInInspector] _Playing ("Playing", Float) = 0

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
            Name "BarrierShimmer"

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
            fixed4 _ShimmerColor;
            float4 _ClipRect;
            float _BaseStrength;
            float _ShimmerStrength;
            float _BandWidth;
            float _BandSoftness;
            float _Diagonal;
            float _CurveStrength;
            float _TopWidth;
            float _BottomFadeStart;
            float _BottomFadeEnd;
            float _BottomFadePower;
            float4 _BarrierUvRect;
            float _SweepDuration;
            float _CycleDuration;
            float _EffectTime;
            float _Playing;

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
                float2 uv = saturate(input.texcoord);
                fixed4 maskSample = tex2D(_MainTex, uv) + _TextureSampleAdd;
                float surfaceMask = saturate(maskSample.a) * input.color.a * saturate(_Playing);

                // The barrier occupies only a small part of the full-size UI sprite.
                // Remap that area so the shimmer crosses the whole dome surface,
                // instead of spending most of its travel outside the mask.
                float2 barrierSize = max(_BarrierUvRect.zw - _BarrierUvRect.xy, float2(0.0001, 0.0001));
                float2 barrierUv = saturate((uv - _BarrierUvRect.xy) / barrierSize);

                float sweepDuration = max(_SweepDuration, 0.001);
                float cycleDuration = max(_CycleDuration, sweepDuration + 0.001);
                float cycleTime = fmod(max(_EffectTime, 0.0), cycleDuration);
                float sweepActive = 1.0 - step(sweepDuration, cycleTime);
                float progress = saturate(cycleTime / sweepDuration);

                // Treat the filled mask as the front projection of a hemisphere.
                // A vertical reflection on a curved dome is wide near the base and
                // converges toward the pole instead of remaining a straight screen-space band.
                float domeX = barrierUv.x * 2.0 - 1.0;
                float domeY = saturate(barrierUv.y);
                float hemisphereWidth = sqrt(saturate(1.0 - domeY * domeY));
                float curveScale = lerp(1.0, hemisphereWidth, saturate(_CurveStrength));

                float sweepPosition = lerp(-1.0 - _BandWidth, 1.0 + _BandWidth, progress);
                float leanOffset = _Diagonal * (domeY - 0.5) * 0.15;
                float curvedCenter = sweepPosition * curveScale + leanOffset;
                float curvedWidth = _BandWidth * lerp(_TopWidth, 1.0, curveScale);
                float curvedSoftness = _BandSoftness * lerp(_TopWidth, 1.0, curveScale);

                float distanceToBand = abs(domeX - curvedCenter);
                float band = 1.0 - smoothstep(curvedWidth, curvedWidth + curvedSoftness, distanceToBand);

                // Fade only the moving reflection toward the bottom of the dome.
                // The subtle base glow remains visible across the full mask.
                float fadeEnd = min(_BottomFadeEnd, _BottomFadeStart - 0.001);
                float verticalFade = smoothstep(fadeEnd, _BottomFadeStart, domeY);
                verticalFade = pow(saturate(verticalFade), max(_BottomFadePower, 0.001));
                band *= verticalFade;
                band *= sweepActive;

                float baseIntensity = _BaseStrength;
                float shimmerIntensity = band * _ShimmerStrength;
                fixed3 color = _BaseColor.rgb * baseIntensity + _ShimmerColor.rgb * shimmerIntensity;
                float alpha = surfaceMask * saturate(_BaseColor.a * baseIntensity + _ShimmerColor.a * shimmerIntensity);

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
