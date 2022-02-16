using System;
using UnityEditor;
using UnityEngine;

namespace Tuntenfisch.Commons.Attributes.Editor
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : PropertyDrawer
    {
        #region Unity Callbacks
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute showIfAttribute = attribute as ShowIfAttribute;

            if (Show(property, showIfAttribute))
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute showIfAttribute = attribute as ShowIfAttribute;

            if (Show(property, showIfAttribute))
            {
                return base.GetPropertyHeight(property, label);
            }
            else
            {
                return 0.0f;
            }
        }
        #endregion

        #region Private Methods
        private bool Show(SerializedProperty property, ShowIfAttribute showIfAttribute)
        {
            SerializedProperty serializedProperty = property.serializedObject.FindProperty(showIfAttribute.FieldName);

            return serializedProperty.propertyType switch
            {
                SerializedPropertyType.Boolean => serializedProperty.boolValue == (bool)showIfAttribute.Value,
                SerializedPropertyType.Float => serializedProperty.floatValue == (float)showIfAttribute.Value,
                SerializedPropertyType.String => serializedProperty.stringValue == (string)showIfAttribute.Value,
                SerializedPropertyType.Enum => serializedProperty.enumValueIndex == Array.IndexOf(Enum.GetValues(showIfAttribute.Value.GetType()), showIfAttribute.Value),
                _ => true,
            };
        }
        #endregion
    }
}