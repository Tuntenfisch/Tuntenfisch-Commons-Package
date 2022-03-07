using Unity.Mathematics;
using UnityEngine;

namespace Tuntenfisch.Commons.Graphics.UI
{
    public static class RectExtensions
    {
        #region Public Methods
        public static Rect Pad(this Rect rect, float2 padding)
        {
            return new Rect((float2)rect.min - 0.5f * padding, (float2)rect.size + padding);
        }
        #endregion
    }
}