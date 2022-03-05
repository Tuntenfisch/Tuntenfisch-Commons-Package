using UnityEditor;
using UnityEngine;

namespace Tuntenfisch.Commons.Attributes.Editor
{
    [CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
    public class MinMaxRangeDrawer : PropertyDrawer
    {
        #region Unity Callbacks
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty minRangeProperty = property.FindPropertyRelative("x");
            SerializedProperty maxRangeProperty = property.FindPropertyRelative("y");

            if (minRangeProperty == null || maxRangeProperty == null)
            {
                EditorGUI.LabelField(position, $"{nameof(SerializedProperty)} {property.displayName} is not support by {nameof(MinMaxRangeAttribute)}.");
            }
            else
            {
                EditorGUI.BeginChangeCheck();
                float minRange = minRangeProperty.floatValue;
                float maxRange = maxRangeProperty.floatValue;
                MinMaxRangeAttribute minMaxRangeAttribute = attribute as MinMaxRangeAttribute;
                Commons.Editor.EditorGUI.MinMaxRange(position, label, ref minRange, ref maxRange, minMaxRangeAttribute.MinLimit, minMaxRangeAttribute.MaxLimit);

                if (EditorGUI.EndChangeCheck())
                {
                    minRangeProperty.floatValue = minRange;
                    maxRangeProperty.floatValue = maxRange;
                }
            }
        }
        #endregion
    }
}