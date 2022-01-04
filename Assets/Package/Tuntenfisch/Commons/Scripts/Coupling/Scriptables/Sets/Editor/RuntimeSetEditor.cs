using System;
using Tuntenfisch.Commons.Coupling.Scriptables.Editor;
using UnityEditor;

namespace Tuntenfisch.Commons.Coupling.Scriptables.Sets.Editor
{
    [CustomEditor(typeof(RuntimeSet<>), editorForChildClasses: true)]
    public class RuntimeSetEditor : ScriptableEditor
    {
        #region Private Variables
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
        protected override void DisplayProperties()
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