using UnityEditor;
using UnityEngine;

namespace Tuntenfisch.Commons.Attributes.Editor
{
    [CustomPropertyDrawer(typeof(FloatColorAttribute))]
    public class FloatColorDrawer : PropertyDrawer
    {
        #region Unity Callbacks
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty r = property.FindPropertyRelative("x");
            SerializedProperty g = property.FindPropertyRelative("y");
            SerializedProperty b = property.FindPropertyRelative("z");
            SerializedProperty a = property.FindPropertyRelative("w"); 

            if (r == null || g == null || b == null)
            {
                EditorGUI.LabelField(position, $"Serialized property {property.displayName} is not support by {nameof(FloatColorAttribute)}.");
                return;
            }

            FloatColorAttribute floatColorAttribute = attribute as FloatColorAttribute;
            Color color = new Color(r.floatValue, g.floatValue, b.floatValue, a != null ? a.floatValue : 1.0f);

            if (floatColorAttribute.IsLinear)
            {
                color.r = Mathf.LinearToGammaSpace(color.r);
                color.g = Mathf.LinearToGammaSpace(color.g);
                color.b = Mathf.LinearToGammaSpace(color.b);
                color.a = Mathf.LinearToGammaSpace(color.a);
            }

            EditorGUI.BeginChangeCheck();
            color = EditorGUI.ColorField(position, label, color);

            if (!EditorGUI.EndChangeCheck())
            {
                return;
            }

            if (floatColorAttribute.IsLinear)
            {
                color.r = Mathf.GammaToLinearSpace(color.r);
                color.g = Mathf.GammaToLinearSpace(color.g);
                color.b = Mathf.GammaToLinearSpace(color.b);
                color.a = Mathf.GammaToLinearSpace(color.a);
            }

            r.floatValue = color.r;
            b.floatValue = color.b;
            g.floatValue = color.g;

            if (a != null)
            {
                a.floatValue = color.a;
            }
        }
        #endregion
    }
}