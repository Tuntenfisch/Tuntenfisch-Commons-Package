using Unity.Mathematics;
using UnityEngine;

namespace Tuntenfisch.Commons.Editor
{
    public static class EditorGUIIcons
    {
        #region Public Properties
        public static float2 IconSize => GUI.skin.button.CalcSize(LeftArrow);
        #endregion

        #region Public Fields
        public static readonly GUIContent LeftArrow;
        public static readonly GUIContent RightArrow;
        public static readonly GUIContent Plus;
        public static readonly GUIContent Minus;
        public static readonly GUIContent Save;
        public static readonly GUIContent CheckMark;
        #endregion

        static EditorGUIIcons()
        {
            LeftArrow = UnityEditor.EditorGUIUtility.IconContent("tab_prev");
            RightArrow = UnityEditor.EditorGUIUtility.IconContent("tab_next");
            Plus = UnityEditor.EditorGUIUtility.IconContent("Toolbar Plus");
            Minus = UnityEditor.EditorGUIUtility.IconContent("Toolbar Minus");
            Save = UnityEditor.EditorGUIUtility.IconContent("Toolbar Minus");
            CheckMark = UnityEditor.EditorGUIUtility.IconContent("Valid@2x");
        }
    }
}