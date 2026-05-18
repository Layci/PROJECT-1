Shader "UI/URPBackgroundBlur"
{
    Properties
    {
        _BlurSize ("Blur Size", Range(0, 20)) = 1
        _Tint ("Tint", Color) = (1, 1, 1, 0.7)
        
        // UI 시스템에서 필요한 속성들
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
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
            "RenderPipeline"="UniversalRenderPipeline"
        }

        // UI 스텐실 지원 (다른 UI와 겹칠 때 필요)
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
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float4 screenPos : TEXCOORD0; // 화면 좌표
                float4 color : COLOR;
            };

            float4 _Tint;
            float _BlurSize;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                // 화면상의 위치를 0~1 사이 좌표로 계산
                o.screenPos = ComputeScreenPos(o.positionHCS);
                o.color = v.color * _Tint;
                return o;
            }

            float4 frag (Varyings i) : SV_Target
            {
                // 화면 좌표 추출
                float2 screenUV = i.screenPos.xy / i.screenPos.w;
                
                // 화면 해상도에 따른 텍셀 크기 계산
                float2 texel = _ScreenSize.zw * _BlurSize;

                float3 col = 0;

                // 강력하고 부드러운 가우시안 느낌을 위한 멀티 샘플링 (17-Tap)
                float weights[5] = {0.2270270270, 0.1945945946, 0.1216216216, 0.0540540541, 0.0162162162};

                col += SampleSceneColor(screenUV) * weights[0];
                for (int t = 1; t < 5; t++)
                {
                    col += SampleSceneColor(screenUV + texel * float2(t, t)) * weights[t];
                    col += SampleSceneColor(screenUV - texel * float2(t, t)) * weights[t];
                    col += SampleSceneColor(screenUV + texel * float2(-t, t)) * weights[t];
                    col += SampleSceneColor(screenUV - texel * float2(-t, t)) * weights[t];
                }

                return float4(col * i.color.rgb, i.color.a);
            }
            ENDHLSL
        }
    }
}