using UnityEditor;
using UnityEngine;

namespace Tuntenfisch.Commons.Animations.Editor
{
    [CustomPropertyDrawer(typeof(Clip))]
    public class ClipDrawer : PropertyDrawer
    {
        #region Private Fields
        SerializedProperty m_animationProperty;
        SerializedProperty m_targetDurationProperty;
        #endregion

        #region Unity Callbacks
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (m_animationProperty == null)
            {
                Initialize(property);
            }

            label = EditorGUI.BeginProperty(position, label, property);
            Rect controlRect = EditorGUI.PrefixLabel(position, label);

            GUIContent animationLabel = new GUIContent(m_animationProperty.displayName);
            GUIContent targetDurationLabel = new GUIContent(m_targetDurationProperty.displayName);

            float animationLabelWidth = EditorStyles.label.CalcSize(animationLabel).x;
            float targetDurationLabelWidth = EditorStyles.label.CalcSize(targetDurationLabel).x;
            float halfControlWidth = 0.5f * controlRect.width - 1.5f * Commons.Editor.EditorGUIUtility.Spacing.x;

            Rect animationLabelRect = new Rect(controlRect.x, controlRect.y, animationLabelWidth, controlRect.height);
            Rect animationRect = new Rect(animationLabelRect.xMax + Commons.Editor.EditorGUIUtility.Spacing.x, controlRect.y, halfControlWidth - animationLabelWidth, controlRect.height);
            Rect targetDurationLabelRect = new Rect(animationRect.xMax + Commons.Editor.EditorGUIUtility.Spacing.x, controlRect.y, targetDurationLabelWidth, controlRect.height);
            Rect targetDurationRect = new Rect(targetDurationLabelRect.xMax + Commons.Editor.EditorGUIUtility.Spacing.x, controlRect.y, halfControlWidth - targetDurationLabelWidth, controlRect.height);

            EditorGUI.LabelField(animationLabelRect, animationLabel);
            EditorGUI.PropertyField(animationRect, m_animationProperty, GUIContent.none);
            EditorGUI.LabelField(targetDurationLabelRect, targetDurationLabel);
            EditorGUI.PropertyField(targetDurationRect, m_targetDurationProperty, GUIContent.none);
            EditorGUI.EndProperty();
        }
        #endregion

        #region Private Methods
        private void Initialize(SerializedProperty property)
        {
            m_animationProperty = property.FindPropertyRelative("m_animation");
            m_targetDurationProperty = property.FindPropertyRelative("m_targetDuration");
        }
        #endregion
    }
}