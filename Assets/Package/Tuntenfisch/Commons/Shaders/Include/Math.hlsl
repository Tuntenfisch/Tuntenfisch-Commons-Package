#ifndef TUNTENFISCH_COMMONS_INCLUDE_MATH
#define TUNTENFISCH_COMMONS_INCLUDE_MATH

float GetSmallestComponent(float2 value)
{
    return min(value.x, value.y);
}

float GetSmallestComponent(float3 value)
{
    return min(GetSmallestComponent(value.xy), value.z);
}

float GetSmallestComponent(float4 value)
{
    return min(GetSmallestComponent(value.xyz), value.w);
}

float4x4 Matrix3x3ToMatrix4x4(float3x3 matrix3x3)
{
    float4x4 matrix4x4 = 0.0f;

    [unroll]
    for (int iterator = 0; iterator < 3; iterator++)
    {
        matrix4x4[iterator].xyz = matrix3x3[iterator];
    }
    matrix4x4[3][3] = 1.0f;

    return matrix4x4;
}

float4x4 AxesToRotationTransform(float3 rightAxis, float3 upAxis, float3 forwardAxis)
{
    return Matrix3x3ToMatrix4x4(transpose(float3x3(rightAxis, upAxis, forwardAxis)));
}

// Function taken from https://gist.github.com/keijiro/ee439d5e7388f3aafc5296005c8c3f33.
float3x3 AngleAxis3x3(float angle, float3 axis)
{
    float c, s;
    sincos(angle, s, c);
    float t = 1.0f - c;
    float x = axis.x;
    float y = axis.y;
    float z = axis.z;

    return float3x3
    (
        t * x * x + c, t * x * y - s * z, t * x * z + s * y,
        t * x * y + s * z, t * y * y + c, t * y * z - s * x,
        t * x * z - s * y, t * y * z + s * x, t * z * z + c
    );
}

float4x4 AngleAxis4x4(float angle, float3 axis)
{
    return Matrix3x3ToMatrix4x4(AngleAxis3x3(angle, axis));
}

float3 GetTransformTranslation(float4x4 transform)
{
    return transform._m03_m13_m23;
}

float4x4 SetTransformTranslation(float4x4 transform, float3 translation)
{
    transform._m03_m13_m23 = translation;
    return transform;
}

float3 TransformPosition(float4x4 transform, float3 position)
{
    return mul(transform, float4(position, 1.0f)).xyz;
}

float3 TransformDirection(float4x4 transform, float3 direction)
{
    return mul(transform, float4(direction, 0.0f)).xyz;
}

float4x4 InvertTranslationTransform(float4x4 translationTransform)
{
    translationTransform = SetTransformTranslation(translationTransform, -GetTransformTranslation(translationTransform));
    return translationTransform;
}

float4x4 InvertRotationTransform(float4x4 rotationTransform)
{
    // Inverting a rotation matrix is fast. It's simply the transpose of the rotation matrix.
    // This only works for matrices with orthonormal column vectors, i.e. rotation matrices, tho.
    return transpose(rotationTransform);
}

// Based on https://stackoverflow.com/questions/2624422/efficient-4x4-matrix-inverse-affine-transform.
float4x4 InvertTranslationRotationTransform(float4x4 translationRotationTransform)
{
    float3 translation = GetTransformTranslation(translationRotationTransform);
    float3x3 inverseRotationTransform = transpose((float3x3)translationRotationTransform);
    float3 inverseTranslation = -mul(inverseRotationTransform, translation);

    return float4x4
    (
        float4(inverseRotationTransform[0], inverseTranslation.x),
        float4(inverseRotationTransform[1], inverseTranslation.y),
        float4(inverseRotationTransform[2], inverseTranslation.z),
        float4(0.0f, 0.0f, 0.0f, 1.0f)
    );
}

#endif