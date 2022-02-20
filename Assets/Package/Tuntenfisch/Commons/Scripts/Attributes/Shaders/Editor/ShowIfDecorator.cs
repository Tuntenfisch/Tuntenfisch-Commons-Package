using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using UnityEngine.Rendering;

namespace Tuntenfisch.Commons.Attributes.Shaders.Editor
{
    public class ShowIfDecorator : MaterialPropertyDrawer
    {
        #region Private Fields
        private string m_keyword;
        private bool m_enabled;
        private FieldInfo m_propertyFlags;
        #endregion

        #region Unity Callbacks
        public override void OnGUI(Rect position, MaterialProperty property, string label, MaterialEditor editor)
        {
            if (m_propertyFlags == null)
            {
                m_propertyFlags = property.GetType().GetField("m_Flags", BindingFlags.NonPublic | BindingFlags.Instance);
            }
            ShaderPropertyFlags propertyFlags = (ShaderPropertyFlags)m_propertyFlags.GetValue(property);

            if (ShouldShow(editor))
            {
                propertyFlags &= ~ShaderPropertyFlags.HideInInspector;
            }
            else
            {
                propertyFlags |= ShaderPropertyFlags.HideInInspector;
            }
            m_propertyFlags.SetValue(property, propertyFlags);
        }

        public override float GetPropertyHeight(MaterialProperty property, string label, MaterialEditor editor)
        {
            if (!ShouldShow(editor))
            {
                return -base.GetPropertyHeight(property, label, editor) - EditorGUIUtility.standardVerticalSpacing;
            }
            return 0.0f;
        }
        #endregion

        #region Public Methods
        public ShowIfDecorator(string keyword, string enabled)
        {
            m_keyword = keyword;

            if (!bool.TryParse(enabled, out m_enabled))
            {
                string message = $"Invalid \"{nameof(enabled)}\" value supplied to ShowIf attribute. Value should be parsable as a bool.";
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