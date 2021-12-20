using System;
using Unity.Mathematics;
using UnityEditor;

namespace Tuntenfisch.Commons.Coupling.Scriptables.Variables.Editor
{
    [CustomEditor(typeof(VariableSetter))]
    public class VariableSetterEditor : UnityEditor.Editor
    {
        private SerializedProperty m_variableTypeProperty;
        private SerializedProperty m_gameObjectVariableProperty;
        private SerializedProperty m_gameObjectValueProperty;
        private SerializedProperty m_transformVariableProperty;
        private SerializedProperty m_transformValueProperty;

        private void OnEnable()
        {
            m_variableTypeProperty = serializedObject.FindProperty("m_variableType");
            m_gameObjectVariableProperty = serializedObject.FindProperty("m_gameObjectVariable");
            m_gameObjectValueProperty = serializedObject.FindProperty("m_gameObjectValue");
            m_transformVariableProperty = serializedObject.FindProperty("m_transformVariable");
            m_transformValueProperty = serializedObject.FindProperty("m_transformValue");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            m_variableTypeProperty.enumValueIndex = math.max(m_variableTypeProperty.enumValueIndex, 0);
            EditorGUILayout.PropertyField(m_variableTypeProperty);
            VariableSetter.VariableType variableType = (VariableSetter.VariableType)Enum.GetValues(typeof(VariableSetter.VariableType)).GetValue(m_variableTypeProperty.enumValueIndex);
            
            switch (variableType)
            {
                case VariableSetter.VariableType.GameObject:
                    EditorGUILayout.PropertyField(m_gameObjectVariableProperty);
                    EditorGUILayout.PropertyField(m_gameObjectValueProperty);
                    break;
            
                case VariableSetter.VariableType.Transform:
                    EditorGUILayout.PropertyField(m_transformVariableProperty);
                    EditorGUILayout.PropertyField(m_transformValueProperty);
                    break;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}