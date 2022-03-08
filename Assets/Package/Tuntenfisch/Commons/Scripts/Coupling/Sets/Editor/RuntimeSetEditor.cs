using System;
using Tuntenfisch.Commons.Coupling.Editor;
using UnityEditor;

namespace Tuntenfisch.Commons.Coupling.Sets.Editor
{
    [CustomEditor(typeof(RuntimeSet<>), editorForChildClasses: true)]
    public class RuntimeSetEditor : CouplingEditor
    {
        #region Private Fields
        private SerializedProperty m_serializedSetProperty;
        private Type m_elementType;
        #endregion

        #region Unity Callbacks
        protected override void OnEnable()
        {
            base.OnEnable();
            m_serializedSetProperty = serializedObject.FindProperty("m_serializedSet");
            m_elementType = target.GetType().BaseType.GetGenericArguments()[0];
        }
        #endregion

        #region Protected Methods
        protected override void DisplayInspectorVariables()
        {
            if (m_serializedSetProperty.arraySize != 0)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                for (int index = 0; index < m_serializedSetProperty.arraySize; index++)
                {
                    SerializedProperty arrayElement = m_serializedSetProperty.GetArrayElementAtIndex(index);
                    EditorGUILayout.ObjectField(arrayElement.displayName, arrayElement.objectReferenceValue, m_elementType, false);
                }
                EditorGUILayout.EndVertical();
            }
        }
        #endregion
    }
}