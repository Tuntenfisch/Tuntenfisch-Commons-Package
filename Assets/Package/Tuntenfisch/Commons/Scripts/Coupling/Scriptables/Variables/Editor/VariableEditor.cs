using System;
using Tuntenfisch.Commons.Coupling.Scriptables.Editor;
using UnityEditor;

namespace Tuntenfisch.Commons.Coupling.Scriptables.Variables.Editor
{
    [CustomEditor(typeof(Variable<>), editorForChildClasses: true)]
    public class VariableEditor : ScriptableEditor
    {
        private SerializedProperty m_defaultValueProperty;
        private SerializedProperty m_isConstantProperty;
        private SerializedProperty m_valueProperty;
        private Type m_valueType;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_defaultValueProperty = serializedObject.FindProperty("m_defaultValue");
            m_isConstantProperty = serializedObject.FindProperty("m_isConstant");
            m_valueProperty = serializedObject.FindProperty("m_currentValue");
            m_valueType = m_valueProperty.serializedObject.targetObject.GetType().BaseType.GetGenericArguments()[0];
        }

        protected override void DisplayProperties()
        {
            EditorGUILayout.PropertyField(m_defaultValueProperty);

            if (!m_isConstantProperty.boolValue)
            {
                if (m_valueProperty.propertyType == SerializedPropertyType.ObjectReference)
                {
                    EditorGUILayout.ObjectField(m_valueProperty.displayName, m_valueProperty.objectReferenceValue, m_valueType, false);
                }
                else
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.PropertyField(m_valueProperty);
                    EditorGUI.EndDisabledGroup();
                }
            }
            EditorGUILayout.PropertyField(m_isConstantProperty);
        }
    }
}