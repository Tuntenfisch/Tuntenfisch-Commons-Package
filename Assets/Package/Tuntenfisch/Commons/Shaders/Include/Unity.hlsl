#ifndef TUNTENFISCH_COMMONS_INCLUDE_UNITY
#define TUNTENFISCH_COMMONS_INCLUDE_UNITY

// Basically same as Unity's implementation found here https://github.com/Unity-Technologies/Graphics/blob/master/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl.
float4 TransformWorldToShadowHClip(float3 positionWS, float3 normalWS, float3 lightPositionWS, float3 lightDirectionWS)
{
    #if defined(_CASTING_PUNCTUAL_LIGHT_SHADOW)
        lightDirectionWS = normalize(lightPositionWS - positionWS);
    #endif

    float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));

    #if defined(UNITY_REVERSED_Z)
        positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
    #else
        positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
    #endif

    return positionCS;
}

// Based on https://www.cyanilux.com/tutorials/depth.
float SampleLinearSceneDepth(float2 uv)
{
    float rawSceneDepth = SampleSceneDepth(uv);
    float perspectiveLinearSceneDepth = LinearEyeDepth(rawSceneDepth, _ZBufferParams);
    float orthographicLinearSceneDepth = lerp(_ProjectionParams.y, _ProjectionParams.z, _ProjectionParams.x > 0.0f ? rawSceneDepth : 1.0f - rawSceneDepth);

    return unity_OrthoParams.w == 0.0f ? perspectiveLinearSceneDepth : orthographicLinearSceneDepth;
}

float FresnelEffect(float3 normalWS, float3 viewDirectionWS, float exponent)
{
    return pow((1.0f - saturate(dot(normalize(normalWS), normalize(viewDirectionWS)))), exponent);
}

#endif