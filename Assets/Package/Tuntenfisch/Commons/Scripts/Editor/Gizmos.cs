using System.Linq;
using Unity.Mathematics;

namespace Tuntenfisch.Commons.Editor
{
    public static class Gizmos
    {
        #region Private Fields
        private static readonly float3[] s_xzArrow = new float3[]
        {
            new float3(-0.5f, 0.0f, -1.0f),
            new float3(0.5f, 0.0f, -1.0f),
            new float3(0.5f, 0.0f, 0.5f),
            new float3(1.0f, 0.0f, 0.5f),
            new float3(0.0f, 0.0f, 1.5f),
            new float3(-1.0f, 0.0f, 0.5f),
            new float3(-0.5f, 0.0f, 0.5f),
            new float3(-0.5f, 0.0f, 0.5f)
        };

        private static readonly float3[] s_xyArrow;
        #endregion

        static Gizmos()
        {
            s_xyArrow = (from point in s_xzArrow select point.yxz).ToArray();
        }

        #region Public Methods
        public static void DrawPolygon(params float3[] points)
        {
            for (int index = 0; index < points.Length; index++)
            {
                UnityEngine.Gizmos.DrawLine(points[index], points[(index + 1) % points.Length]);
            }
        }

        public static void DrawForwardArrow()
        {
            DrawPolygon(s_xzArrow);
            DrawPolygon(s_xyArrow);
        }
        #endregion
    }
}