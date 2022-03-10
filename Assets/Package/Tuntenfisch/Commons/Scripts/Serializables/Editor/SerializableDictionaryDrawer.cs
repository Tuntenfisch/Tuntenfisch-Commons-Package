using System.Collections;
using Tuntenfisch.Commons.Editor;
using Tuntenfisch.Commons.Graphics.UI;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Tuntenfisch.Commons.Serializables.Editor
{
    [CustomPropertyDrawer(typeof(SerializableDictionary<,>), useForChildren: true)]
    public class SerializableDictionaryDrawer : PropertyDrawer
    {
        #region Privat Fields
        private SerializedProperty m_serializableDictionaryProperty;
        private SerializedProperty m_keyValuePairsProperty;
        private SerializedProperty m_keyValuePairProperty;
        private UnityEditorInternal.ReorderableList m_list;
        #endregion

        #region Unity Callbacks
        // Appearance is in accordance with https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/Inspector/ReorderableListWrapper.cs.
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.serializedObject.Update();

            if (m_serializableDictionaryProperty == null)
            {
                InitializeProperties(property);
            }

            (Rect headerRect, Rect sizeRect, Rect listRect) = GetHeaderAndSizeAndListRect(position);
            EventType previousEventType = Event.current.type;

            if (Event.current.type == EventType.MouseUp && sizeRect.Contains(Event.current.mousePosition))
            {
                Event.current.type = EventType.Used;
            }
            bool oldEnabled = GUI.enabled;
            GUI.enabled = true;
            property.isExpanded = UnityEditor.EditorGUI.BeginFoldoutHeaderGroup(headerRect, property.isExpanded, label);
            UnityEditor.EditorGUI.EndFoldoutHeaderGroup();
            GUI.enabled = oldEnabled;

            if (Event.current.type == EventType.Used && sizeRect.Contains(Event.current.mousePosition))
            {
                Event.current.type = previousEventType;
            }
            UnityEditor.EditorGUI.BeginDisabledGroup(true);
            math.max(UnityEditor.EditorGUI.DelayedIntField(sizeRect, m_keyValuePairsProperty.arraySize), 0);
            UnityEditor.EditorGUI.EndDisabledGroup();

            if (property.isExpanded)
            {
                m_list.DoList(listRect);
            }
            property.serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (m_keyValuePairsProperty == null)
            {
                InitializeProperties(property);
            }

            float height = UnityEditor.EditorGUIUtility.singleLineHeight;

            if (property.isExpanded)
            {
                height += UnityEditor.EditorGUIUtility.standardVerticalSpacing + m_list.GetHeight();
            }
            return height;
        }
        #endregion

        #region Private Methods
        private void InitializeProperties(SerializedProperty property)
        {
            m_serializableDictionaryProperty = property;
            m_keyValuePairsProperty = property.FindPropertyRelative("m_keyValuePairs");
            m_keyValuePairProperty = property.FindPropertyRelative("m_keyValuePair");
            m_list = new UnityEditorInternal.ReorderableList(m_keyValuePairsProperty.serializedObject, m_keyValuePairsProperty, false, false, true, true);
            m_list.drawElementCallback = DrawElement;
            m_list.elementHeightCallback = GetElementHeight;
            m_list.drawNoneElementCallback = DrawNoneElement;
            m_list.onAddCallback = AddElement;
        }

        private (Rect, Rect, Rect) GetHeaderAndSizeAndListRect(Rect position)
        {
            Rect headerRect = new Rect(position.x, position.y, position.width, UnityEditor.EditorGUIUtility.singleLineHeight);
            float width = 48.0f - UnityEditor.EditorGUI.indentLevel * UnityEditor.EditorGUI.indentLevel * 15.0f;
            Rect sizeRect = new Rect(headerRect.xMax - width, headerRect.y, width, headerRect.height);
            Rect listRect = new Rect(headerRect.x, headerRect.yMax + UnityEditor.EditorGUIUtility.standardVerticalSpacing, position.width, m_list.GetHeight());

            return (headerRect, sizeRect, listRect);
        }

        private float GetElementHeight(int index)
        {
            SerializedProperty keyValuePairProperty = m_keyValuePairsProperty.GetArrayElementAtIndex(index);
            SerializedProperty keyProperty = keyValuePairProperty.FindPropertyRelative("Key");
            SerializedProperty valueProperty = keyValuePairProperty.FindPropertyRelative("Value");

            return math.max(UnityEditor.EditorGUI.GetPropertyHeight(keyProperty), UnityEditor.EditorGUI.GetPropertyHeight(valueProperty));
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            UnityEditor.EditorGUIUtility.labelWidth *= 0.5f;
            SerializedProperty keyValuePairProperty = m_keyValuePairsProperty.GetArrayElementAtIndex(index);
            SerializedProperty keyProperty = keyValuePairProperty.FindPropertyRelative("Key");
            SerializedProperty valueProperty = keyValuePairProperty.FindPropertyRelative("Value");

            rect = rect.Pad(0.0f, -UnityEditor.EditorGUIUtility.standardVerticalSpacing);
            float width = 0.5f * rect.width - UnityEditor.EditorGUIUtility.singleLineHeight;
            Rect keyRect = new Rect(rect.x + UnityEditor.EditorGUIUtility.singleLineHeight, rect.y, width, UnityEditor.EditorGUI.GetPropertyHeight(keyProperty));
            Rect valueRect = new Rect(rect.x + 0.5f * rect.width + UnityEditor.EditorGUIUtility.singleLineHeight, rect.y, width, UnityEditor.EditorGUI.GetPropertyHeight(valueProperty));

            UnityEditor.EditorGUI.BeginDisabledGroup(true);
            UnityEditor.EditorGUI.PropertyField(keyRect, keyProperty, true);
            UnityEditor.EditorGUI.EndDisabledGroup();
            UnityEditor.EditorGUI.PropertyField(valueRect, valueProperty, true);
            UnityEditor.EditorGUIUtility.labelWidth = 0.0f;
        }

        private void DrawNoneElement(Rect rect)
        {
            UnityEditor.EditorGUI.LabelField(rect, "Dictionary is Empty");
        }

        private void AddElement(UnityEditorInternal.ReorderableList list)
        {
            SerializedProperty keyProperty = m_keyValuePairProperty.FindPropertyRelative("Key");

            PropertyPrompt.Show
            (
                new GUIContent("Add Dictionary Element"),
                keyProperty,
                property =>
                {
                    object key = keyProperty.GetValue<object>();
                    IDictionary dictionary = m_serializableDictionaryProperty.GetValue<IDictionary>();

                    if (dictionary.Contains(key))
                    {
                        return;
                    }
                    dictionary.Add(key, m_keyValuePairProperty.FindPropertyRelative("Value").GetValue<object>());
                },
                message: "Add an element to the dictionary by specifying a new key. If the key already exists, this will do nothing.",
                okButtonLabel: "Add"
            );
        }
        #endregion
    }
}