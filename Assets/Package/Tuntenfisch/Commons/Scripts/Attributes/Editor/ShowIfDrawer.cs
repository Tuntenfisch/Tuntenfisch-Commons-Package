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

            if (ShouldShow(property, showIfAttribute))
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute showIfAttribute = attribute as ShowIfAttribute;

            if (ShouldShow(property, showIfAttribute))
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
        private bool ShouldShow(SerializedProperty property, ShowIfAttribute showIfAttribute)
        {
            SerializedProperty serializedProperty = property.serializedObject.FindProperty(showIfAttribute.FieldName);

            switch (serializedProperty.propertyType)
            {
                case SerializedPropertyType.Boolean:
                    return serializedProperty.boolValue == (bool)showIfAttribute.Value;

                case SerializedPropertyType.Float:
                    return serializedProperty.floatValue == (float)showIfAttribute.Value;

                case SerializedPropertyType.String:
                    return serializedProperty.stringValue == (string)showIfAttribute.Value;

                case SerializedPropertyType.Enum:
                    return serializedProperty.enumValueIndex == Array.IndexOf(Enum.GetValues(showIfAttribute.Value.GetType()), showIfAttribute.Value);

                default: 
                    return true;
            };
        }
        #endregion
    }
}