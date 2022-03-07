using System;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Tuntenfisch.Commons.Editor
{
    public static class EditorGUI
    {
        #region Public Methods
        public static KeyCode TextFieldWithPlaceholder(Rect position, GUIContent label, ref string text, string placeholder)
        {
            const string controlName = "EG6FL3Qjll7nyGOK";

            KeyCode keyCode = KeyCode.None;

            if (Event.current.type == EventType.KeyDown)
            {
                keyCode = Event.current.keyCode;
            }

            GUI.SetNextControlName(controlName);
            text = UnityEditor.EditorGUI.TextField(position, label, text);

            if (GUI.GetNameOfFocusedControl() != controlName)
            {
                keyCode = KeyCode.None;
            }

            float offset = EditorGUIUtility.HasContent(label) ? UnityEditor.EditorGUIUtility.labelWidth + EditorGUIUtility.Spacing.x : 0.5f * EditorGUIUtility.Spacing.x;

            if (string.IsNullOrEmpty(text))
            {
                Rect placeholderRect = new Rect(position.x + offset, position.y, position.width - offset, position.height);
                Color guiColor = GUI.color;
                GUI.color = Color.gray;
                UnityEditor.EditorGUI.LabelField(placeholderRect, placeholder);
                GUI.color = guiColor;
            }
            return keyCode;
        }

        public static void GenericObjectField(Rect position, GUIContent label, SerializedProperty property, Type baseType)
        {
            TypeCache.TypeCollection collection = TypeCache.GetTypesDerivedFrom(baseType);
            property.objectReferenceValue = UnityEditor.EditorGUI.ObjectField(position, label, property.objectReferenceValue, collection.Count > 0 ? collection[0] : null, false);
        }

        public static void MinMaxRange(Rect position, GUIContent label, ref float minRange, ref float maxRange, float minLimit, float maxLimit)
        {
            Rect controlRect = UnityEditor.EditorGUI.PrefixLabel(position, label);
            Rect minValueRect = new Rect(controlRect.x, controlRect.y, UnityEditor.EditorGUIUtility.fieldWidth, controlRect.height);
            Rect maxValueRect = new Rect(controlRect.xMax - UnityEditor.EditorGUIUtility.fieldWidth, controlRect.y, UnityEditor.EditorGUIUtility.fieldWidth, controlRect.height);
            Rect minMaxSliderRect = new Rect(minValueRect.xMax + EditorGUIUtility.Spacing.x, controlRect.y, maxValueRect.x - minValueRect.xMax - 2.0f * EditorGUIUtility.Spacing.x, controlRect.height);

            UnityEditor.EditorGUI.MinMaxSlider(minMaxSliderRect, GUIContent.none, ref minRange, ref maxRange, minLimit, maxLimit);
            minRange = UnityEditor.EditorGUI.FloatField(minValueRect, GUIContent.none, minRange);
            maxRange = UnityEditor.EditorGUI.FloatField(maxValueRect, GUIContent.none, maxRange);
            minRange = math.clamp(minRange, minLimit, maxRange);
            maxRange = math.clamp(maxRange, minRange, maxLimit);
        }

        public static int ObjectFieldWithPaneOptions<T>(Rect position, GUIContent label, ref T obj, int selectedPaneOption, string[] paneOptions) where T : UnityEngine.Object
        {
            (Rect fieldRect, Rect paneOptionsRect) = GetFieldAndPaneOptionsRect(position, label);

            obj = UnityEditor.EditorGUI.ObjectField(fieldRect, GUIContent.none, obj, typeof(T), false) as T;
            return PaneOptions(paneOptionsRect, selectedPaneOption, paneOptions);
        }

        public static int PropertyFieldWithPaneOptions(Rect position, GUIContent label, SerializedProperty property, int selectedPaneOption, string[] paneOptions)
        {
            (Rect fieldRect, Rect paneOptionsRect) = GetFieldAndPaneOptionsRect(position, label);

            if (property != null)
            {
                UnityEditor.EditorGUI.PropertyField(fieldRect, property, GUIContent.none);
            }
            return PaneOptions(paneOptionsRect, selectedPaneOption, paneOptions);
        }

        public static int PaneOptions(Rect position, int selectedPaneOptionIndex, string[] popupOptions)
        {
            Rect rect = new Rect(position.x, position.y + 0.5f * (position.height - EditorGUIStyles.PaneOptionsSytle.fixedHeight), EditorGUIStyles.PaneOptionsSytle.fixedWidth, position.height);
            return UnityEditor.EditorGUI.Popup(rect, selectedPaneOptionIndex, popupOptions, EditorGUIStyles.PaneOptionsSytle);
        }
        #endregion

        #region Private Methods
        private static (Rect, Rect) GetFieldAndPaneOptionsRect(Rect position, GUIContent label)
        {
            Rect controlRect = UnityEditor.EditorGUI.PrefixLabel(position, label);
            Rect fieldRect = new Rect(controlRect.x, controlRect.y, controlRect.width - EditorGUIStyles.PaneOptionsSytle.fixedWidth - EditorGUIUtility.Spacing.x, controlRect.height);
            Rect paneOptionsRect = new Rect(fieldRect.xMax + EditorGUIUtility.Spacing.x, controlRect.y, EditorGUIStyles.PaneOptionsSytle.fixedWidth, fieldRect.height);

            return (fieldRect, paneOptionsRect);
        }
        #endregion
    }
}