Shader "WanderSoul/UI/OrbLiquid"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _TopColor ("Top Color", Color) = (0.95,0.16,0.12,1)
        _BottomColor ("Bottom Color", Color) = (0.18,0.005,0.01,1)
        _SurfaceColor ("Surface Color", Color) = (1,0.55,0.25,1)
        _FillAmount ("Fill Amount", Range(0,1)) = 1
        _WaveAmplitude ("Wave Amplitude", Range(0,0.05)) = 0.012
        _WaveFrequency ("Wave Frequency", Range(1,30)) = 10
        _WaveSpeed ("Wave Speed", Range(-5,5)) = 1
        _WavePhase ("Wave Phase", Range(0,1)) = 0
        _WaveSpeedMultiplier ("Wave Speed Multiplier", Range(0.5,1.5)) = 1
        _NoiseStrength ("Noise Strength", Range(0,0.3)) = 0.06
        _NoiseSpeed ("Noise Speed", Range(0,3)) = 0.25
        _EdgeDarkness ("Edge Darkness", Range(0,1)) = 0.55
        _InnerGlow ("Inner Glow", Range(0,2)) = 0.25
        _HitImpulse ("Hit Impulse", Range(0,1)) = 0
        _SurfaceWidth ("Surface Width", Range(0.001,0.08)) = 0.018

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
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
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
            Name "OrbLiquid"

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
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            fixed4 _TopColor;
            fixed4 _BottomColor;
            fixed4 _SurfaceColor;
            float4 _ClipRect;
            float _FillAmount;
            float _WaveAmplitude;
            float _WaveFrequency;
            float _WaveSpeed;
            float _WavePhase;
            float _WaveSpeedMultiplier;
            float _NoiseStrength;
            float _NoiseSpeed;
            float _EdgeDarkness;
            float _InnerGlow;
            float _HitImpulse;
            float _SurfaceWidth;

            v2f vert(appdata_t input)
            {
                v2f output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                output.worldPosition = input.vertex;
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.texcoord = input.texcoord;
                output.color = input.color * _Color;
                return output;
            }

            fixed4 frag(v2f input) : SV_Target
            {
                float2 centeredUv = input.texcoord * 2.0 - 1.0;
                float radius = length(centeredUv);
                float sphereMask = 1.0 - smoothstep(0.96, 1.0, radius);

                float waveTime = _Time.y * _WaveSpeed * _WaveSpeedMultiplier;
                float wavePhase = _WavePhase * 6.2831853;
                float wave = sin((input.texcoord.x * _WaveFrequency + waveTime) * 6.2831853 + wavePhase);
                wave += sin((input.texcoord.x * (_WaveFrequency * 0.63) - waveTime * 1.37) * 6.2831853 - wavePhase * 0.73) * 0.45;
                wave *= _WaveAmplitude * (1.0 + _HitImpulse * 2.5);

                float hitWave = sin((input.texcoord.x * 3.0 + _Time.y * 5.0 * _WaveSpeedMultiplier) * 6.2831853 + wavePhase);
                wave += hitWave * _HitImpulse * 0.012;

                float surfaceHeight = saturate(_FillAmount) + wave;
                float liquidMask = 1.0 - smoothstep(surfaceHeight, surfaceHeight + 0.004, input.texcoord.y);
                liquidMask *= step(0.0001, _FillAmount);
                float surfaceMask = 1.0 - smoothstep(_SurfaceWidth, _SurfaceWidth * 2.0,
                    abs(input.texcoord.y - surfaceHeight));
                surfaceMask *= liquidMask;

                float verticalGradient = saturate(input.texcoord.y * 0.9 + 0.05);
                fixed3 liquidColor = lerp(_BottomColor.rgb, _TopColor.rgb, verticalGradient);

                float noiseA = sin((input.texcoord.x * 9.0 + input.texcoord.y * 7.0 + _Time.y * _NoiseSpeed) * 6.2831853);
                float noiseB = sin((input.texcoord.x * -5.0 + input.texcoord.y * 11.0 - _Time.y * _NoiseSpeed * 0.73) * 6.2831853);
                float flowingNoise = (noiseA + noiseB) * 0.25 + 0.5;
                liquidColor *= lerp(1.0 - _NoiseStrength, 1.0 + _NoiseStrength, flowingNoise);

                float edge = smoothstep(0.25, 1.0, radius);
                liquidColor *= 1.0 - edge * _EdgeDarkness;
                liquidColor += _TopColor.rgb * (1.0 - edge) * _InnerGlow * 0.18;
                liquidColor = lerp(liquidColor, _SurfaceColor.rgb, surfaceMask * _SurfaceColor.a);

                fixed4 spriteColor = tex2D(_MainTex, input.texcoord) + _TextureSampleAdd;
                fixed4 outputColor;
                outputColor.rgb = liquidColor * input.color.rgb * spriteColor.rgb;
                outputColor.a = spriteColor.a * input.color.a * sphereMask * liquidMask;

                #ifdef UNITY_UI_CLIP_RECT
                outputColor.a *= UnityGet2DClipping(input.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip(outputColor.a - 0.001);
                #endif

                return outputColor;
            }
            ENDCG
        }
    }
}
