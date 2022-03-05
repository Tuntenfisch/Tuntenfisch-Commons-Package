using Unity.Mathematics;
using UnityEngine;

namespace Tuntenfisch.Commons.Graphics
{
    public static class CameraExtensions 
    {
        #region Public Methods
        // Taken from https://forum.unity.com/threads/_zbufferparams-values.39332/.
        public static float4 GetZBufferParameters(this Camera camera)
        {
            float x = 1.0f - camera.farClipPlane / camera.nearClipPlane;
            float y = camera.farClipPlane / camera.nearClipPlane;
            return new float4(x, y, x / camera.farClipPlane, y / camera.farClipPlane);
        }
        #endregion
    }
}