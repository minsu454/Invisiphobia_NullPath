Shader "Custom/URP_OutlineExample"
{
    Properties
    {
        // 메인 텍스처
        _MainTex ("Main Texture", 2D) = "white" {}

        // 아웃라인 색상
        _OutlineColor ("Outline Color", Color) = (1, 1, 0, 1)

        // 아웃라인 두께
        _OutlineWidth ("Outline Width", Range(0, 0.1)) = 0.02
    }

    SubShader
    {
        // 일반적으로 Opaque(불투명) 큐에 그리지만,
        // 상황에 따라 Transparent로 변경할 수도 있습니다.
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100

        //----------------------------------------------------------------------
        // 1) Outline Pass
        //----------------------------------------------------------------------
        Pass
        {
            Name "OutlinePass"
            Tags { "LightMode" = "UniversalForward" }

            // 앞면을 버리고 뒷면만 그려서(Front Cull)
            // 바깥쪽으로 확장된 지오메트리 부분이 보이게 만든다.
            Cull Front

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // URP에서 공통으로 쓰이는 HLSL 라이브러리
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // 셰이더 프로퍼티
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            
            float4 _OutlineColor;
            float  _OutlineWidth;

            struct Attributes
            {
                float4 positionOS : POSITION; // 오브젝트 로컬 좌표
                float3 normalOS   : NORMAL;   // 오브젝트 로컬 법선
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION; // 클립 좌표 (동화상 좌표)
                float2 uv          : TEXCOORD0;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                // 월드 좌표로 변환
                float3 worldPos    = TransformObjectToWorld(IN.positionOS.xyz);
                float3 worldNormal = normalize(TransformObjectToWorldNormal(IN.normalOS));

                // 아웃라인 두께만큼 법선 방향으로 바깥 확장
                worldPos += worldNormal * _OutlineWidth;

                // 다시 클립 좌표로 변환
                OUT.positionHCS = TransformWorldToHClip(worldPos);
                OUT.uv = IN.uv;

                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                // 단색으로 Outline 색상을 출력
                return _OutlineColor;
            }
            ENDHLSL
        }

        //----------------------------------------------------------------------
        // 2) Base Pass (실제 오브젝트 본체)
        //----------------------------------------------------------------------
        Pass
        {
            Name "BasePass"
            Tags{ "LightMode" = "UniversalForward" }

            Cull Back // 일반적인 백 페이스 컬링

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv          = IN.uv;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                // 메인 텍스처 샘플링(간단 예시)
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                return col;
            }
            ENDHLSL
        }
    }

    FallBack "Universal Render Pipeline/Lit"
}

