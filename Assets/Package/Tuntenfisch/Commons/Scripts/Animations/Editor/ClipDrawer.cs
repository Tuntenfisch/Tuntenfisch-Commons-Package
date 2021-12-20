using UnityEditor;
using UnityEngine;

namespace Tuntenfisch.Commons.Animations.Editor
{
    [CustomPropertyDrawer(typeof(Clip))]
    public class ClipDrawer : PropertyDrawer
    {
        SerializedProperty m_animationProperty;
        SerializedProperty m_targetDurationProperty;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (m_animationProperty == null)
            {
                Initialize(property);
            }

            label = UnityEditor.EditorGUI.BeginProperty(position, label, property);
            Rect controlRect = UnityEditor.EditorGUI.PrefixLabel(position, label);
        
            GUIContent animationLabel = new GUIContent(m_animationProperty.displayName);
            GUIContent targetDurationLabel = new GUIContent(m_targetDurationProperty.displayName);
        
            float animationLabelWidth = EditorStyles.label.CalcSize(animationLabel).x + Commons.Editor.EditorGUI.HorizontalPadding;
            float targetDurationLabelWidth = EditorStyles.label.CalcSize(targetDurationLabel).x + Commons.Editor.EditorGUI.HorizontalPadding;
        
            Rect animationLabelRect = new Rect(controlRect.x, controlRect.y, animationLabelWidth, controlRect.height);
            Rect animationRect = new Rect(animationLabelRect.xMax, controlRect.y, 0.5f * controlRect.width - animationLabelWidth, controlRect.height);
            Rect targetDurationLabelRect = new Rect(animationRect.xMax + Commons.Editor.EditorGUI.HorizontalPadding, controlRect.y, targetDurationLabelWidth, controlRect.height);
            Rect targetDurationRect = new Rect(targetDurationLabelRect.xMax, controlRect.y, 0.5f * controlRect.width - targetDurationLabelWidth, controlRect.height);

            UnityEditor.EditorGUI.LabelField(animationLabelRect, animationLabel);
            UnityEditor.EditorGUI.PropertyField(animationRect, m_animationProperty, GUIContent.none);
            UnityEditor.EditorGUI.LabelField(targetDurationLabelRect, targetDurationLabel);
            UnityEditor.EditorGUI.PropertyField(targetDurationRect, m_targetDurationProperty, GUIContent.none);
            UnityEditor.EditorGUI.EndProperty();
        }

        private void Initialize(SerializedProperty property)
        {
            m_animationProperty = property.FindPropertyRelative("m_animation");
            m_targetDurationProperty = property.FindPropertyRelative("m_targetDuration");
        }
    }
}