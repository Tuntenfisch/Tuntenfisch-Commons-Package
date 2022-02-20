using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Tuntenfisch.Commons.Attributes.Shaders.Editor
{
    public class ShowIfDrawer : MaterialPropertyDrawer
    {
        #region Private Fields
        private string m_keyword;
        private bool m_enabled;
        #endregion

        #region Unity Callbacks
        public override void OnGUI(Rect position, MaterialProperty property, string label, MaterialEditor editor)
        {
            if (ShouldShow(editor))
            {
                editor.DefaultShaderProperty(property, label);
            }
        }

        public override float GetPropertyHeight(MaterialProperty property, string label, MaterialEditor editor)
        {
            return -EditorGUIUtility.standardVerticalSpacing;
        }
        #endregion

        #region Public Methods
        public ShowIfDrawer(string keyword, string enabled)
        {
            m_keyword = keyword;

            if (!bool.TryParse(enabled, out m_enabled))
            {
                string message = $"Invalid \"{nameof(enabled)}\" value supplied to ShowIf attribute. Value should be bool interpretable.";
                Debug.LogWarning(message);
                throw new ArgumentException(message, nameof(enabled));
            }
        }
        #endregion

        #region Private Methods
        private bool ShouldShow(MaterialEditor editor)
        {
            bool show = false;

            foreach (Material material in from target in editor.targets where target != null select target as Material)
            {
                show |= material.IsKeywordEnabled(m_keyword) == m_enabled;
            }
            return show;
        }
        #endregion
    }
}