using UnityEngine;

namespace Tuntenfisch.Commons.Editor
{
    public static class EditorGUIStyles
    {
        #region Public Fields
        public static readonly GUIStyle CenteredLabelStyle;
        public static readonly GUIStyle PaneOptionsSytle;
        public static readonly GUIStyle CenteredBoldLabelStyle;
        public static readonly GUIStyle CenteredBlackLabelStyle;
        public static readonly GUIStyle BoxStyle;
        #endregion

        static EditorGUIStyles()
        {
            CenteredLabelStyle = new GUIStyle(GUI.skin.label);
            CenteredLabelStyle.alignment = TextAnchor.MiddleCenter;
            CenteredLabelStyle.richText = true;

            PaneOptionsSytle = new GUIStyle("PaneOptions");
            PaneOptionsSytle.imagePosition = ImagePosition.ImageOnly;

            CenteredBoldLabelStyle = new GUIStyle(GUI.skin.label);
            CenteredBoldLabelStyle.alignment = TextAnchor.MiddleCenter;
            CenteredBoldLabelStyle.fontStyle = FontStyle.Bold;
            CenteredBoldLabelStyle.richText = true;

            CenteredBlackLabelStyle = new GUIStyle(GUI.skin.label);
            CenteredBlackLabelStyle.alignment = TextAnchor.MiddleCenter;
            CenteredBlackLabelStyle.normal.textColor = Color.black;
            CenteredBlackLabelStyle.richText = true;

            BoxStyle = new GUIStyle("HelpBox");
        }
    }
}