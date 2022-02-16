using System;
using Tuntenfisch.Commons.Coupling.Editor;
using UnityEditor;
using UnityEngine;

namespace Tuntenfisch.Commons.Coupling.Variables.Editor
{
    [CustomEditor(typeof(Variable<>), editorForChildClasses: true)]
    public class VariableEditor : BaseCouplingEditor
    {
        #region Private Fields
        private readonly GUIContent valueDisplayName = new GUIContent("Value");

        private SerializedProperty m_defaultValueProperty;
        private SerializedProperty m_currentValueProperty;
        private SerializedProperty m_isConstantProperty;
        private Type m_valueType;
        #endregion

        #region Unity Callbacks
        protected override void OnEnable()
        {
            base.OnEnable();

            m_defaultValueProperty = serializedObject.FindProperty("m_defaultValue");
            m_currentValueProperty = serializedObject.FindProperty("m_currentValue");
            m_isConstantProperty = serializedObject.FindProperty("m_isConstant");
            m_valueType = m_currentValueProperty.serializedObject.targetObject.GetType().BaseType.GetGenericArguments()[0];
        }
        #endregion

        #region Protected Methods
        protected override void DisplayInspectorVariables()
        {
            EditorGUILayout.LabelField("Default Value", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_defaultValueProperty, valueDisplayName);

            if (!m_isConstantProperty.boolValue)
            {
                EditorGUILayout.LabelField("Current Value", EditorStyles.boldLabel);
                EditorGUI.BeginDisabledGroup(true);

                if (m_currentValueProperty.propertyType == SerializedPropertyType.ObjectReference)
                {
                    EditorGUILayout.ObjectField(m_currentValueProperty.displayName, m_currentValueProperty.objectReferenceValue, m_valueType, false);
                }
                else
                {
                    EditorGUILayout.PropertyField(m_currentValueProperty, valueDisplayName);
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.PropertyField(m_isConstantProperty);
        }
        #endregion
    }
}