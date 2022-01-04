using UnityEditor;
using UnityEngine;

namespace Tuntenfisch.Commons.Coupling.Scriptables.Variables.Editor
{
    [CustomPropertyDrawer(typeof(Variable<>), useForChildren: true)]
    public class VariableDrawer : PropertyDrawer
    {
        #region Unity Callbacks
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Commons.Editor.EditorGUI.GenericObjectField(position, property, fieldInfo.FieldType, label);
        }
        #endregion
    }
}