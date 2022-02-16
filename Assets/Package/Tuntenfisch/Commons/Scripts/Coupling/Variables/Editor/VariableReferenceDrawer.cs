using System;
using UnityEditor;
using UnityEngine;

namespace Tuntenfisch.Commons.Coupling.Variables.Editor
{
    [CustomPropertyDrawer(typeof(VariableReadReference<>))]
    [CustomPropertyDrawer(typeof(VariableWriteReference<>))]
    [CustomPropertyDrawer(typeof(VariableReadWriteReference<>))]
    public class VariableReferenceDrawer : PropertyDrawer
    {
        #region Private Fields
        private static readonly string[] s_popupOptions = { "Use Literal", "Use Variable" };

        private SerializedProperty m_variableProperty;
        private SerializedProperty m_literalValueProperty;
        private SerializedProperty m_useLiteralProperty;
        private Type m_variableCurrentValueType;
        #endregion

        #region Unity Callbacks
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (m_variableProperty == null)
            {
                Initialize(property);
            }
            Rect propertyRect = GetPropertyRect(position);
            DisplayVariableProperty(property, position, label);
            EditorGUIUtility.labelWidth -= position.width - propertyRect.width;
            DisplayValueProperty(propertyRect);
            EditorGUIUtility.labelWidth = 0.0f;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (m_variableProperty == null)
            {
                Initialize(property);
            }

            if (m_useLiteralProperty.boolValue)
            {
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + EditorGUI.GetPropertyHeight(m_literalValueProperty);
            }
            else if (m_variableProperty.objectReferenceValue != null)
            {
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + EditorGUI.GetPropertyHeight(new SerializedObject(m_variableProperty.objectReferenceValue).FindProperty("m_currentValue"));
            }
            return base.GetPropertyHeight(property, label);
        }
        #endregion

        #region Private Methods
        private void Initialize(SerializedProperty property)
        {
            m_variableProperty = property.FindPropertyRelative("m_variable");
            m_literalValueProperty = property.FindPropertyRelative("m_literalValue");
            m_useLiteralProperty = property.FindPropertyRelative("m_useLiteral");
        }

        private Rect GetPropertyRect(Rect position)
        {
            float offset = EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight;
            return Commons.Editor.EditorGUI.GetIndentedRect(EditorGUI.indentLevel + 1, new Rect(position.x, position.y + offset, position.width, position.height - offset));
        }

        private void DisplayVariableProperty(SerializedProperty property, Rect position, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            int selectedPopupOption = m_useLiteralProperty.boolValue ? 0 : 1;
            selectedPopupOption = Commons.Editor.EditorGUI.PropertyFieldWithPopupOptions(position, m_useLiteralProperty.boolValue ? null : m_variableProperty, label, s_popupOptions, selectedPopupOption);

            if (EditorGUI.EndChangeCheck())
            {
                m_useLiteralProperty.boolValue = selectedPopupOption == 0;
                Initialize(property);
            }
        }

        private void DisplayValueProperty(Rect propertyRect)
        {
            if (m_useLiteralProperty.boolValue)
            {
                EditorGUI.PropertyField(propertyRect, m_literalValueProperty);
            }
            else if (m_variableProperty.objectReferenceValue != null)
            {
                SerializedObject variableObject = new SerializedObject(m_variableProperty.objectReferenceValue);
                SerializedProperty variableCurrentValueProperty = variableObject.FindProperty("m_currentValue");
                EditorGUI.BeginDisabledGroup(true);

                if (variableCurrentValueProperty.propertyType == SerializedPropertyType.ObjectReference)
                {
                    if (m_variableCurrentValueType == null)
                    {
                        m_variableCurrentValueType = variableObject.targetObject.GetType().BaseType.GetGenericArguments()[0];
                    }
                    EditorGUI.ObjectField(propertyRect, variableCurrentValueProperty.displayName, variableCurrentValueProperty.objectReferenceValue, m_variableCurrentValueType, false);
                }
                else
                {
                    EditorGUI.PropertyField(propertyRect, variableObject.FindProperty("m_currentValue"));
                }
                EditorGUI.EndDisabledGroup();
            }
        }
        #endregion
    }
}