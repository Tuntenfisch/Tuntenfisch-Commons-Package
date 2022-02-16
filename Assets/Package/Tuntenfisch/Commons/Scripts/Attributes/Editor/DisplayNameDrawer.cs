using UnityEditor;
using UnityEngine;

namespace Tuntenfisch.Commons.Attributes.Editor
{
    [CustomPropertyDrawer(typeof(DisplayNameAttribute))]
    public class DisplayNameDrawer : PropertyDrawer
    {
        #region Unity Callbacks
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DisplayNameAttribute displayNameAttribute = attribute as DisplayNameAttribute;
            label.text = displayNameAttribute.DisplayName;
            EditorGUI.PropertyField(position, property, label);
        }
        #endregion
    }
}