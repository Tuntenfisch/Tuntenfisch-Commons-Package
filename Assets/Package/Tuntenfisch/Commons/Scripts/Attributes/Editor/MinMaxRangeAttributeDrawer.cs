using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Tuntenfisch.Commons.Attributes.Editor
{
    [CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
    public class MinMaxRangeAttributeDrawer : PropertyDrawer
    {
        #region Unity Callbacks
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty x = property.FindPropertyRelative("x");
            SerializedProperty y = property.FindPropertyRelative("y");

            if (x == null || y == null)
            {
                EditorGUI.LabelField(position, $"{nameof(SerializedProperty)} {property.displayName} is not support by {nameof(MinMaxRangeAttribute)}.");
            }
            else
            {
                Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
                float remainingWidth = position.width - labelRect.width - 2.0f * (EditorGUIUtility.fieldWidth + Commons.Editor.EditorGUI.HorizontalPadding);
                Rect minValueRect = new Rect(labelRect.xMax, position.y, EditorGUIUtility.fieldWidth, position.height);
                Rect minMaxSliderRect = new Rect(minValueRect.xMax + Commons.Editor.EditorGUI.HorizontalPadding, position.y, remainingWidth, position.height);
                Rect maxValueRect = new Rect(minMaxSliderRect.xMax + Commons.Editor.EditorGUI.HorizontalPadding, position.y, EditorGUIUtility.fieldWidth, position.height);

                MinMaxRangeAttribute minMaxRangeAttribute = attribute as MinMaxRangeAttribute;
                float2 value = new float2(x.floatValue, y.floatValue);
                EditorGUI.LabelField(labelRect, property.displayName);
                EditorGUI.MinMaxSlider(minMaxSliderRect, GUIContent.none, ref value.x, ref value.y, minMaxRangeAttribute.Min, minMaxRangeAttribute.Max);
                value.x = EditorGUI.FloatField(minValueRect, GUIContent.none, value.x);
                value.y = EditorGUI.FloatField(maxValueRect, GUIContent.none, value.y);
                x.floatValue = math.clamp(value.x, minMaxRangeAttribute.Min, value.y);
                y.floatValue = math.clamp(value.y, value.x, minMaxRangeAttribute.Max);
            }
        }
        #endregion
    }
}