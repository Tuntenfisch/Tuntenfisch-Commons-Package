using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Tuntenfisch.Commons.Attributes.Shaders.Editor
{
    public class EulerAnglesToAxesDrawer : MaterialPropertyDrawer
    {
        #region Private Fields
        private readonly string m_rightAxisName;
        private readonly string m_upAxisName;
        private readonly string m_forwardAxisName;
        #endregion

        #region Unity Callbacks
        public override void OnGUI(Rect position, MaterialProperty property, string label, MaterialEditor editor)
        {
            EditorGUI.BeginChangeCheck();
            float4 eulerAngles = property.vectorValue;
            eulerAngles.xyz = EditorGUI.Vector3Field(position, label, eulerAngles.xyz);

            if (EditorGUI.EndChangeCheck())
            {
                property.vectorValue = eulerAngles;
                Material material = editor.target as Material;
                Matrix4x4 matrix = Matrix4x4.Rotate(Quaternion.Euler(eulerAngles.xyz));
                material.SetVector(m_rightAxisName, matrix.GetColumn(0));
                material.SetVector(m_upAxisName, matrix.GetColumn(1));
                material.SetVector(m_forwardAxisName, matrix.GetColumn(2));
            }
        }
        #endregion

        #region Public Methods
        public EulerAnglesToAxesDrawer(string rightAxisName, string upAxisName, string forwardAxisName)
        {
            m_rightAxisName = rightAxisName;
            m_upAxisName = upAxisName;
            m_forwardAxisName = forwardAxisName;
        }
        #endregion
    }
}