using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Tuntenfisch.Commons.Editor
{
    public static class EditorGUILayout
    {
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
            UnityEditor.EditorGUILayout.ObjectField("Script", script, serializedObject.targetObject.GetType(), false);
            UnityEditor.EditorGUI.EndDisabledGroup();
        }

        public static KeyCode TextFieldWithPlaceholder(GUIContent label, ref string text, string placeholder)
        {
            Rect position = UnityEditor.EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true), GUILayout.Height(UnityEditor.EditorGUIUtility.singleLineHeight));
            return EditorGUI.TextFieldWithPlaceholder(position, label, ref text, placeholder);
        }

        public static bool LabeledArrayField(bool foldout, SerializedProperty array, string[] labels)
        {
            if (array.arraySize != labels.Length)
            {
                array.arraySize = labels.Length;
            }

            if (foldout = UnityEditor.EditorGUILayout.BeginFoldoutHeaderGroup(foldout, array.displayName))
            {
                UnityEditor.EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                for (int index = 0; index < labels.Length; index++)
                {
                    SerializedProperty property = array.GetArrayElementAtIndex(index);
                    UnityEditor.EditorGUILayout.PropertyField(property, new GUIContent(labels[index]));
                }
                UnityEditor.EditorGUILayout.EndVertical();
            }
            UnityEditor.EditorGUILayout.EndFoldoutHeaderGroup();

            return foldout;
        }

        public static void MinMaxRange(GUIContent label, ref float minRange, ref float maxRange, float minLimit, float maxLimit)
        {
            Rect position = UnityEditor.EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true), GUILayout.Height(UnityEditor.EditorGUIUtility.singleLineHeight));
            EditorGUI.MinMaxRange(position, label, ref minRange, ref maxRange, minLimit, maxLimit);
        }

        public static int ObjectFieldWithPopupOptions<T>(GUIContent label, ref T obj, int selectedPopupOption, string[] popupOptions) where T : Object
        {
            Rect position = UnityEditor.EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true), GUILayout.Height(UnityEditor.EditorGUIUtility.singleLineHeight));
            return EditorGUI.ObjectFieldWithPopupOptions(position, label, ref obj, selectedPopupOption, popupOptions);
        }

        public static int PropertyFieldWithPopupOptions(GUIContent label, SerializedProperty property, int selectedPopupOption, string[] popupOptions)
        {
            Rect position = UnityEditor.EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true), GUILayout.Height(UnityEditor.EditorGUIUtility.singleLineHeight));
            return EditorGUI.PropertyFieldWithPopupOptions(position, label, property, selectedPopupOption, popupOptions);
        }

        public static int PaneOptions(int selectedPaneOptionIndex, string[] popupOptions, params GUILayoutOption[] options)
        {
            Rect position = UnityEditor.EditorGUILayout.GetControlRect(options);
            return EditorGUI.PaneOptions(position, selectedPaneOptionIndex, popupOptions);
        }

        #endregion
    }
}