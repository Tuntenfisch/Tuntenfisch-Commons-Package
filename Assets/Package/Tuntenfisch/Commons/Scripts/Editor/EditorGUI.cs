using System;
using System.Reflection;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Tuntenfisch.Commons.Editor
{
    public static class EditorGUI
    {
        #region Public Properties
        public static readonly float HorizontalPadding;
        public static readonly GUIStyle CenteredLabelStyle;
        #endregion

        #region Private Fields
        private static GUIStyle s_popupStyle;
        #endregion

        static EditorGUI()
        {
            HorizontalPadding = 4;
            CenteredLabelStyle = new GUIStyle(GUI.skin.label);
            CenteredLabelStyle.alignment = TextAnchor.MiddleCenter;
            s_popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
            s_popupStyle.imagePosition = ImagePosition.ImageOnly;
        }

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

        public static void MinMaxRange(Rect position, ref float minRange, ref float maxRange, float minLimit, float maxLimit, GUIContent label)
        {
            Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
            float remainingWidth = position.width - labelRect.width - 2.0f * (EditorGUIUtility.fieldWidth + HorizontalPadding);
            Rect minValueRect = new Rect(labelRect.xMax, position.y, EditorGUIUtility.fieldWidth, position.height);
            Rect minMaxSliderRect = new Rect(minValueRect.xMax + HorizontalPadding, position.y, remainingWidth, position.height);
            Rect maxValueRect = new Rect(minMaxSliderRect.xMax + HorizontalPadding, position.y, EditorGUIUtility.fieldWidth, position.height);

            UnityEditor.EditorGUI.LabelField(labelRect, label);
            UnityEditor.EditorGUI.MinMaxSlider(minMaxSliderRect, GUIContent.none, ref minRange, ref maxRange, minLimit, maxLimit);
            minRange = UnityEditor.EditorGUI.FloatField(minValueRect, GUIContent.none, minRange);
            maxRange = UnityEditor.EditorGUI.FloatField(maxValueRect, GUIContent.none, maxRange);
            minRange = math.clamp(minRange, minLimit, maxRange);
            maxRange = math.clamp(maxRange, minRange, maxLimit);
        }

        public static (T, int) ObjectFieldWithPopupOptions<T>(T obj, GUIContent label, string[] popupOptions, int selectedPopupOption) where T : UnityEngine.Object
        {
            Rect position = EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true), GUILayout.Height(EditorGUIUtility.singleLineHeight));
            return ObjectFieldWithPopupOptions(position, obj, label, popupOptions, selectedPopupOption);
        }

        public static (T, int) ObjectFieldWithPopupOptions<T>(Rect position, T obj, GUIContent label, string[] popupOptions, int selectedPopupOption) where T : UnityEngine.Object
        {
            (Rect variableRect, Rect buttonRect) = FieldWithPopupOptionsPrefixLabel(position, label);

            obj = UnityEditor.EditorGUI.ObjectField(variableRect, GUIContent.none, obj, typeof(T), false) as T;
            selectedPopupOption = UnityEditor.EditorGUI.Popup(buttonRect, selectedPopupOption, popupOptions, s_popupStyle);
            return (obj, selectedPopupOption);
        }

        public static int PropertyFieldWithPopupOptions(SerializedProperty property, GUIContent label, string[] popupOptions, int selectedPopupOption)
        {
            Rect position = EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true), GUILayout.Height(EditorGUIUtility.singleLineHeight));
            return PropertyFieldWithPopupOptions(position, property, label, popupOptions, selectedPopupOption);
        }

        public static int PropertyFieldWithPopupOptions(Rect position, SerializedProperty property, GUIContent label, string[] popupOptions, int selectedPopupOption)
        {
            (Rect variableRect, Rect buttonRect) = FieldWithPopupOptionsPrefixLabel(position, label);

            if (property != null)
            {
                UnityEditor.EditorGUI.PropertyField(variableRect, property, GUIContent.none);
            }
            selectedPopupOption = UnityEditor.EditorGUI.Popup(buttonRect, selectedPopupOption, popupOptions, s_popupStyle);
            return selectedPopupOption;
        }

        public static Rect GetIndentedRect(int indentLevel, Rect rect)
        {
            int oldIndentLevel = UnityEditor.EditorGUI.indentLevel;
            UnityEditor.EditorGUI.indentLevel = indentLevel;
            rect = UnityEditor.EditorGUI.IndentedRect(rect);
            UnityEditor.EditorGUI.indentLevel = oldIndentLevel;
            return rect;
        }
        #endregion

        #region Private Methods
        private static (Rect, Rect) FieldWithPopupOptionsPrefixLabel(Rect position, GUIContent label)
        {
            Rect controlRect = UnityEditor.EditorGUI.PrefixLabel(position, label);
            Rect variableRect = new Rect(controlRect.x, controlRect.y, controlRect.width - s_popupStyle.fixedWidth, EditorGUIUtility.singleLineHeight);
            Rect buttonRect = new Rect(variableRect.xMax, controlRect.y, s_popupStyle.fixedWidth, variableRect.height);

            return (variableRect, buttonRect);
        }
        #endregion
    }
}