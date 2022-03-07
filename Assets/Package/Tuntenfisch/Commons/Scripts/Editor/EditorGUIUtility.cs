using UnityEngine;
using Unity.Mathematics;

namespace Tuntenfisch.Commons.Editor
{
    public static class EditorGUIUtility
    {
        #region Public Properties
        public static readonly float2 Spacing;

        // Workaround to get width. Taken from https://forum.unity.com/threads/editorguilayout-get-width-of-inspector-window-area.82068/#post-7155460.
        public static float AvailableHorizontalWidth
        {
            get
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                if (Event.current.type == EventType.Repaint)
                {
                    s_width = GUILayoutUtility.GetLastRect().width;
                }
                return s_width;
            }
        }
        #endregion

        #region Private Fields
        private static float s_width;
        #endregion

        static EditorGUIUtility()
        {
            Spacing = new float2(4.0f, UnityEditor.EditorGUIUtility.standardVerticalSpacing);
        }

        #region Public Methods
        public static Rect GetIndentedRect(Rect rect, int indentLevel)
        {
            int oldIndentLevel = UnityEditor.EditorGUI.indentLevel;
            UnityEditor.EditorGUI.indentLevel = indentLevel;
            rect = UnityEditor.EditorGUI.IndentedRect(rect);
            UnityEditor.EditorGUI.indentLevel = oldIndentLevel;

            return rect;
        }

        public static bool HasContent(GUIContent content)
        {
            if (content == null)
            {
                return false;
            }

            return !string.IsNullOrEmpty(content.text) || content.image != null;
        }
        #endregion
    }
}