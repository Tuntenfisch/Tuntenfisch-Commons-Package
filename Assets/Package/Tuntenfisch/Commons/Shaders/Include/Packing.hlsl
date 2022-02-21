#ifndef TUNTENFISCH_COMMONS_INCLUDE_PACKING
#define TUNTENFISCH_COMMONS_INCLUDE_PACKING

uint PackR8G8B8(float3 rgb)
{
    uint r = clamp((uint)floor(rgb.r * 256.0f), 0, 255);
    uint g = clamp((uint)floor(rgb.g * 256.0f), 0, 255);
    uint b = clamp((uint)floor(rgb.b * 256.0f), 0, 255);

    return r << 0 | g << 8 | b << 16;
}

float3 UnpackR8G8B8(uint rgb)
{
    float r = (rgb >> 0) & 255;
    float g = (rgb >> 8) & 255;
    float b = (rgb >> 16) & 255;

    return float3(r, g, b) / 255.0f;
}

#endif