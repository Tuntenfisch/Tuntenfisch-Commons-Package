using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Tuntenfisch.Commons.Editor
{
    public static class EditorGUI
    {
        #region Public Variables
        public static float HorizontalPadding => c_horizontalPadding;
        #endregion

        #region Private Variables
        private const float c_horizontalPadding = 4;
        #endregion

        #region Public Methods
        public static void ScriptHeaderField(SerializedObject serializedObject)
        {
            MonoScript script = null;

            if (serializedObject.targetObject is MonoBehaviour monoBehaviour)
            {
                script = MonoScript.FromMonoBehaviour(monoBehaviour);
            }
            else if (serializedObject.targetObject is ScriptableObject scriptableObject)
            {
                script = MonoScript.FromScriptableObject(scriptableObject);
            }

            UnityEditor.EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script", script, serializedObject.targetObject.GetType(), false);
            UnityEditor.EditorGUI.EndDisabledGroup();
        }

        public static bool LabeledArrayField(bool foldout, SerializedProperty array, string[] labels)
        {
            if (array.arraySize != labels.Length)
            {
                array.arraySize = labels.Length;
            }

            if (foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, array.displayName))
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                for (int index = 0; index < labels.Length; index++)
                {
                    SerializedProperty property = array.GetArrayElementAtIndex(index);
                    EditorGUILayout.PropertyField(property, new GUIContent(labels[index]));
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            return foldout;
        }

        public static void GenericObjectField(Rect position, SerializedProperty property, Type baseType, GUIContent label)
        {
            TypeCache.TypeCollection collection = TypeCache.GetTypesDerivedFrom(baseType);
            property.objectReferenceValue = UnityEditor.EditorGUI.ObjectField(position, label, property.objectReferenceValue, collection.Count > 0 ? collection[0] : null, false);
        }

        public static T GetAttribute<T>(SerializedProperty property) where T : Attribute
        {
            FieldInfo fieldInfo = GetFieldInfo(property);

            if (fieldInfo == null)
            {
                return default;
            }
            return fieldInfo.GetCustomAttribute<T>();
        }

        public static FieldInfo GetFieldInfo(SerializedProperty property)
        {
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            FieldInfo fieldInfo = null;
            Type type = property.serializedObject.targetObject.GetType();

            foreach (string fieldName in property.propertyPath.Split('.'))
            {
                while (type != null)
                {
                    fieldInfo = type.GetField(fieldName, bindingFlags);

                    if (fieldInfo != null)
                    {
                        type = fieldInfo.FieldType;

                        break;
                    }
                    type = type.BaseType;
                }
            }
            return fieldInfo;
        }
        #endregion
    }
}