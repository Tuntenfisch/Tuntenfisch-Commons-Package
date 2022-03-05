using Unity.Mathematics;
using UnityEngine;

namespace Tuntenfisch.Commons.Editor
{
    public static class EditorGUIIcons
    {
        #region Public Properties
        public static float2 IconSize => GUI.skin.button.CalcSize(LeftArrowIcon);
        #endregion

        #region Public Fields
        public static readonly GUIContent LeftArrowIcon;
        public static readonly GUIContent RightArrowIcon;
        public static readonly GUIContent PlusIcon;
        public static readonly GUIContent MinusIcon;
        public static readonly GUIContent SaveIcon;
        public static readonly GUIContent CheckMarkIcon;
        #endregion

        static EditorGUIIcons()
        {
            LeftArrowIcon = UnityEditor.EditorGUIUtility.IconContent("tab_prev");
            RightArrowIcon = UnityEditor.EditorGUIUtility.IconContent("tab_next");
            PlusIcon = UnityEditor.EditorGUIUtility.IconContent("Toolbar Plus");
            MinusIcon = UnityEditor.EditorGUIUtility.IconContent("Toolbar Minus");
            SaveIcon = UnityEditor.EditorGUIUtility.IconContent("SaveAs");
            CheckMarkIcon = UnityEditor.EditorGUIUtility.IconContent("Valid@2x");
        }
    }
}