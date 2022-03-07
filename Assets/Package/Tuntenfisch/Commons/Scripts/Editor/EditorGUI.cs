﻿using System;
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

        public static int ObjectFieldWithPopupOptions<T>(Rect position, GUIContent label, ref T obj, int selectedPopupOption, string[] popupOptions) where T : UnityEngine.Object
        {
            (Rect variableRect, Rect buttonRect) = FieldWithPopupOptionsPrefixLabel(position, label);

            obj = UnityEditor.EditorGUI.ObjectField(variableRect, GUIContent.none, obj, typeof(T), false) as T;
            return UnityEditor.EditorGUI.Popup(buttonRect, selectedPopupOption, popupOptions, EditorGUIStyles.PaneOptionsSytle);
        }

        public static int PropertyFieldWithPopupOptions(Rect position, GUIContent label, SerializedProperty property, int selectedPopupOption, string[] popupOptions)
        {
            (Rect variableRect, Rect buttonRect) = FieldWithPopupOptionsPrefixLabel(position, label);

            if (property != null)
            {
                UnityEditor.EditorGUI.PropertyField(variableRect, property, GUIContent.none);
            }
            return UnityEditor.EditorGUI.Popup(buttonRect, selectedPopupOption, popupOptions, EditorGUIStyles.PaneOptionsSytle);
        }

        public static int PaneOptions(Rect position, int selectedPaneOptionIndex, string[] popupOptions)
        {
            Rect rect = new Rect(position.x, position.y + 0.5f * (position.height - EditorGUIStyles.PaneOptionsSytle.fixedHeight), EditorGUIStyles.PaneOptionsSytle.fixedWidth, position.height);
            return UnityEditor.EditorGUI.Popup(rect, selectedPaneOptionIndex, popupOptions, EditorGUIStyles.PaneOptionsSytle);
        }
        #endregion

        #region Private Methods
        private static (Rect, Rect) FieldWithPopupOptionsPrefixLabel(Rect position, GUIContent label)
        {
            Rect controlRect = UnityEditor.EditorGUI.PrefixLabel(position, label);
            Rect variableRect = new Rect(controlRect.x, controlRect.y, controlRect.width - EditorGUIStyles.PaneOptionsSytle.fixedWidth, controlRect.height);
            Rect buttonRect = new Rect(variableRect.xMax, controlRect.y + 0.5f * (variableRect.height - EditorGUIStyles.PaneOptionsSytle.fixedHeight), EditorGUIStyles.PaneOptionsSytle.fixedWidth, variableRect.height);

            return (variableRect, buttonRect);
        }
        #endregion
    }
}