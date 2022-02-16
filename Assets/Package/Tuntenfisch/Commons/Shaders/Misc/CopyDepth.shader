Shader "Tuntenfisch/Commons/Misc/Copy Depth"
{
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "Queue" = "Geometry" }

        HLSLINCLUDE

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        ENDHLSL

        Pass
        {
            Name "Unlit"

            HLSLPROGRAM

            #pragma vertex VertexPass
            #pragma fragment FragmentPass


            struct VertexPassInput
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;

                static VertexPassInput Create(float4 positionOS, float2 uv)
                {
                    VertexPassInput input = (VertexPassInput)0;
                    input.positionOS = positionOS;
                    input.uv = uv;

                    return input;
                }
            };

            struct FragmentPassInput
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;

                static FragmentPassInput Create(float4 positionCS, float2 uv)
                {
                    FragmentPassInput input = (FragmentPassInput)0;
                    input.positionCS = positionCS;
                    input.uv = uv;

                    return input;
                }
            };

            TEXTURE2D(_CameraDepthTexture);
            SAMPLER(sampler_CameraDepthTexture);

            FragmentPassInput VertexPass(VertexPassInput input)
            {
                VertexPositionInputs positionInputs = GetVertexPositionInputs(input.positionOS.xyz);
                return FragmentPassInput::Create(positionInputs.positionCS, input.uv);
            }

            float4 FragmentPass(FragmentPassInput input) : SV_Target
            {
                return SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, input.uv).r;
            }

            ENDHLSL
        }
    }
}