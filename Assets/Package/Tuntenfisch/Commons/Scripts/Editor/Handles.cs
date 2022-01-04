using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Tuntenfisch.Commons.Editor
{
    public static class Handles
    {
        #region Public Variables
        public static GUIStyle s_centeredTextStyle;
        #endregion

        #region Public Methods
        public static void ColoredLabel(float3 position, string label, Color color)
        {
            if (s_centeredTextStyle == null)
            {
                s_centeredTextStyle = new GUIStyle(GUI.skin.label);
                s_centeredTextStyle.alignment = TextAnchor.MiddleCenter;
                s_centeredTextStyle.normal.textColor = Color.black;
                s_centeredTextStyle.richText = true;
            }

            float3 guiPoint = HandleUtility.WorldToGUIPointWithDepth(position);

            // If the point lies behind the camera, don't draw a label.
            if (guiPoint.z < 0.0f)
            {
                return;
            }

            GUIContent content = new GUIContent(label);
            float2 contentSize = s_centeredTextStyle.CalcSize(content);
            float2 backgroundSize = new float2(1.1f, 1.25f) * contentSize;
            UnityEditor.Handles.BeginGUI();
            GUI.DrawTexture(new Rect(guiPoint.xy - 0.5f * backgroundSize, backgroundSize), Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0, color, 0.0f, 10.0f);
            GUI.Label(new Rect(guiPoint.xy - 0.5f * contentSize, contentSize), content, s_centeredTextStyle);
            UnityEditor.Handles.EndGUI();
        }
        #endregion
    }
}