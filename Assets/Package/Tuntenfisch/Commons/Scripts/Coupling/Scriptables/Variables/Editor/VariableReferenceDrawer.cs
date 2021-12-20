using System;
using UnityEditor;
using UnityEngine;

namespace Tuntenfisch.Commons.Coupling.Scriptables.Variables.Editor
{
    [CustomPropertyDrawer(typeof(VariableReadReference<>), useForChildren: true)]
    [CustomPropertyDrawer(typeof(VariableWriteReference<>), useForChildren: true)]
    [CustomPropertyDrawer(typeof(VariableReadWriteReference<>), useForChildren: true)]
    public class VariableReferenceDrawer : PropertyDrawer
    {
        private readonly string[] m_popupOptions =
        {
            "Use Literal",
            "Use Variable"
        };

        private GUIStyle m_popupStyle;
        private SerializedProperty m_variableProperty;
        private SerializedProperty m_literalValueProperty;
        private SerializedProperty m_useLiteralProperty;
        private Type m_variableCurrentValueType;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (m_popupStyle == null)
            {
                m_popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
                m_popupStyle.imagePosition = ImagePosition.ImageOnly;
            }

            if (m_variableProperty == null)
            {
                Initialize(property);
            }

            label = EditorGUI.BeginProperty(position, label, property);
            Rect controlRect = EditorGUI.PrefixLabel(position, label);
            Rect buttonRect = new Rect(controlRect.x, controlRect.y, controlRect.width, EditorGUIUtility.singleLineHeight);
            buttonRect.xMin = buttonRect.xMax - m_popupStyle.fixedWidth;
            Rect propertyRect = new Rect(controlRect.x, controlRect.y, controlRect.width, EditorGUIUtility.singleLineHeight);
            propertyRect.xMax -= buttonRect.width;
            m_useLiteralProperty.boolValue = EditorGUI.Popup(buttonRect, m_useLiteralProperty.boolValue ? 0 : 1, m_popupOptions, m_popupStyle) == 0;

            if (!m_useLiteralProperty.boolValue)
            {
                Rect valueRect = new Rect(controlRect.x, controlRect.y + EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight, controlRect.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(propertyRect, m_variableProperty, GUIContent.none);

                if (m_variableProperty.objectReferenceValue != null)
                {
                    SerializedObject variableObject = new SerializedObject(m_variableProperty.objectReferenceValue);
                    SerializedProperty variableCurrentValueProperty = variableObject.FindProperty("m_currentValue");

                    if (variableCurrentValueProperty.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        if (m_variableCurrentValueType == null)
                        {
                            m_variableCurrentValueType = variableObject.targetObject.GetType().BaseType.GetGenericArguments()[0];
                        }
                        EditorGUI.ObjectField(valueRect, GUIContent.none, variableCurrentValueProperty.objectReferenceValue, m_variableCurrentValueType, false);
                    }
                    else
                    {
                        EditorGUI.BeginDisabledGroup(true);
                        EditorGUI.PropertyField(valueRect, variableCurrentValueProperty, GUIContent.none);
                        EditorGUI.EndDisabledGroup();
                    }
                }
            }
            else
            {
                EditorGUI.PropertyField(propertyRect, m_literalValueProperty, GUIContent.none);
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (m_variableProperty == null)
            {
                Initialize(property);
            }

            if (!m_useLiteralProperty.boolValue && m_variableProperty.objectReferenceValue != null)
            {
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight;
            }
            else
            {
                return EditorGUIUtility.singleLineHeight;
            }
        }

        private void Initialize(SerializedProperty property)
        {
            m_variableProperty = property.FindPropertyRelative("m_variable");
            m_literalValueProperty = property.FindPropertyRelative("m_literalValue");
            m_useLiteralProperty = property.FindPropertyRelative("m_useLiteral");
        }
    }
}