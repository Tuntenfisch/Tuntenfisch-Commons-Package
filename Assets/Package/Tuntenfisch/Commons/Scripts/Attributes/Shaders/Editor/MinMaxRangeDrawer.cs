using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Tuntenfisch.Commons.Attributes.Shaders.Editor
{
    public class MinMaxRangeDrawer : MaterialPropertyDrawer
    {
        #region Private Fields
        private MinMaxRangeAttribute m_info;
        #endregion

        #region Unity Callbacks
        public override void OnGUI(Rect position, MaterialProperty property, string label, MaterialEditor editor)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUIUtility.labelWidth = 0.0f;
            float4 minMaxRange = property.vectorValue;
            Commons.Editor.EditorGUI.MinMaxRange(position, new GUIContent(label), ref minMaxRange.x, ref minMaxRange.y, m_info.MinLimit, m_info.MaxLimit);

            if (EditorGUI.EndChangeCheck())
            {
                property.vectorValue = minMaxRange;
            }
        }
        #endregion

        #region Public Methods
        public MinMaxRangeDrawer(float min, float max)
        {
            m_info = new MinMaxRangeAttribute(min, max);
        }
        #endregion
    }
}